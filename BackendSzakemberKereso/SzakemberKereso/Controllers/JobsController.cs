using FluentValidation;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using SzakemberKereso.DTOs.Job;
using SzakemberKereso.DTOs.Settlement;
using SzakemberKereso.DTOs.TimeInterval;
using SzakemberKereso.Models;
using SzakemberKereso.Services;

namespace SzakemberKereso.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class JobsController : ControllerBase
    {
        private readonly Context _context;
        private readonly AddressService _addressService;
        private readonly IValidator<InputSettlementDto> _settlementValidator;
        private readonly TimeIntervalService _timeIntervalService;

        public JobsController(Context context, AddressService addressService, TimeIntervalService timeIntervalService, IValidator<InputSettlementDto> settlementValidator)
        {
            _context = context;
            _addressService = addressService;
            _timeIntervalService = timeIntervalService;
            _settlementValidator = settlementValidator;
        }

        // GET: api/jobs/{id}
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<OutputJobDto>> GetJob(int id)
        {
            if (!TryGetLoggedInUserId(out int userId))
                return Unauthorized();

            var job = await LoadJobAsync(id);
            if (job == null) return NotFound($"Job {id} not found.");

            if (!await IsPartyToJobAsync(job, userId))
                return Forbid();

            return Ok(job.Adapt<OutputJobDto>());
        }

        // POST: api/jobs  state: pending
        [HttpPost]
        [Authorize(Roles = "Generic")]
        public async Task<ActionResult<OutputJobDto>> CreateJob([FromBody] CreateJobDto dto)
        {
            if (!TryGetLoggedInUserId(out int userId))
                return Unauthorized();

            var settlementValidation = await _settlementValidator.ValidateAsync(dto.Location.Settlement); 
            if (!settlementValidation.IsValid)
                return BadRequest(settlementValidation.Errors.Select(e => new { property = e.PropertyName, error = e.ErrorMessage }));

            if (string.IsNullOrWhiteSpace(dto.Title))
                return BadRequest(new { error = "A cím megadása kötelező." });

            if (!dto.AvailableTimeIntervals.Any())
                return BadRequest(new { error = "Legalább egy elérhetőségi időpontot meg kell adni." });

            if (dto.AvailableTimeIntervals.Any(t => !_timeIntervalService.IsValid(t)))
                return BadRequest(new { error = "Az egyik időpont befejezési ideje korábbi mint a kezdésének ideje."});

            if (_timeIntervalService.HasInternalCollisions(dto.AvailableTimeIntervals))
                return BadRequest(new { error = "A megadott időszakok nem ütközhetnek egymással."});

            if (!await _context.Services.AnyAsync(s => s.Id == dto.ServiceId))
                return NotFound(new { error = "A szolgáltatás nem található."});

            var locationId = await _addressService.ResolveAsync(dto.Location);
            if (locationId == null)
                return BadRequest(new { error = "A megadott cím nem érvényes."});

            var job = new Job
            {
                Status = JobStatus.Pending,
                InitiatingUserId = userId,
                ServiceId = dto.ServiceId,
                Title = dto.Title,
                Description = dto.Description,
                LocationId = (int)locationId,
                TimeIntervals = dto.AvailableTimeIntervals.Select(t => new TimeInterval
                {
                    Type = TimeIntervalType.Available,
                    Date = t.Date,
                    StartTime = t.StartTime,
                    EndTime = t.EndTime
                }).ToList()
            };

            _context.Jobs.Add(job);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetJob), new { id = job.Id }, job.Adapt<OutputJobDto>());
        }

        // POST: api/jobs/{id}/offer state: pending to offered 
        [HttpPost("{id}/offer")]
        [Authorize(Roles = "Expert")]
        public async Task<ActionResult<OutputJobDto>> MakeOffer(int id, [FromBody] MakeOfferDto dto)
        {
            if (!TryGetLoggedInUserId(out int userId))
                return Unauthorized();

            if (!dto.OfferedTimeIntervals.Any())
                return BadRequest("Legalább egy javasolt időpontot meg kell adni.");

            if (dto.OfferedTimeIntervals.Any(t => !_timeIntervalService.IsValid(t)))
                return BadRequest("Az egyik időpont befejezési ideje korábbi mint a kezdésének ideje.");

            if (_timeIntervalService.HasInternalCollisions(dto.OfferedTimeIntervals))
                return BadRequest("A megadott időszakok nem ütközhetnek egymással.");

            var job = await LoadJobAsync(id);
            if (job == null) return NotFound($"Job {id} not found.");

            if (job.Status != JobStatus.Pending) 
                return Conflict($"A requestet nem lehet teljesíteni a munka jelenlegi állapotában. Jelenlegi állapot: {job.Status}.");

            if (job.InitiatingUserId == userId) 
                return Conflict("Nem tehetsz ajánlatot a saját munkakérésdre.");

            if (!await IsExpertForJobAsync(job, userId)) return Forbid();

            job.Pricing = dto.Pricing.Adapt<Pricing>();
            foreach (var t in dto.OfferedTimeIntervals)
                job.TimeIntervals.Add(new TimeInterval
                {
                    Type = TimeIntervalType.Offered,
                    Date = t.Date,
                    StartTime = t.StartTime,
                    EndTime = t.EndTime
                });
            job.Status = JobStatus.Offered;

            await _context.SaveChangesAsync();

            await _context.Entry(job).Reference(j => j.Pricing).LoadAsync();

            return Ok(job.Adapt<OutputJobDto>());
        }

        // POST: api/jobs/{id}/accept  state: offered to accepted
        [HttpPost("{id}/accept")]
        [Authorize(Roles = "Generic")]
        public async Task<ActionResult<OutputJobDto>> AcceptOffer(int id)
        {
            if (!TryGetLoggedInUserId(out int userId))
                return Unauthorized();

            var job = await LoadJobAsync(id);
            if (job == null) return NotFound($"Job {id} not found.");

            if (job.Status != JobStatus.Offered) 
                return Conflict($"A requestet nem lehet teljesíteni a munka jelenlegi állapotában. Jelenlegi állapot: {job.Status}.");
            if (job.InitiatingUserId != userId) return Forbid();

            job.Status = JobStatus.Accepted;

            await _context.SaveChangesAsync();

            return Ok(job.Adapt<OutputJobDto>());
        }

        // POST: api/jobs/{id}/complete  state: accepted to completed
        [HttpPost("{id}/complete")]
        [Authorize(Roles = "Expert")]
        public async Task<ActionResult<OutputJobDto>> CompleteJob(int id)
        {
            if (!TryGetLoggedInUserId(out int userId))
                return Unauthorized();

            var job = await LoadJobAsync(id);
            if (job == null) return NotFound($"Job {id} not found.");

            if (job.Status != JobStatus.Accepted)
                return Conflict($"A requestet nem lehet teljesíteni a munka jelenlegi állapotában. Jelenlegi állapot: {job.Status}.");

            if (!await IsExpertForJobAsync(job, userId))
                return Forbid();

            var offeredEnd = job.TimeIntervals
                .Where(t => t.Type == TimeIntervalType.Offered)
                .Select(t => t.Date.ToDateTime(t.EndTime))
                .DefaultIfEmpty(DateTime.MinValue)
                .Max();

            //if (DateTime.UtcNow < offeredEnd)   //removed for testing purposes
            //    return BadRequest("Cannot complete a job before all offered time slots have passed.");

            job.Status = JobStatus.Completed;
            await _context.SaveChangesAsync();

            return Ok(job.Adapt<OutputJobDto>());
        }

        // POST: api/jobs/{id}/cancel  state: to cancelled
        [HttpPost("{id}/cancel")]
        public async Task<ActionResult<OutputJobDto>> CancelJob(int id)
        {
            if (!TryGetLoggedInUserId(out int userId))
                return Unauthorized();

            var job = await LoadJobAsync(id);
            if (job == null) return NotFound($"Job {id} not found.");

            var cancellable = new[] { JobStatus.Pending, JobStatus.Offered, JobStatus.Accepted };
            if (!cancellable.Contains(job.Status))
                return Conflict($"A requestet nem lehet teljesíteni a munka jelenlegi állapotában. Jelenlegi állapot: {job.Status} állapotban nem mondható le.");

            if (!await IsPartyToJobAsync(job, userId))
                return Forbid();

            job.CancelledFromStatus = job.Status;
            job.Status = JobStatus.Cancelled;
            await _context.SaveChangesAsync();

            return Ok(job.Adapt<OutputJobDto>());
        }

        // POST: api/jobs/{id}/rate  
        [HttpPost("{id}/rate")]
        [Authorize(Roles = "Generic")]
        public async Task<ActionResult<OutputJobDto>> RateJob(int id, [FromQuery] float rating)
        {
            if (!TryGetLoggedInUserId(out int userId))
                return Unauthorized();

            if (rating < 1 || rating > 5)
                return BadRequest("Az értékelésnek 1 és 5 közötti értéket kell adni.");

            var job = await LoadJobAsync(id);
            if (job == null) return NotFound($"Job {id} not found.");

            if (job.Status != JobStatus.Completed)
                return Conflict($"Csak Befejezett munkát lehet értékelni. Jelenlegi állapot: {job.Status}.");

            if (job.InitiatingUserId != userId)
                return Forbid();

            if (job.Rating.HasValue)
                return Conflict("Ez a munka már értékelve van.");

            job.Rating = rating;
            await _context.SaveChangesAsync();

            return Ok(job.Adapt<OutputJobDto>());
        }

        //helpers
        private bool TryGetLoggedInUserId(out int userId)
        {
            return int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out userId);
        }

        private Task<Job?> LoadJobAsync(int jobId)
        {
            return _context.Jobs
                .Include(j => j.Pricing)
                .Include(j => j.TimeIntervals)
                .Include(j => j.Service)
                    .ThenInclude(s => s.ExpertSpecialty)
                    .ThenInclude(es => es.Expert)
                    .ThenInclude(e => e.User)  
                .Include(j => j.InitiatingUser)
                .Include(j => j.Location).ThenInclude(l => l.Settlement)
                .FirstOrDefaultAsync(j => j.Id == jobId);
        }

        private async Task<bool> IsExpertForJobAsync(Job job, int userId)
        {
            return await _context.Services
                .Where(s => s.Id == job.ServiceId)
                .Include(s => s.ExpertSpecialty)
                .AnyAsync(s => s.ExpertSpecialty.ExpertId == userId);
        }

        private bool IsInitiatingUserForJob(Job job, int userId)
        {
            return job.InitiatingUserId == userId;
        }

        private async Task<bool> IsPartyToJobAsync(Job job, int userId)
        {
            return IsInitiatingUserForJob(job, userId) || await IsExpertForJobAsync(job, userId);
        }

    }
}
