using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SzakemberKereso.DTOs.Settlement;
using SzakemberKereso.Models;

namespace SzakemberKereso.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettlementsController : ControllerBase
    {
        private readonly Context _context;

        public SettlementsController(Context context)
        {
            _context = context;
        }

        // GET: api/Settlements/counties
        [HttpGet("counties")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<string>>> GetCounties()
        {
            var counties = await _context.Experts
                .Where(e => e.WorkLocation != null)
                .Select(e => e.WorkLocation!.CountyName)
                .Distinct()
                .OrderBy(c => c)
                .ToListAsync();

            return Ok(counties);
        }

        // GET: api/Settlements/byCounty?countyName=X
        [HttpGet("byCounty")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<string>>> GetByCounty([FromQuery] string countyName)
        {
            var settlements = await _context.Experts
                .Where(e => e.WorkLocation != null && e.WorkLocation.CountyName == countyName)
                .Select(e => e.WorkLocation!.Name)
                .Distinct()
                .OrderBy(s => s)
                .ToListAsync();

            return Ok(settlements);
        }

        // GET: api/Settlements/allCounties
        [HttpGet("allCounties")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<string>>> GetAllCounties()
        {
            var counties = await _context.Settlements
                .Select(s => s.CountyName)
                .Distinct()
                .OrderBy(c => c)
                .ToListAsync();

            return Ok(counties);
        }

        // GET: api/Settlements/allByCounty?countyName=X
        [HttpGet("allByCounty")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<OutputSettlementDto>>> GetAllByCounty([FromQuery] string countyName)
        {
            var settlements = await _context.Settlements
                .Where(s => s.CountyName == countyName)
                .OrderBy(s => s.Name)
                .ProjectToType<OutputSettlementDto>()
                .ToListAsync();

            return Ok(settlements);
        }
    }
}
