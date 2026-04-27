using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SzakemberKereso.Controllers;
using SzakemberKereso.DTOs.Occupation;
using SzakemberKereso.Models;

namespace SzakemberKereso.Tests
{
    public class OccupationsControllerTests
    {
        private Context CreateContext()
        {
            var options = new DbContextOptionsBuilder<Context>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new Context(options);
        }

        private OccupationsController CreateController(Context context)
        {
            return new OccupationsController(context);
        }

        [Fact]
        public async Task GetOccupations_ReturnsAllOccupations()
        {
            var context = CreateContext();
            context.Occupations.AddRange(new List<Occupation> {
                new Occupation { Id = 1, Name = "Vízszerelő", Description = "Vízvezeték szerelés és javítás" },
                new Occupation { Id = 2, Name = "Gázszerelő", Description = "Gázkészülék karbantartás" }
            });
            context.SaveChanges();
            var controller = CreateController(context);

            var result = await controller.GetOccupations();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var occupations = Assert.IsAssignableFrom<IEnumerable<OutputOccupationDto>>(okResult.Value);
            Assert.Equal(2, occupations.Count());
        }
    }
}
