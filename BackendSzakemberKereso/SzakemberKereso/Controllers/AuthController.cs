using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SzakemberKereso.DTOs.Auth;
using SzakemberKereso.Models;

namespace SzakemberKereso.Controllers
{
    [Route("api")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly Context _context;

        public AuthController(SignInManager<User> signInManager, UserManager<User> userManager, Context context)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _context = context;
        }
        
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            var user = new User { 
                UserName = registerDto.Email, 
                Email = registerDto.Email, 
                FirstName = registerDto.FirstName, 
                LastName = registerDto.LastName
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if(result.Errors.Any(e => e.Code == "PasswordTooShort" || e.Code == "PasswordRequiresNonAlphanumeric" || e.Code == "PasswordRequiresDigit" || e.Code == "PasswordRequiresUpper" || e.Code == "PasswordRequiresLower"))
            {
                return BadRequest("A jelszó nem felel meg a követelményeknek.");
            }

            if(result.Errors.Any(e => e.Code == "DuplicateUserName"))
            {
                return BadRequest("Ez az email cím már használatban van.");
            }

            if (!result.Succeeded)
            {
                return BadRequest("Hiba történt.");
            }

            try
            {
                await _userManager.AddToRoleAsync(user, "Generic");
            }
            catch {
                await _userManager.DeleteAsync(user);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            await _signInManager.SignInAsync(user, isPersistent: false);
            return Ok();
        }

        [HttpPost("register/expert")]
        public async Task<IActionResult> RegisterExpert([FromBody] RegisterExpertDto dto)
        {
            if (!await _context.Settlements.AnyAsync(s => s.Id == dto.WorkLocationId))
                return BadRequest("Érvénytelen település azonosító.");

            var user = new User {
                UserName = dto.Email,
                Email = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (result.Errors.Any(e => e.Code == "PasswordTooShort" || e.Code == "PasswordRequiresNonAlphanumeric" || e.Code == "PasswordRequiresDigit" || e.Code == "PasswordRequiresUpper" || e.Code == "PasswordRequiresLower"))
                return BadRequest("A jelszó nem felel meg a követelményeknek.");

            if (result.Errors.Any(e => e.Code == "DuplicateUserName"))
                return BadRequest("Ez az email cím már használatban van.");

            if (!result.Succeeded)
                return BadRequest("Hiba történt.");

            try
            {
                await _userManager.AddToRoleAsync(user, "Expert");
            }
            catch
            {
                await _userManager.DeleteAsync(user);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            try
            {
                var expert = new Expert
                {
                    UserId = user.Id,
                    Biography = dto.Biography,
                    CompanyEmail = dto.CompanyEmail,
                    CompanyPhoneNumber = dto.CompanyPhoneNumber,
                    WorkLocationId = dto.WorkLocationId,
                };
                _context.Experts.Add(expert);
                await _context.SaveChangesAsync();
            }
            catch
            {
                await _userManager.DeleteAsync(user);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            await _signInManager.SignInAsync(user, isPersistent: false);
            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user == null)
            {
                return NotFound();
            }

            var result = await _signInManager.PasswordSignInAsync(user, loginDto.Password, false, false);

            if (!result.Succeeded)
            {
                return Unauthorized();
            }
            return Ok();
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }
    }
}
