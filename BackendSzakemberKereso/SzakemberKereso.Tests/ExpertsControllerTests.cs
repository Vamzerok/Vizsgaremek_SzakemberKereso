using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using SzakemberKereso.Controllers;
using SzakemberKereso.DTOs.Expert;
using SzakemberKereso.DTOs.Pricing;
using SzakemberKereso.DTOs.Service;
using SzakemberKereso.Models;

namespace SzakemberKereso.Tests
{
    public class ExpertsControllerTests
    {
        private Context CreateContext()
        {
            var options = new DbContextOptionsBuilder<Context>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new Context(options);
        }

        private Mock<UserManager<User>> CreateMockUserManager()
        {
            var store = new Mock<IUserStore<User>>();
            return new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
        }

        private ExpertsController CreateController(Context context, int userId, string role = "Expert")
        {
            var mockUM = CreateMockUserManager();
            var contextAccessor = new Mock<IHttpContextAccessor>();
            var claimsFactory = new Mock<IUserClaimsPrincipalFactory<User>>();
            var mockSM = new Mock<SignInManager<User>>(mockUM.Object, contextAccessor.Object, claimsFactory.Object, null, null, null, null);

            var mockValidator = new Mock<IValidator<InputServiceDto>>();
            mockValidator.Setup(v => v.ValidateAsync(It.IsAny<InputServiceDto>(), default))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult());

            var controller = new ExpertsController(context, mockUM.Object, mockSM.Object, mockValidator.Object);

            var claims = new List<Claim> {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            new Claim(ClaimTypes.Role, role)
        };
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal(new ClaimsIdentity(claims)) }
            };

            return controller;
        }

        [Fact]
        public async Task GetMe_ExpertExists_ReturnsOk()
        {
            var context = CreateContext();

            var testUser = new User { Id = 1, UserName = "teszt@teszt.hu", FirstName = "Test", LastName = "User" };
            context.Users.Add(testUser);

            context.Experts.Add(new Expert
            {
                UserId = 1,
                User = testUser,
                Biography = "Profi szaki",
                CompanyEmail = "test.expert@gmail.com",
                CompanyPhoneNumber = "06301234567"
            });

            context.SaveChanges();

            var controller = CreateController(context, 1);

            var result = await controller.GetMe();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var dto = Assert.IsType<OutputExpertDto>(okResult.Value);
            Assert.Equal("Profi szaki", dto.Biography);
        }

        [Fact]
        public async Task GetMe_NotExpert_ReturnsNotFound()
        {
            var context = CreateContext(); 
            var controller = CreateController(context, 1);

            var result = await controller.GetMe();

            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public async Task CreateExpertSpecialty_NewOccupation_ReturnsCreated()
        {
            var context = CreateContext();
            context.Occupations.Add(new Occupation { Id = 10, Name = "Vízszerelő", Description = "Vízszerelő" });
            context.SaveChanges();
            var controller = CreateController(context, 1);

            var result = await controller.CreateExpertSpecialty(10);

            Assert.IsType<CreatedResult>(result);
            Assert.True(context.ExpertSpecialties.Any(es => es.ExpertId == 1 && es.OccupationId == 10));
        }

        [Fact]
        public async Task CreateService_ValidData_ReturnsCreated()
        {
            var context = CreateContext();

            var testUser = new User { Id = 1, UserName = "expert@test.com", FirstName = "Test", LastName = "User" };
            context.Users.Add(testUser);

            var expert = new Expert { UserId = 1, Biography = "Test", User = testUser, CompanyEmail = "test.expert@gmail.com", CompanyPhoneNumber = "06301234567" };
            var occupation = new Occupation { Id = 10, Name = "Szerelő", Description = "Teszt" };

            context.Experts.Add(expert);
            context.Occupations.Add(occupation);

            context.ExpertSpecialties.Add(new ExpertSpecialty
            {
                Id = 5,
                ExpertId = 1,
                OccupationId = 10,
                Expert = expert,
                Occupation = occupation
            });

            context.SaveChanges();
            var controller = CreateController(context, 1);

            var serviceDto = new InputServiceDto
            {
                Name = "Csaptelep csere",
                Pricing = new PricingDto
                {
                    PricingType = PricingType.Fixed,
                    FixedPrice = 5000
                }
            };

            var result = await controller.CreateService(serviceDto, 10);

            var createdResult = Assert.IsAssignableFrom<ObjectResult>(result.Result);
            Assert.Equal(201, createdResult.StatusCode);
            Assert.NotNull(createdResult.Value);
        }

        [Fact]
        public async Task DeleteService_NotOwned_ReturnsNotFound()
        {
            var context = CreateContext();
            context.ExpertSpecialties.Add(new ExpertSpecialty { Id = 1, ExpertId = 1, OccupationId = 10 });
            context.SaveChanges();
            var controller = CreateController(context, 1);

            var result = await controller.DeleteService(10, 999);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task GetExperts_FilterByOccupation_ReturnsOnlyRelevant()
        {
            var context = CreateContext();
            var occupation1 = new Occupation { Id = 1, Name = "Festő", Description = "Festő" };
            var occupation2 = new Occupation { Id = 2, Name = "Asztalos", Description = "Asztalos" };

            var user1 = new User { Id = 1, UserName = "expert1@test.com", FirstName = "Test", LastName = "User" };
            var user2 = new User { Id = 2, UserName = "expert2@test.com", FirstName = "Test", LastName = "Expert" };
            context.Users.AddRange(user1, user2);

            context.Experts.Add(new Expert { UserId = 1, User = user1, CompanyEmail = "test.expert@gmail.com", CompanyPhoneNumber = "06301234567", ExpertSpecialties = new List<ExpertSpecialty> { new ExpertSpecialty { Occupation = occupation1 } } });
            context.Experts.Add(new Expert { UserId = 2, User = user2, CompanyEmail = "test.expert@gmail.com", CompanyPhoneNumber = "06301234567", ExpertSpecialties = new List<ExpertSpecialty> { new ExpertSpecialty { Occupation = occupation2 } } });
            context.SaveChanges();

            var controller = CreateController(context, 0); 
            var filters = new ExpertFilterParams { OccupationId = 1 };

            var result = await controller.GetExperts(filters);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var experts = Assert.IsAssignableFrom<IEnumerable<OutputExpertDto>>(okResult.Value);
            Assert.Single(experts);
        }

        [Fact]
        public async Task GetExperts_FilterByCity_ReturnsCorrectExpert()
        {
            var context = CreateContext();
            var loc = new Settlement { Name = "Budapest", CountyName = "Pest" };

            var user1 = new User { Id = 1, UserName = "expert1@test.com", FirstName = "Test", LastName = "User" };
            var user2 = new User { Id = 2, UserName = "expert2@test.com", FirstName = "Test", LastName = "Expert" };
            context.Users.AddRange(user1, user2);

            context.Experts.Add(new Expert { UserId = 1, User = user1, CompanyEmail = "test.expert@gmail.com", CompanyPhoneNumber = "06301234567", WorkLocation = loc });
            context.Experts.Add(new Expert { UserId = 2, User = user2, CompanyEmail = "test.expert@gmail.com", CompanyPhoneNumber = "06301234567", WorkLocation = new Settlement { Name = "Debrecen", CountyName = "Hajdú-Bihar" } });
            context.SaveChanges();

            var controller = CreateController(context, 0);
            var filters = new ExpertFilterParams { SettlementName = "Budapest" };

            var result = await controller.GetExperts(filters);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var experts = Assert.IsAssignableFrom<IEnumerable<OutputExpertDto>>(okResult.Value);
            Assert.Single(experts);
        }
    }
}
