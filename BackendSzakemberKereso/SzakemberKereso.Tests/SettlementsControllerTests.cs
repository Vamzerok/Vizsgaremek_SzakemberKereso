using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SzakemberKereso.Controllers;
using SzakemberKereso.Models;

namespace SzakemberKereso.Tests
{
    public class SettlementsControllerTests
    {
        private Context CreateContext()
        {
            var options = new DbContextOptionsBuilder<Context>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new Context(options);
        }

        private SettlementsController CreateController(Context context)
        {
            return new SettlementsController(context);
        }

        [Fact]
        public async Task GetCounties_ReturnsDistinctOrderedCounties()
        {
            var context = CreateContext();
            context.Experts.AddRange(new List<Expert> {
                new Expert { UserId = 1, CompanyEmail = "test.expert@gmail.com", CompanyPhoneNumber = "06301234567", WorkLocation = new Settlement { CountyName = "Pest", Name = "Érd" } },
                new Expert { UserId = 2, CompanyEmail = "test.expert@gmail.com", CompanyPhoneNumber = "06301234567", WorkLocation = new Settlement { CountyName = "Baranya", Name = "Pécs" } },
                new Expert { UserId = 3, CompanyEmail = "test.expert@gmail.com", CompanyPhoneNumber = "06301234567", WorkLocation = new Settlement { CountyName = "Pest", Name = "Budaörs" } }
            });
            context.SaveChanges();
            var controller = CreateController(context);

            var result = await controller.GetCounties();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var counties = Assert.IsAssignableFrom<IEnumerable<string>>(okResult.Value).ToList();

            Assert.Equal(2, counties.Count);
            Assert.Equal("Baranya", counties[0]);
            Assert.Equal("Pest", counties[1]);
        }

        [Fact]
        public async Task GetCounties_NoExperts_ReturnsEmptyList()
        {
            var context = CreateContext();
            var controller = CreateController(context);

            var result = await controller.GetCounties();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var counties = Assert.IsAssignableFrom<IEnumerable<string>>(okResult.Value);
            Assert.Empty(counties);
        }

        [Fact]
        public async Task GetCounties_SkipsNullLocations()
        {
            var context = CreateContext();
            context.Experts.Add(new Expert { UserId = 1, CompanyEmail = "test.expert@gmail.com", CompanyPhoneNumber = "06301234567", WorkLocation = null });
            context.SaveChanges();
            var controller = CreateController(context);

            var result = await controller.GetCounties();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var counties = Assert.IsAssignableFrom<IEnumerable<string>>(okResult.Value);
            Assert.Empty(counties);
        }

        [Fact]
        public async Task GetByCounty_ReturnsCorrectSettlements()
        {
            var context = CreateContext();
            var pest = "Pest";
            context.Experts.AddRange(new List<Expert> {
        new Expert { UserId = 1, CompanyEmail = "test.expert@gmail.com", CompanyPhoneNumber = "06301234567", WorkLocation = new Settlement { CountyName = pest, Name = "Vác" } },
        new Expert { UserId = 2, CompanyEmail = "test.expert@gmail.com", CompanyPhoneNumber = "06301234567", WorkLocation = new Settlement { CountyName = pest, Name = "Göd" } },
        new Expert { UserId = 3, CompanyEmail = "test.expert@gmail.com", CompanyPhoneNumber = "06301234567", WorkLocation = new Settlement { CountyName = "Hajdú", Name = "Debrecen" } }
    });
            context.SaveChanges();
            var controller = CreateController(context);

            var result = await controller.GetByCounty(pest);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var settlements = Assert.IsAssignableFrom<IEnumerable<string>>(okResult.Value).ToList();

            Assert.Equal(2, settlements.Count);
            Assert.Contains("Vác", settlements);
            Assert.Contains("Göd", settlements);
            Assert.DoesNotContain("Debrecen", settlements);
        }

        [Fact]
        public async Task GetByCounty_DistinctNamesOnly()
        {
            var context = CreateContext();
            var city = "Szentendre";
            context.Experts.AddRange(new List<Expert> {
        new Expert { UserId = 1, CompanyEmail = "test.expert@gmail.com", CompanyPhoneNumber = "06301234567", WorkLocation = new Settlement { CountyName = "Pest", Name = city } },
        new Expert { UserId = 2, CompanyEmail = "test.expert@gmail.com", CompanyPhoneNumber = "06301234567", WorkLocation = new Settlement { CountyName = "Pest", Name = city } }
    });
            context.SaveChanges();
            var controller = CreateController(context);

            var result = await controller.GetByCounty("Pest");

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var settlements = Assert.IsAssignableFrom<IEnumerable<string>>(okResult.Value);
            Assert.Single(settlements);
        }

        [Fact]
        public async Task GetByCounty_InvalidCounty_ReturnsEmpty()
        {
            var context = CreateContext();
            context.Experts.Add(new Expert { UserId = 1, CompanyEmail = "test.expert@gmail.com", CompanyPhoneNumber = "06301234567", WorkLocation = new Settlement { CountyName = "Zala", Name = "Lenti" } });
            context.SaveChanges();
            var controller = CreateController(context);

            var result = await controller.GetByCounty("NemLetezoMegye");

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var settlements = Assert.IsAssignableFrom<IEnumerable<string>>(okResult.Value);
            Assert.Empty(settlements);
        }


    }
}
