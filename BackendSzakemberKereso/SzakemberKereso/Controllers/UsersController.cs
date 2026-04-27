using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SzakemberKereso.DTOs.Job;
using SzakemberKereso.DTOs.User;
using SzakemberKereso.Models;

namespace SzakemberKereso.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly Context _context;
        private readonly UserManager<User> _userManager;

        public UsersController(Context context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<ActionResult<OutputUserDto>> GetMe()
        {
            if (!TryGetLoggedInUserId(out int userId))
                return Unauthorized();

            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                return NotFound();

            var userDto = user.Adapt<OutputUserDto>();
            userDto.Roles = (await _userManager.GetRolesAsync(user)).ToArray();

            return Ok(userDto);
        }

        [HttpPut("me")]
        [Authorize]
        public async Task<ActionResult<OutputUserDto>> UpdateMe(UpdateUserDto dto)
        {
            if (!TryGetLoggedInUserId(out int userId))
                return Unauthorized();

            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null) return NotFound();

            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;

            IdentityResult result = null!;
            if (dto.PhoneNumber != user.PhoneNumber)
            {
                result = await _userManager.SetPhoneNumberAsync(user, dto.PhoneNumber);
            }
            else
            {
                result = await _userManager.UpdateAsync(user);
            }

            if (!result.Succeeded)
                return BadRequest(result.Errors.Select(e => new { code = e.Code, description = e.Description }));

            var userDto = user.Adapt<OutputUserDto>();
            userDto.Roles = (await _userManager.GetRolesAsync(user)).ToArray();
            return Ok(userDto);
        }

        [HttpGet("me/jobs")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<OutputJobDto>>> GetMyJobs()
        {
            if (!TryGetLoggedInUserId(out int userId))
                return Unauthorized();

            var jobs = await _context.Jobs
                .Include(j => j.TimeIntervals)
                .Include(j => j.Pricing)
                .ProjectToType<OutputJobDto>()
                .Where(j => j.InitiatingUserId == userId)
                .ToListAsync();

            return Ok(jobs);
        }

        private bool TryGetLoggedInUserId(out int userId)
        {
            return int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out userId);
        }
    }
}
