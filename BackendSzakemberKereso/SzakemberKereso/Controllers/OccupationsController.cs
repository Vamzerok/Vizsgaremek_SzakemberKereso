using System.Collections.Generic;
using System.Threading.Tasks;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SzakemberKereso.DTOs.Occupation;
using SzakemberKereso.Models;

namespace SzakemberKereso.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OccupationsController : ControllerBase
    {
        private readonly Context _context;

        public OccupationsController(Context context)
        {
            _context = context;
        }

        // GET: api/Occupations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OutputOccupationDto>>> GetOccupations()
        {
            return Ok(await _context.Occupations.ProjectToType<OutputOccupationDto>().ToListAsync());
        }
    }
}
