using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SzakemberKereso.DTOs;
using SzakemberKereso.DTOs.Service;
using SzakemberKereso.Models;

namespace SzakemberKereso.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicesController : ControllerBase
    {
        private readonly Context _context;

        public ServicesController(Context context)
        {
            _context = context;
        }

        // GET: api/Services/pricingOptions
        [HttpGet("pricingOptions")]
        [AllowAnonymous]
        public async Task<ActionResult> GetPricingOptions([FromQuery] int? occupationId)
        {
            var query = _context.Services
                .Include(s => s.Pricing)
                .Include(s => s.ExpertSpecialty)
                .AsQueryable();

            if (occupationId.HasValue)
                query = query.Where(s => s.ExpertSpecialty.OccupationId == occupationId.Value);

            var prices = await query.Select(s => s.Pricing.FixedPrice).ToListAsync();
            var unitNames = await query
                .Where(s => s.Pricing.PricingType == PricingType.FixedAndUnitBased && s.Pricing.UnitName != null)
                .Select(s => s.Pricing.UnitName!)
                .Distinct()
                .OrderBy(u => u)
                .ToListAsync();

            return Ok(new
            {
                minFixedPrice = prices.Count > 0 ? prices.Min() : 0,
                maxFixedPrice = prices.Count > 0 ? prices.Max() : 100000,
                unitNames
            });
        }

        // GET: api/Services/truncated?expertId=X
        [HttpGet("truncated")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<TruncatedServiceDto>>> GetTruncated([FromQuery] int expertId)
        {
            var services = await _context.Services
                .Include(s => s.ExpertSpecialty)
                .Where(s => s.ExpertSpecialty.ExpertId == expertId)
                .ToListAsync();

            return Ok(services.Select(s => new TruncatedServiceDto { Id = s.Id, Name = s.Name }));
        }

    }
}
