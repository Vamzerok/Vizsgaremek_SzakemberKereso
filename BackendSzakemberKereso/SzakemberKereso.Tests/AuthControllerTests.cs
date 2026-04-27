using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Security.Claims;
using SzakemberKereso;
using SzakemberKereso.Controllers;
using SzakemberKereso.DTOs.Auth;
using SzakemberKereso.DTOs.Job;
using SzakemberKereso.DTOs.Pricing;
using SzakemberKereso.DTOs.ResidentialAddress;
using SzakemberKereso.DTOs.Settlement;
using SzakemberKereso.DTOs.TimeInterval;
using SzakemberKereso.Models;
using SzakemberKereso.Services;

namespace SzakemberKereso.Tests
{
    public class AuthControllerTests
    {
        private readonly Mock<UserManager<User>> _mockUserManager;
        private readonly Mock<SignInManager<User>> _mockSignInManager;
        private readonly AuthController _controller;

        public AuthControllerTests()
        {
            var store = new Mock<IUserStore<User>>();
            _mockUserManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);

            var contextAccessor = new Mock<IHttpContextAccessor>();
            var claimsFactory = new Mock<IUserClaimsPrincipalFactory<User>>();
            _mockSignInManager = new Mock<SignInManager<User>>(
                _mockUserManager.Object, contextAccessor.Object, claimsFactory.Object, null, null, null, null);
            
            var context = new Context(new DbContextOptionsBuilder<Context>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options);

            _controller = new AuthController(_mockSignInManager.Object, _mockUserManager.Object, context);
        }

        [Fact]
        public async Task Register_Success_ReturnsOk()
        {
            var dto = new RegisterDto { Email = "test@test.com", Password = "Password123!", FirstName = "Teszt", LastName = "Elek" };
            _mockUserManager.Setup(u => u.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);
            _mockUserManager.Setup(u => u.AddToRoleAsync(It.IsAny<User>(), "Generic"))
                .ReturnsAsync(IdentityResult.Success);

            var result = await _controller.Register(dto);

            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task Register_WeakPassword_ReturnsBadRequest()
        {
            var dto = new RegisterDto { Email = "test@test.com", Password = "123" };
            var identityError = IdentityResult.Failed(new IdentityError { Code = "PasswordTooShort", Description = "Túl rövid" });

            _mockUserManager.Setup(u => u.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(identityError);

            var result = await _controller.Register(dto);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("A jelszó nem felel meg a követelményeknek.", badRequest.Value);
        }

        [Fact]
        public async Task Register_DuplicateEmail_ReturnsBadRequest()
        {
            var dto = new RegisterDto { Email = "marfoglalt@test.com" };
            var identityError = IdentityResult.Failed(new IdentityError { Code = "DuplicateUserName" });

            _mockUserManager.Setup(u => u.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(identityError);

            var result = await _controller.Register(dto);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Ez az email cím már használatban van.", badRequest.Value);
        }

        [Fact]
        public async Task Login_UserNotFound_ReturnsNotFound()
        {
            var dto = new LoginDto { Email = "nincsilyem@test.com", Password = "any" };
            _mockUserManager.Setup(u => u.FindByEmailAsync(dto.Email)).ReturnsAsync((User)null);

            var result = await _controller.Login(dto);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Login_WrongPassword_ReturnsUnauthorized()
        {
            var user = new User { Email = "test@test.com" };
            var dto = new LoginDto { Email = "test@test.com", Password = "wrongpassword" };

            _mockUserManager.Setup(u => u.FindByEmailAsync(dto.Email)).ReturnsAsync(user);
            _mockSignInManager.Setup(s => s.PasswordSignInAsync(user, dto.Password, false, false))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Failed);

            var result = await _controller.Login(dto);

            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task Login_Success_ReturnsOk()
        {
            var user = new User { Email = "test@test.com" };
            var dto = new LoginDto { Email = "test@test.com", Password = "CorrectPassword123" };

            _mockUserManager.Setup(u => u.FindByEmailAsync(dto.Email)).ReturnsAsync(user);
            _mockSignInManager.Setup(s => s.PasswordSignInAsync(user, dto.Password, false, false))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);

            var result = await _controller.Login(dto);

            Assert.IsType<OkResult>(result);
        }
    }
}
