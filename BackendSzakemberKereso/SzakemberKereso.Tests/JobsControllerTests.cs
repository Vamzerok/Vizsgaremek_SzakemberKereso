using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using SzakemberKereso.Controllers;
using SzakemberKereso.DTOs.Job;
using SzakemberKereso.DTOs.Pricing;
using SzakemberKereso.DTOs.TimeInterval;
using SzakemberKereso.Models;
using SzakemberKereso;
using SzakemberKereso.DTOs.ResidentialAddress;
using SzakemberKereso.DTOs.Settlement;
using SzakemberKereso.Services;

namespace SzakemberKereso.Tests
{
    public class JobsControllerTests
    {
        private const int InitiatorId = 1;
        private const int ExpertUserId = 2;
        private const int UnrelatedUserId = 3;
        private const int ServiceId = 1;
        private const int LocationId = 1;
        private const int SpecialtyId = 1;

        static JobsControllerTests()
        {
            Program.ConfigureMapster();
        }

        private static Context CreateContext() =>
            new Context(new DbContextOptionsBuilder<Context>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options);

        private static JobsController CreateController(Context context, int userId)
        {
            var controller = new JobsController(context, new AddressService(context), new TimeIntervalService(context), new InputSettlementDtoValidator());
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, userId.ToString())
                    }))
                }
            };
            return controller;
        }

        private static void SeedBaseData(Context context)
        {
            context.Users.Add(new User { Id = InitiatorId, UserName = "initiator@test.com", FirstName = "Test", LastName = "User" });
            context.Users.Add(new User { Id = ExpertUserId, UserName = "expert@test.com", FirstName = "Test", LastName = "Expert" });
            context.Experts.Add(new Expert { UserId = ExpertUserId, CompanyEmail = "test.expert@gmail.com", CompanyPhoneNumber = "06301234567" });
            context.Settlements.Add(new Settlement { Id = 1, Name = "Szombathely", PostalCode = 9700, CountyName = "Vas" });
            context.Pricings.Add(new Pricing { Id = 1, PricingType = PricingType.Fixed, FixedPrice = 10000 });
            context.ExpertSpecialties.Add(new ExpertSpecialty { Id = SpecialtyId, ExpertId = ExpertUserId, OccupationId = 1 });
            context.Services.Add(new Service { Id = ServiceId, Name = "Test Service", ExpertSpecialtyId = SpecialtyId, PricingId = 1 });
            context.ResidentialAddresses.Add(new ResidentialAddress { Id = LocationId, StreetName = "Zrínyi Ilona", BuildingNumber = 12, PublicAreaType = "utca", SettlementId = 1 });
            context.SaveChanges();
        }

        private static InputResidentialAddressDto ValidLocation() => new InputResidentialAddressDto
        {
            Settlement = new InputSettlementDto { Name = "Szombathely", PostalCode = 9700 },
            StreetAddress = "Zrínyi Ilona utca 12",
        };

        private static Job SeedJob(Context context, JobStatus status, int? offerPricingId = null, List<TimeInterval>? extraIntervals = null)
        {
            var intervals = new List<TimeInterval>
            {
                new TimeInterval
                {
                    Type = TimeIntervalType.Available,
                    Date = new DateOnly(2030, 6, 1),
                    StartTime = new TimeOnly(9, 0),
                    EndTime = new TimeOnly(11, 0)
                }
            };
            if (extraIntervals != null) intervals.AddRange(extraIntervals);

            var job = new Job
            {
                Status = status,
                InitiatingUserId = InitiatorId,
                ServiceId = ServiceId,
                LocationId = LocationId,
                PricingId = offerPricingId,
                TimeIntervals = intervals
            };
            context.Jobs.Add(job);
            context.SaveChanges();
            return job;
        }

        private static void SeedOfferPricing(Context context)
        {
            context.Pricings.Add(new Pricing { Id = 2, PricingType = PricingType.Fixed, FixedPrice = 20000 });
            context.SaveChanges();
        }

        private static InputTimeIntervalDto FutureInterval() => new InputTimeIntervalDto
        {
            Date = new DateOnly(2030, 6, 1),
            StartTime = new TimeOnly(10, 0),
            EndTime = new TimeOnly(12, 0)
        };

        private static MakeOfferDto ValidOfferDto() => new MakeOfferDto
        {
            Pricing = new PricingDto { PricingType = PricingType.Fixed, FixedPrice = 20000 },
            OfferedTimeIntervals = new List<InputTimeIntervalDto> { FutureInterval() }
        };

        #region CreateJob

        [Fact]
        public async Task CreateJob_ValidDto_ReturnsCreated()
        {
            var context = CreateContext();
            SeedBaseData(context);
            var controller = CreateController(context, InitiatorId);

            var dto = new CreateJobDto
            {
                ServiceId = ServiceId,
                Title = "Paint job for classroom 13.A",
                Description = "classroom 13.A needs a new paint job :D",
                Location = ValidLocation(),
                AvailableTimeIntervals = new List<InputTimeIntervalDto> { FutureInterval() }
            };

            var result = await controller.CreateJob(dto);

            var created = Assert.IsType<CreatedAtActionResult>(result.Result);
            var job = Assert.IsType<OutputJobDto>(created.Value);
            Assert.Equal(JobStatus.Pending, job.Status);
            Assert.Equal("Paint job for classroom 13.A", job.Title);
            Assert.Equal("classroom 13.A needs a new paint job :D", job.Description);
        }

        [Fact]
        public async Task CreateJob_EmptyTitle_ReturnsBadRequest()
        {
            var context = CreateContext();
            SeedBaseData(context);
            var controller = CreateController(context, InitiatorId);

            var dto = new CreateJobDto
            {
                ServiceId = ServiceId,
                Title = "",
                Location = ValidLocation(),
                AvailableTimeIntervals = new List<InputTimeIntervalDto> { FutureInterval() }
            };

            var result = await controller.CreateJob(dto);

            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task CreateJob_NoTimeIntervals_ReturnsBadRequest()
        {
            var context = CreateContext();
            SeedBaseData(context);
            var controller = CreateController(context, InitiatorId);

            var dto = new CreateJobDto
            {
                ServiceId = ServiceId,
                Title = "Test job",
                Location = ValidLocation(),
                AvailableTimeIntervals = new List<InputTimeIntervalDto>()
            };

            var result = await controller.CreateJob(dto);

            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task CreateJob_OverlappingIntervals_ReturnsBadRequest()
        {
            var context = CreateContext();
            SeedBaseData(context);
            var controller = CreateController(context, InitiatorId);

            var dto = new CreateJobDto
            {
                ServiceId = ServiceId,
                Title = "Test job",
                Location = ValidLocation(),
                AvailableTimeIntervals = new List<InputTimeIntervalDto>
                {
                    new InputTimeIntervalDto { Date = new DateOnly(2030, 6, 1), StartTime = new TimeOnly(9, 0), EndTime = new TimeOnly(11, 0) },
                    new InputTimeIntervalDto { Date = new DateOnly(2030, 6, 1), StartTime = new TimeOnly(10, 0), EndTime = new TimeOnly(12, 0) }
                }
            };

            var result = await controller.CreateJob(dto);

            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task CreateJob_ServiceNotFound_ReturnsNotFound()
        {
            var context = CreateContext();
            SeedBaseData(context);
            var controller = CreateController(context, InitiatorId);

            var dto = new CreateJobDto
            {
                ServiceId = 999,
                Title = "Test job",
                Location = ValidLocation(),
                AvailableTimeIntervals = new List<InputTimeIntervalDto> { FutureInterval() }
            };

            var result = await controller.CreateJob(dto);

            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public async Task CreateJob_SettlementNotFound_ReturnsNotFound()
        {
            var context = CreateContext();
            SeedBaseData(context);
            var controller = CreateController(context, InitiatorId);

            var dto = new CreateJobDto
            {
                ServiceId = ServiceId,
                Title = "Test job",
                Location = new InputResidentialAddressDto { Settlement = new InputSettlementDto() { PostalCode = 9999, Name = "Nonexistent" }, StreetAddress = "Zrínyi Ilona utca 12" },
                AvailableTimeIntervals = new List<InputTimeIntervalDto> { FutureInterval() }
            };

            var result = await controller.CreateJob(dto);

            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task CreateJob_DuplicateAddress_ReusesExistingRecord()
        {
            var context = CreateContext();
            SeedBaseData(context);
            var controller = CreateController(context, InitiatorId);

            var dto = new CreateJobDto
            {
                ServiceId = ServiceId,
                Title = "Test job",
                Location = ValidLocation(),
                AvailableTimeIntervals = new List<InputTimeIntervalDto> { FutureInterval() }
            };

            await controller.CreateJob(dto);
            await controller.CreateJob(dto);

            Assert.Equal(1, context.ResidentialAddresses.Count());
        }

        [Fact]
        public async Task CreateJob_EndBeforeStart_ReturnsBadRequest()
        {
            var context = CreateContext();
            SeedBaseData(context);
            var controller = CreateController(context, InitiatorId);

            var dto = new CreateJobDto
            {
                ServiceId = ServiceId,
                Title = "Test job",
                Location = ValidLocation(),
                AvailableTimeIntervals = new List<InputTimeIntervalDto>
                {
                    new InputTimeIntervalDto { Date = new DateOnly(2026, 4, 30), StartTime = new TimeOnly(12, 0), EndTime = new TimeOnly(11, 0) }
                }
            };

            var result = await controller.CreateJob(dto);

            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        #endregion

        #region GetJob
        [Fact]
        public async Task GetJob_CallerIsInitiator_ReturnsOk()
        {
            var context = CreateContext();
            SeedBaseData(context);
            var job = SeedJob(context, JobStatus.Pending);
            var controller = CreateController(context, InitiatorId);

            var result = await controller.GetJob(job.Id);

            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetJob_CallerIsExpert_ReturnsOk()
        {
            var context = CreateContext();
            SeedBaseData(context);
            var job = SeedJob(context, JobStatus.Pending);
            var controller = CreateController(context, ExpertUserId);

            var result = await controller.GetJob(job.Id);

            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetJob_CallerNotParty_ReturnsForbid()
        {
            var context = CreateContext();
            SeedBaseData(context);
            var job = SeedJob(context, JobStatus.Pending);
            var controller = CreateController(context, UnrelatedUserId);

            var result = await controller.GetJob(job.Id);

            Assert.IsType<ForbidResult>(result.Result);
        }

        [Fact]
        public async Task GetJob_JobNotFound_ReturnsNotFound()
        {
            var context = CreateContext();
            SeedBaseData(context);
            var controller = CreateController(context, InitiatorId);

            var result = await controller.GetJob(999);

            Assert.IsType<NotFoundObjectResult>(result.Result);
        }
        #endregion

        #region MakeOffer
        [Fact]
        public async Task MakeOffer_ValidOffer_ReturnsOffered()
        {
            var context = CreateContext();
            SeedBaseData(context);
            var job = SeedJob(context, JobStatus.Pending);
            var controller = CreateController(context, ExpertUserId);

            var result = await controller.MakeOffer(job.Id, ValidOfferDto());

            var ok = Assert.IsType<OkObjectResult>(result.Result);
            var dto = Assert.IsType<OutputJobDto>(ok.Value);
            Assert.Equal(JobStatus.Offered, dto.Status);
        }

        [Fact]
        public async Task MakeOffer_NoTimeIntervals_ReturnsBadRequest()
        {
            var context = CreateContext();
            SeedBaseData(context);
            var job = SeedJob(context, JobStatus.Pending);
            var controller = CreateController(context, ExpertUserId);

            var offerDto = new MakeOfferDto
            {
                Pricing = new PricingDto { PricingType = PricingType.Fixed, FixedPrice = 20000 },
                OfferedTimeIntervals = new List<InputTimeIntervalDto>()
            };

            var result = await controller.MakeOffer(job.Id, offerDto);

            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task MakeOffer_OverlappingIntervals_ReturnsBadRequest()
        {
            var context = CreateContext();
            SeedBaseData(context);
            var job = SeedJob(context, JobStatus.Pending);
            var controller = CreateController(context, ExpertUserId);

            var offerDto = new MakeOfferDto
            {
                Pricing = new PricingDto { PricingType = PricingType.Fixed, FixedPrice = 20000 },
                OfferedTimeIntervals = new List<InputTimeIntervalDto>
                {
                    new InputTimeIntervalDto { Date = new DateOnly(2030, 6, 1), StartTime = new TimeOnly(9, 0), EndTime = new TimeOnly(11, 0) },
                    new InputTimeIntervalDto { Date = new DateOnly(2030, 6, 1), StartTime = new TimeOnly(10, 0), EndTime = new TimeOnly(12, 0) }
                }
            };

            var result = await controller.MakeOffer(job.Id, offerDto);

            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task MakeOffer_JobNotPending_ReturnsConflict()
        {
            var context = CreateContext();
            SeedBaseData(context);
            SeedOfferPricing(context);
            var job = SeedJob(context, JobStatus.Offered, offerPricingId: 2,
                extraIntervals: new List<TimeInterval>
                {
                    new TimeInterval { Type = TimeIntervalType.Offered, Date = new DateOnly(2030, 6, 1), StartTime = new TimeOnly(10, 0), EndTime = new TimeOnly(11, 0) }
                });
            var controller = CreateController(context, ExpertUserId);

            var result = await controller.MakeOffer(job.Id, ValidOfferDto());

            Assert.IsType<ConflictObjectResult>(result.Result);
        }

        [Fact]
        public async Task MakeOffer_CallerIsInitiator_ReturnsConflict()
        {
            var context = CreateContext();
            SeedBaseData(context);
            var job = SeedJob(context, JobStatus.Pending);
            // InitiatorId is both the job creator and the caller trying to offer
            var controller = CreateController(context, InitiatorId);

            var result = await controller.MakeOffer(job.Id, ValidOfferDto());

            Assert.IsType<ConflictObjectResult>(result.Result);
        }

        [Fact]
        public async Task MakeOffer_CallerNotExpert_ReturnsForbid()
        {
            var context = CreateContext();
            SeedBaseData(context);
            var job = SeedJob(context, JobStatus.Pending);
            var controller = CreateController(context, UnrelatedUserId);

            var result = await controller.MakeOffer(job.Id, ValidOfferDto());

            Assert.IsType<ForbidResult>(result.Result);
        }

        [Fact]
        public async Task MakeOffer_EndBeforeStart_ReturnsBadRequest()
        {
            var context = CreateContext();
            SeedBaseData(context);
            var job = SeedJob(context, JobStatus.Pending);
            var controller = CreateController(context, ExpertUserId);

            var offerDto = new MakeOfferDto
            {
                Pricing = new PricingDto { PricingType = PricingType.Fixed, FixedPrice = 20000 },
                OfferedTimeIntervals = new List<InputTimeIntervalDto>
                {
                    new InputTimeIntervalDto { Date = new DateOnly(2026, 4, 30), StartTime = new TimeOnly(12, 0), EndTime = new TimeOnly(11, 0) }
                }
            };

            var result = await controller.MakeOffer(job.Id, offerDto);

            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        #endregion

        #region AcceptOffer

        [Fact]
        public async Task AcceptOffer_ValidAcceptance_ReturnsAccepted()
        {
            var context = CreateContext();
            SeedBaseData(context);
            SeedOfferPricing(context);
            var job = SeedJob(context, JobStatus.Offered, offerPricingId: 2,
                extraIntervals: new List<TimeInterval>
                {
                    new TimeInterval { Type = TimeIntervalType.Offered, Date = new DateOnly(2030, 6, 1), StartTime = new TimeOnly(10, 0), EndTime = new TimeOnly(11, 0) }
                });
            var controller = CreateController(context, InitiatorId);

            var result = await controller.AcceptOffer(job.Id);

            var ok = Assert.IsType<OkObjectResult>(result.Result);
            var dto = Assert.IsType<OutputJobDto>(ok.Value);
            Assert.Equal(JobStatus.Accepted, dto.Status);
        }

        [Fact]
        public async Task AcceptOffer_JobNotOffered_ReturnsConflict()
        {
            var context = CreateContext();
            SeedBaseData(context);
            var job = SeedJob(context, JobStatus.Pending);
            var controller = CreateController(context, InitiatorId);

            var result = await controller.AcceptOffer(job.Id);

            Assert.IsType<ConflictObjectResult>(result.Result);
        }

        [Fact]
        public async Task AcceptOffer_CallerNotInitiator_ReturnsForbid()
        {
            var context = CreateContext();
            SeedBaseData(context);
            SeedOfferPricing(context);
            var job = SeedJob(context, JobStatus.Offered, offerPricingId: 2,
                extraIntervals: new List<TimeInterval>
                {
                    new TimeInterval { Type = TimeIntervalType.Offered, Date = new DateOnly(2030, 6, 1), StartTime = new TimeOnly(10, 0), EndTime = new TimeOnly(11, 0) }
                });
            var controller = CreateController(context, UnrelatedUserId);

            var result = await controller.AcceptOffer(job.Id);

            Assert.IsType<ForbidResult>(result.Result);
        }

        #endregion

        #region CompleteJob

        [Fact]
        public async Task CompleteJob_AfterOfferedTime_ReturnsCompleted()
        {
            var context = CreateContext();
            SeedBaseData(context);
            SeedOfferPricing(context);
            // Offered interval in the past so the time check passes
            var job = SeedJob(context, JobStatus.Accepted, offerPricingId: 2,
                extraIntervals: new List<TimeInterval>
                {
                    new TimeInterval { Type = TimeIntervalType.Offered, Date = new DateOnly(2020, 1, 1), StartTime = new TimeOnly(9, 0), EndTime = new TimeOnly(10, 0) }
                });
            var controller = CreateController(context, ExpertUserId);

            var result = await controller.CompleteJob(job.Id);

            var ok = Assert.IsType<OkObjectResult>(result.Result);
            var dto = Assert.IsType<OutputJobDto>(ok.Value);
            Assert.Equal(JobStatus.Completed, dto.Status);
        }

        [Fact]
        public async Task CompleteJob_JobNotAccepted_ReturnsConflict()
        {
            var context = CreateContext();
            SeedBaseData(context);
            var job = SeedJob(context, JobStatus.Pending);
            var controller = CreateController(context, ExpertUserId);

            var result = await controller.CompleteJob(job.Id);

            Assert.IsType<ConflictObjectResult>(result.Result);
        }

        [Fact]
        public async Task CompleteJob_CallerNotExpert_ReturnsForbid()
        {
            var context = CreateContext();
            SeedBaseData(context);
            SeedOfferPricing(context);
            var job = SeedJob(context, JobStatus.Accepted, offerPricingId: 2,
                extraIntervals: new List<TimeInterval>
                {
                    new TimeInterval { Type = TimeIntervalType.Offered, Date = new DateOnly(2020, 1, 1), StartTime = new TimeOnly(9, 0), EndTime = new TimeOnly(10, 0) }
                });
            var controller = CreateController(context, UnrelatedUserId);

            var result = await controller.CompleteJob(job.Id);

            Assert.IsType<ForbidResult>(result.Result);
        }

        #endregion

        #region CancelJob

        [Fact]
        public async Task CancelJob_InitiatorCancelsPending_ReturnsCancelled()
        {
            var context = CreateContext();
            SeedBaseData(context);
            var job = SeedJob(context, JobStatus.Pending);
            var controller = CreateController(context, InitiatorId);

            var result = await controller.CancelJob(job.Id);

            var ok = Assert.IsType<OkObjectResult>(result.Result);
            var dto = Assert.IsType<OutputJobDto>(ok.Value);
            Assert.Equal(JobStatus.Cancelled, dto.Status);
        }

        [Fact]
        public async Task CancelJob_ExpertCancelsOffered_ReturnsCancelled()
        {
            var context = CreateContext();
            SeedBaseData(context);
            SeedOfferPricing(context);
            var job = SeedJob(context, JobStatus.Offered, offerPricingId: 2,
                extraIntervals: new List<TimeInterval>
                {
                    new TimeInterval { Type = TimeIntervalType.Offered, Date = new DateOnly(2030, 6, 1), StartTime = new TimeOnly(10, 0), EndTime = new TimeOnly(11, 0) }
                });
            var controller = CreateController(context, ExpertUserId);

            var result = await controller.CancelJob(job.Id);

            var ok = Assert.IsType<OkObjectResult>(result.Result);
            var dto = Assert.IsType<OutputJobDto>(ok.Value);
            Assert.Equal(JobStatus.Cancelled, dto.Status);
        }

        [Fact]
        public async Task CancelJob_JobAlreadyCompleted_ReturnsConflict()
        {
            var context = CreateContext();
            SeedBaseData(context);
            var job = SeedJob(context, JobStatus.Completed);
            var controller = CreateController(context, InitiatorId);

            var result = await controller.CancelJob(job.Id);

            Assert.IsType<ConflictObjectResult>(result.Result);
        }

        [Fact]
        public async Task CancelJob_CallerNotParty_ReturnsForbid()
        {
            var context = CreateContext();
            SeedBaseData(context);
            var job = SeedJob(context, JobStatus.Pending);
            var controller = CreateController(context, UnrelatedUserId);

            var result = await controller.CancelJob(job.Id);

            Assert.IsType<ForbidResult>(result.Result);
        }

        #endregion

        #region RateJob

        [Fact]
        public async Task RateJob_ValidRating_SetsRating()
        {
            var context = CreateContext();
            SeedBaseData(context);
            var job = SeedJob(context, JobStatus.Completed);
            var controller = CreateController(context, InitiatorId);

            var result = await controller.RateJob(job.Id, 4.5f);

            var ok = Assert.IsType<OkObjectResult>(result.Result);
            var dto = Assert.IsType<OutputJobDto>(ok.Value);
            Assert.Equal(4.5f, dto.Rating);
        }

        [Fact]
        public async Task RateJob_RatingTooLow_ReturnsBadRequest()
        {
            var context = CreateContext();
            SeedBaseData(context);
            var job = SeedJob(context, JobStatus.Completed);
            var controller = CreateController(context, InitiatorId);

            var result = await controller.RateJob(job.Id, 0f);

            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task RateJob_RatingTooHigh_ReturnsBadRequest()
        {
            var context = CreateContext();
            SeedBaseData(context);
            var job = SeedJob(context, JobStatus.Completed);
            var controller = CreateController(context, InitiatorId);

            var result = await controller.RateJob(job.Id, 6f);

            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task RateJob_JobNotCompleted_ReturnsConflict()
        {
            var context = CreateContext();
            SeedBaseData(context);
            var job = SeedJob(context, JobStatus.Accepted);
            var controller = CreateController(context, InitiatorId);

            var result = await controller.RateJob(job.Id, 3f);

            Assert.IsType<ConflictObjectResult>(result.Result);
        }

        [Fact]
        public async Task RateJob_CallerNotInitiator_ReturnsForbid()
        {
            var context = CreateContext();
            SeedBaseData(context);
            var job = SeedJob(context, JobStatus.Completed);
            var controller = CreateController(context, UnrelatedUserId);

            var result = await controller.RateJob(job.Id, 3f);

            Assert.IsType<ForbidResult>(result.Result);
        }

        [Fact]
        public async Task RateJob_AlreadyRated_ReturnsConflict()
        {
            var context = CreateContext();
            SeedBaseData(context);
            var job = SeedJob(context, JobStatus.Completed);
            job.Rating = 3f;
            context.SaveChanges();
            var controller = CreateController(context, InitiatorId);

            var result = await controller.RateJob(job.Id, 4f);

            Assert.IsType<ConflictObjectResult>(result.Result);
        }

        #endregion
    }
}
