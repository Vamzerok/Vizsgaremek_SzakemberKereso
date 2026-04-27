using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using FluentValidation;
using SzakemberKereso.DTOs.Expert;
using SzakemberKereso.DTOs.Job;
using SzakemberKereso.DTOs.Service;
using SzakemberKereso.Models;

namespace SzakemberKereso.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpertsController : ControllerBase
    {
        private readonly Context _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IValidator<InputServiceDto> _serviceValidator;

        public ExpertsController(Context context, UserManager<User> userManager, SignInManager<User> signInManager, IValidator<InputServiceDto> serviceValidator)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _serviceValidator = serviceValidator;
        }

        // GET: api/experts/me
        [HttpGet("me")]
        [Authorize(Roles = "Expert")]
        public async Task<ActionResult<OutputExpertDto>> GetMe()
        {
            if (!TryGetLoggedInUserId(out int userId))
                return Unauthorized();

            var expert = await _context.Experts
                .Include(e => e.User)
                .Include(e => e.ExpertSpecialties)
                    .ThenInclude(es => es.Occupation)
                .Include(e => e.ExpertSpecialties)
                    .ThenInclude(es => es.Services)
                        .ThenInclude(s => s.Pricing)
                .Include(e => e.WorkLocation)
                .FirstOrDefaultAsync(e => e.UserId == userId);

            if (expert == null)
                return NotFound("A szakember profil nem található.");

            return Ok(expert.Adapt<OutputExpertDto>());
        }

        // GET: api/experts/me/jobs
        [HttpGet("me/jobs")]
        [Authorize(Roles = "Expert")]
        public async Task<ActionResult<IEnumerable<OutputJobDto>>> GetMyJobs()
        {
            if (!TryGetLoggedInUserId(out int userId))
                return Unauthorized();

            var jobs = await _context.Jobs
                .Include(j => j.Service)
                    .ThenInclude(s => s.ExpertSpecialty)
                        .ThenInclude(es => es.Expert)
                .Include(j => j.TimeIntervals)
                .Include(j => j.Pricing)
                .Include(j => j.InitiatingUser)
                .Include(j => j.Location).ThenInclude(l => l.Settlement)
                .Where(j => j.Service.ExpertSpecialty.Expert.UserId == userId)
                .ProjectToType<OutputJobDto>()
                .ToArrayAsync();

            return Ok(jobs);
        }

        // PUT: api/experts/me
        [HttpPut("me")]
        [Authorize(Roles = "Expert")]
        public async Task<ActionResult<OutputExpertDto>> UpdateExpert(UpdateExpertDto dto)
        {
            if (!TryGetLoggedInUserId(out int userId))
                return Unauthorized();

            if (!await _context.Settlements.AnyAsync(s => s.Id == dto.WorkLocationId))
                return BadRequest("Érvénytelen település azonosító.");

            var expert = await _context.Experts
                .Include(e => e.User)
                .Include(e => e.WorkLocation)
                .FirstOrDefaultAsync(e => e.UserId == userId);

            if (expert == null)
                return NotFound();

            expert.Biography = dto.Biography;
            expert.CompanyPhoneNumber = dto.CompanyPhoneNumber;
            expert.CompanyEmail = dto.CompanyEmail;
            expert.WorkLocationId = dto.WorkLocationId;

            await _context.SaveChangesAsync();
            return Ok(expert.Adapt<OutputExpertDto>());
        }

        // POST: api/experts/me/occupations
        [HttpPost("me/occupations")]
        [Authorize(Roles = "Expert")]
        public async Task<ActionResult> CreateExpertSpecialty([FromQuery] int occupationId)
        {
            if (!TryGetLoggedInUserId(out int userId))
                return Unauthorized();

            if (!await _context.Occupations.AnyAsync(o => o.Id == occupationId))
                return NotFound("A megadott foglalkozás nem található.");

            if (await _context.ExpertSpecialties.AnyAsync(es => es.ExpertId == userId && es.OccupationId == occupationId))
                return Conflict("Ez a foglalkozás már hozzá van rendelve ehhez a profilhoz.");

            _context.ExpertSpecialties.Add(new ExpertSpecialty { ExpertId = userId, OccupationId = occupationId });
            await _context.SaveChangesAsync();

            return Created();
        }

        // DELETE: api/experts/me/occupations/{occupationId}
        [HttpDelete("me/occupations/{occupationId}")]
        [Authorize(Roles = "Expert")]
        public async Task<ActionResult> DeleteExpertSpecialty(int occupationId)
        {
            if (!TryGetLoggedInUserId(out int userId))
                return Unauthorized();

            var specialty = await _context.ExpertSpecialties
                .FirstOrDefaultAsync(es => es.ExpertId == userId && es.OccupationId == occupationId);

            if (specialty == null)
                return NotFound("Ez a foglalkozás nincs hozzárendelve ehhez a profilhoz.");

            _context.ExpertSpecialties.Remove(specialty);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/experts/me/occupations/{occupationId}/services
        [HttpPost("me/occupations/{occupationId}/services")]
        [Authorize(Roles = "Expert")]
        public async Task<ActionResult<OutputServiceDto>> CreateService([FromBody] InputServiceDto serviceDto, int occupationId)
        {
            var validation = await _serviceValidator.ValidateAsync(serviceDto); //real sad that this can't be done automatically before hitting the action for WebApi projects
            if (!validation.IsValid)
                return BadRequest(validation.Errors.Select(e => new { property = e.PropertyName, message = e.ErrorMessage }));

            if (!TryGetLoggedInUserId(out int userId))
                return Unauthorized();

            var specialty = await _context.ExpertSpecialties
                .FirstOrDefaultAsync(es => es.ExpertId == userId && es.OccupationId == occupationId);

            if (specialty == null)
                return NotFound("Ez a foglalkozás nincs hozzárendelve ehhez a profilhoz.");

            var service = new Service
            {
                Name = serviceDto.Name,
                Description = serviceDto.Description,
                ExpertSpecialtyId = specialty.Id,
                Pricing = serviceDto.Pricing.Adapt<Pricing>()
            };

            _context.Services.Add(service);
            await _context.SaveChangesAsync();

            await _context.Entry(service).Reference(s => s.Pricing).LoadAsync();
            await _context.Entry(service).Reference(s => s.ExpertSpecialty).LoadAsync();
            await _context.Entry(service.ExpertSpecialty).Reference(es => es.Expert).LoadAsync();
            await _context.Entry(service.ExpertSpecialty.Expert).Reference(e => e.User).LoadAsync();
            return Created(string.Empty, service.Adapt<OutputServiceDto>());
        }

        // PUT: api/experts/me/occupations/{occupationId}/services/{serviceId}
        [HttpPut("me/occupations/{occupationId}/services/{serviceId}")]
        [Authorize(Roles = "Expert")]
        public async Task<ActionResult<OutputServiceDto>> UpdateService([FromBody] InputServiceDto serviceDto, int occupationId, int serviceId)
        {
            var validation = await _serviceValidator.ValidateAsync(serviceDto);
            if (!validation.IsValid)
                return BadRequest(validation.Errors.Select(e => new { property = e.PropertyName, message = e.ErrorMessage }));

            if (!TryGetLoggedInUserId(out int userId))
                return Unauthorized();

            var specialty = await _context.ExpertSpecialties
                .FirstOrDefaultAsync(es => es.ExpertId == userId && es.OccupationId == occupationId);

            if (specialty == null)
                return NotFound("Ez a foglalkozás nincs hozzárendelve ehhez a profilhoz.");

            var service = await _context.Services
                .Include(s => s.Pricing)
                .Include(s => s.ExpertSpecialty).ThenInclude(es => es.Expert).ThenInclude(e => e.User)
                .FirstOrDefaultAsync(s => s.Id == serviceId && s.ExpertSpecialtyId == specialty.Id);

            if (service == null)
                return NotFound("A megadott szolgáltatás nem található.");

            service.Name = serviceDto.Name;
            service.Description = serviceDto.Description;
            service.Pricing.PricingType = serviceDto.Pricing.PricingType;
            service.Pricing.FixedPrice = serviceDto.Pricing.FixedPrice;
            service.Pricing.UnitPrice = serviceDto.Pricing.UnitPrice;
            service.Pricing.UnitName = serviceDto.Pricing.UnitName;

            await _context.SaveChangesAsync();

            var outputServiceDto = service.Adapt<OutputServiceDto>();

            return Ok(outputServiceDto);
        }

        // DELETE: api/experts/me/occupations/{occupationId}/services/{serviceId}
        [HttpDelete("me/occupations/{occupationId}/services/{serviceId}")]
        [Authorize(Roles = "Expert")]
        public async Task<ActionResult> DeleteService(int occupationId, int serviceId)
        {
            if (!TryGetLoggedInUserId(out int userId))
                return Unauthorized();

            var specialty = await _context.ExpertSpecialties
                .FirstOrDefaultAsync(es => es.ExpertId == userId && es.OccupationId == occupationId);

            if (specialty == null)
                return NotFound("Ez a foglalkozás nincs hozzárendelve ehhez a profilhoz.");

            var service = await _context.Services
                .FirstOrDefaultAsync(s => s.Id == serviceId && s.ExpertSpecialtyId == specialty.Id);

            if (service == null)
                return NotFound("A megadott szolgáltatás nem található.");

            _context.Services.Remove(service);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/experts
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<OutputExpertDto>>> GetExperts([FromQuery] ExpertFilterParams filters)
        {
            var query = _context.Experts
                .Include(e => e.User)
                .Include(e => e.WorkLocation)
                .Include(e => e.ExpertSpecialties)
                    .ThenInclude(es => es.Occupation)
                .Include(e => e.ExpertSpecialties)
                    .ThenInclude(es => es.Services)
                        .ThenInclude(s => s.Pricing)
                .AsQueryable();

            //clean af
            if (filters.OccupationId.HasValue)
                query = query.Where(e => e.ExpertSpecialties.Any(es => es.OccupationId == filters.OccupationId.Value));

            if (filters.MinFixedPrice.HasValue)
                query = query.Where(e => e.ExpertSpecialties.Any(es =>
                    es.Services.Any(s => s.Pricing.FixedPrice >= filters.MinFixedPrice.Value)));

            if (filters.MaxFixedPrice.HasValue)
                query = query.Where(e => e.ExpertSpecialties.Any(es =>
                    es.Services.Any(s => s.Pricing.FixedPrice <= filters.MaxFixedPrice.Value)));

            if (!filters.AllowUnitBased)
                query = query.Where(e => e.ExpertSpecialties.Any(es =>
                    es.Services.Any(s => s.Pricing.PricingType == PricingType.Fixed)));

            if (!string.IsNullOrEmpty(filters.UnitName))
                query = query.Where(e => e.ExpertSpecialties.Any(es =>
                    es.Services.Any(s => s.Pricing.UnitName == filters.UnitName)));

            if (!string.IsNullOrEmpty(filters.CountyName))
                query = query.Where(e => e.WorkLocation != null && e.WorkLocation.CountyName == filters.CountyName);

            if (!string.IsNullOrEmpty(filters.SettlementName))
                query = query.Where(e => e.WorkLocation != null && e.WorkLocation.Name == filters.SettlementName);

            return Ok(await query.ProjectToType<OutputExpertDto>().ToArrayAsync());
        }

        // GET: api/experts/truncated
        [HttpGet("truncated")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<TruncatedExpertDto>>> GetTruncated()
        {
            var experts = await _context.Experts
                .Include(e => e.User)
                .ToListAsync();

            return Ok(experts.Select(e => new TruncatedExpertDto
            {
                Id = e.UserId,
                Name = e.User.LastName + " " + e.User.FirstName,
            }));
        }

        // GET: api/experts/{id}
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<OutputExpertDto>> GetExpert(int id)
        {
            var expert = await _context.Experts
                .Include(e => e.User)
                .Include(e => e.ExpertSpecialties)
                    .ThenInclude(eo => eo.Occupation)
                .ProjectToType<OutputExpertDto>()
                .FirstOrDefaultAsync(e => e.UserId == id);

            if (expert == null)
                return NotFound();

            return expert;
        }

        // GET: api/experts/{id}/jobs
        [HttpGet("{id}/jobs")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<OutputJobDto>>> GetExpertCompletedJobs(int id)
        {
            var jobs = await _context.Jobs
                .Include(j => j.Service)
                    .ThenInclude(s => s.ExpertSpecialty)
                        .ThenInclude(es => es.Expert)
                .Include(j => j.TimeIntervals)
                .Include(j => j.Pricing)
                .Include(j => j.Location).ThenInclude(l => l.Settlement)
                .Where(j => j.Service.ExpertSpecialty.Expert.UserId == id
                            && j.Status == JobStatus.Completed)
                .ProjectToType<OutputJobDto>()
                .ToArrayAsync();

            return Ok(jobs);
        }

        private bool TryGetLoggedInUserId(out int userId)
        {
            return int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out userId);
        }
    }
}
