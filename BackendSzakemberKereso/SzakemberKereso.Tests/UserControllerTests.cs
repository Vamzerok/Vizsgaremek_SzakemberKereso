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
using SzakemberKereso.DTOs.User;
using SzakemberKereso.Models;

namespace SzakemberKereso.Tests
{
    public class UserControllerTests
    {
        private Mock<UserManager<User>> CreateMockUserManager()
        {
            var store = new Mock<IUserStore<User>>();
            //fuckin beautiful init boyy
            return new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
        }

        private Context CreateContext()
        {
            return new Context(new DbContextOptionsBuilder<Context>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options);
        }

        private UsersController CreateController(Context context, int userId, UserManager<User> userManager = null)
        {
            var mockUserManager = userManager ?? CreateMockUserManager().Object;

            var controller = new UsersController(context, mockUserManager);

            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString())
        };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            };

            return controller;
        }

        [Fact]
        public async Task GetMe_ReturnsOwnData_IfAuthorized()
        {
            var context = new Context(new DbContextOptionsBuilder<Context>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options);

            var testUser = new User { Id = 1, UserName = "énvagyok", FirstName = "Józsi", LastName = "Teszt" };
            context.Users.Add(testUser);
            context.SaveChanges();

            var mockUserManager = CreateMockUserManager();
            mockUserManager.Setup(um => um.FindByIdAsync("1")).ReturnsAsync(testUser);
            mockUserManager.Setup(um => um.GetRolesAsync(testUser)).ReturnsAsync(new List<string> { "User" });

            var controller = new UsersController(context, mockUserManager.Object);

            var claims = new List<Claim> { new Claim(ClaimTypes.NameIdentifier, "1") };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identity);
            controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = claimsPrincipal } };

            var result = await controller.GetMe();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedUser = Assert.IsType<OutputUserDto>(okResult.Value);
            Assert.Equal("Józsi", returnedUser.FirstName);
        }

        [Fact]
        public async Task GetMe_NoAuth_ReturnsUnauthorized()
        {
            var context = CreateContext();
            var mockUM = CreateMockUserManager();

            var controller = new UsersController(context, mockUM.Object);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext() 
            };

            var result = await controller.GetMe();

            Assert.IsType<UnauthorizedResult>(result.Result);
        }

        [Fact]
        public async Task UpdateMe_ValidData_ReturnsOkAndUpdatesUser()
        {
            var context = CreateContext();
            var user = new User { Id = 1, FirstName = "Régi", LastName = "Név", PhoneNumber = "123" };
            context.Users.Add(user);
            context.SaveChanges();

            var mockUM = CreateMockUserManager();
            mockUM.Setup(u => u.FindByIdAsync("1")).ReturnsAsync(user);
            mockUM.Setup(u => u.SetPhoneNumberAsync(user, "456")).ReturnsAsync(IdentityResult.Success);
            mockUM.Setup(u => u.GetRolesAsync(user)).ReturnsAsync(new List<string>());

            var controller = CreateController(context, 1, mockUM.Object);
            var dto = new UpdateUserDto { FirstName = "Új", LastName = "Név", PhoneNumber = "456" };

            var result = await controller.UpdateMe(dto);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedUser = Assert.IsType<OutputUserDto>(okResult.Value);
            Assert.Equal("Új", user.FirstName);
            Assert.Equal("Új", returnedUser.FirstName);
        }

        [Fact]
        public async Task UpdateMe_IdentityError_ReturnsBadRequest()
        {
            var context = CreateContext();
            var user = new User { Id = 1, FirstName = "Test", LastName = "User", PhoneNumber = "123" };
            context.Users.Add(user);
            context.SaveChanges();

            var mockUM = CreateMockUserManager();
            mockUM.Setup(u => u.FindByIdAsync("1")).ReturnsAsync(user);

            var identityError = IdentityResult.Failed(new IdentityError { Code = "PhoneError", Description = "Hibás szám" });
            mockUM.Setup(u => u.SetPhoneNumberAsync(user, "invalid")).ReturnsAsync(identityError);

            var controller = CreateController(context, 1, mockUM.Object);
            var dto = new UpdateUserDto { FirstName = "Teszt", LastName = "User", PhoneNumber = "invalid" };

            var result = await controller.UpdateMe(dto);

            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        

        [Fact]
        public async Task UpdateMe_UserNotFound_ReturnsNotFound()
        {
            var context = CreateContext();
            var mockUM = CreateMockUserManager();
            mockUM.Setup(u => u.FindByIdAsync("1")).ReturnsAsync((User)null); 

            var controller = CreateController(context, 1, mockUM.Object);
            var dto = new UpdateUserDto { FirstName = "Bármi", LastName = "Bármi" };

            var result = await controller.UpdateMe(dto);

            Assert.IsType<NotFoundResult>(result.Result);
        }
    }
}
