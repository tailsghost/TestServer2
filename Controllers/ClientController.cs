using Kurskcartuning.Server_v2.Config;
using Kurskcartuning.Server_v2.Core.DbContext;
using Kurskcartuning.Server_v2.Core.Dtos.Auth;
using Kurskcartuning.Server_v2.Core.Entities.AppDB;
using Kurskcartuning.Server_v2.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Kurskcartuning.Server_v2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {

        private readonly ApplicationDbContext _context;

        public ClientController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("new-client")]
        public async Task<ActionResult<LoginServiceResponceDto>> NewClient(string name, string phone)
        {
            Client client = new Client { FirstName = name, Phone = phone, UserId = UserId};

            _context.Clients.Add(client);

            _context.SaveChanges();

            return Ok(client);
            
        }

        private string UserId => HttpContext.User.Claims.First(x => x.Type == CustomClaimTypes.Id).Value;
    }
}
