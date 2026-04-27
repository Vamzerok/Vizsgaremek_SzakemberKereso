using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SzakemberKereso.Controllers;
using SzakemberKereso.DTOs;
using SzakemberKereso.DTOs.Service;
using SzakemberKereso.Models;

namespace SzakemberKereso.Tests
{
    public class ServicesControllerTests
    {
        private Context CreateContext()
        {
            var options = new DbContextOptionsBuilder<Context>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new Context(options);
        }

        private ServicesController CreateController(Context context)
        {
            return new ServicesController(context);
        }

        [Fact]
        public async Task GetPricingOptions_ReturnsCorrectAggregates()
        {
            var context = CreateContext();
            var specialty = new ExpertSpecialty { Id = 1, OccupationId = 10 };

            context.Services.AddRange(new List<Service> {
                new Service {
                    Id = 1, ExpertSpecialty = specialty,
                    Pricing = new Pricing { FixedPrice = 1000, PricingType = PricingType.FixedAndUnitBased, UnitName = "m2" }
                },
                new Service {
                    Id = 2, ExpertSpecialty = specialty,
                    Pricing = new Pricing { FixedPrice = 5000, PricingType = PricingType.FixedAndUnitBased, UnitName = "alkalom" }
                },
                new Service {
                    Id = 3, ExpertSpecialty = specialty,
                    Pricing = new Pricing { FixedPrice = 3000, PricingType = PricingType.FixedAndUnitBased, UnitName = "m2" }
                }
            });
            await context.SaveChangesAsync();
            var controller = CreateController(context);

            var result = await controller.GetPricingOptions(10);
            var okResult = Assert.IsType<OkObjectResult>(result);

            var data = okResult.Value;
            var minPrice = data.GetType().GetProperty("minFixedPrice")?.GetValue(data, null);
            var maxPrice = data.GetType().GetProperty("maxFixedPrice")?.GetValue(data, null);
            var unitNames = data.GetType().GetProperty("unitNames")?.GetValue(data, null) as IEnumerable<string>;

            Assert.Equal(1000m, Convert.ToDecimal(minPrice));
            Assert.Equal(5000m, Convert.ToDecimal(maxPrice));

            Assert.NotNull(unitNames);
            Assert.Equal(2, unitNames.Count());
        }

        [Fact]
        public async Task GetPricingOptions_NoServices_ReturnsDefaults()
        {
            var context = CreateContext();
            var controller = CreateController(context);

            var result = await controller.GetPricingOptions(null);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var data = okResult.Value;
            var minPrice = data.GetType().GetProperty("minFixedPrice")?.GetValue(data, null);
            var maxPrice = data.GetType().GetProperty("maxFixedPrice")?.GetValue(data, null);

            Assert.Equal(0, Convert.ToInt32(minPrice));
            Assert.Equal(100000, Convert.ToDecimal(maxPrice));
        }

        [Fact]
        public async Task GetTruncated_SpecificExpert_ReturnsOnlyTheirServices()
        {
            var context = CreateContext();
            var expert1 = new Expert { UserId = 1, CompanyEmail = "test.expert@gmail.com", CompanyPhoneNumber = "06301234567" };
            var expert2 = new Expert { UserId = 2, CompanyEmail = "test.expert@gmail.com", CompanyPhoneNumber = "06301234567" };
            var spec1 = new ExpertSpecialty { Expert = expert1 };
            var spec2 = new ExpertSpecialty { Expert = expert2 };

            context.Services.Add(new Service { Id = 1, Name = "Szakember 1 szervize", ExpertSpecialty = spec1 });
            context.Services.Add(new Service { Id = 2, Name = "Szakember 2 szervize", ExpertSpecialty = spec2 });
            context.SaveChanges();
            var controller = CreateController(context);

            var result = await controller.GetTruncated(1);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var services = Assert.IsAssignableFrom<IEnumerable<TruncatedServiceDto>>(okResult.Value);
            Assert.Single(services);
            Assert.Equal("Szakember 1 szervize", services.First().Name);
        }

        [Fact]
        public async Task GetTruncated_ExpertWithNoServices_ReturnsEmptyList()
        {
            var context = CreateContext();
            var expert = new Expert { UserId = 5, CompanyEmail = "expert@test.com", CompanyPhoneNumber = "06301234567" };
            context.Experts.Add(expert);
            context.SaveChanges();
            var controller = CreateController(context);

            var result = await controller.GetTruncated(5);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var list = Assert.IsAssignableFrom<IEnumerable<TruncatedServiceDto>>(okResult.Value);
            Assert.Empty(list);
        }
    }
}
