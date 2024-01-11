using Kurskcartuning.Server_v2.Core.DbContext;
using Kurskcartuning.Server_v2.Core.Dtos.Auth;
using Kurskcartuning.Server_v2.Core.Entities.AppDB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Kurskcartuning.Server_v2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public VehicleController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("new-client")]
        public async Task<ActionResult<LoginServiceResponceDto>> NewClient(string configuration, long enginePower, string registrationNumber, long year, long clientId)
        {
            Vehicle vehicle = new Vehicle {Configuration = configuration, EnginePower = enginePower, RegistrationNumber = registrationNumber, Year = year };

            var client =  await _context.Clients.Where(x => x.Id.Equals(clientId)).FirstOrDefaultAsync();

          var lol =  client.Vehicles = new List<Vehicle>();

            lol.Add(vehicle);

            _context.SaveChanges();

            Thread.Sleep(2000);

            var client1 = await _context.Clients.Where(x => x.Id.Equals(clientId)).FirstOrDefaultAsync();

            return Ok(client1);

        }
    }
}
