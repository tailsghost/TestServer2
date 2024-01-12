using Kurskcartuning.Server_v2.Config;
using Kurskcartuning.Server_v2.Core.Constants;
using Kurskcartuning.Server_v2.Core.Dtos.App.Client;
using Kurskcartuning.Server_v2.Core.Entities.AppDB;
using Kurskcartuning.Server_v2.Core.Interfaces.App;
using Kurskcartuning.Server_v2.Core.Services.App;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kurskcartuning.Server_v2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {

        private readonly IClientService _clientService;

        public ClientController(IClientService service)
        {
            _clientService = service;
        }

        [HttpPost("new-client")]
        [Authorize(Roles = StaticUserRoles.OwnerAdminUserPremium)]
        public async Task<ActionResult<ClientServiceResponceDto>> PostClient([FromForm] ClientPostDto dto)
        {
            var newClient = await _clientService.PostClientAsync(User, dto, UserId);

            if (newClient.IsSucced is false)
                return StatusCode(newClient.StatusCode, newClient.Message);

            return newClient;
        }


        [HttpGet("get-client/{id}")]
        [Authorize(Roles = StaticUserRoles.OwnerAdminUserPremium)]
        public async Task<ActionResult<ClientServiceResponceDto>> GetClientAsync(long id)
        {
            var client = await _clientService.GetClientIdAsync(id);

            if (client.IsSucced is false)
                return StatusCode(client.StatusCode, client.Message);

            return client;
        }

        [HttpGet("get-clients")]
        [Authorize(Roles = StaticUserRoles.OwnerAdminUserPremium)]
        public async Task<ActionResult<ClientServiceResponceDto>> GetClients()
        {
            var clients = await _clientService.GetClientsAsync(UserId);

            if (clients.IsSucced is false)
                return StatusCode(clients.StatusCode, clients.Message);

            return Ok(clients);
        }

        [HttpPut("put-client")]
        [Authorize(Roles = StaticUserRoles.OwnerAdminUserPremium)]
        public async Task<ActionResult<ClientServiceResponceDto>> PutClient(ClientPutDto dto)
        {
            var client = await _clientService.PutClientAsync(User, dto);

            if (client.IsSucced is false)
                return StatusCode(client.StatusCode, client.Message);


            return Ok(client);
        }

        [HttpDelete("delete-client")]
        [Authorize(Roles = StaticUserRoles.OwnerAdminUserPremium)]
        public async Task<ActionResult<ClientServiceResponceDto>> DeleteClient(ClientDeleteDto dto)
        {
            var client = await _clientService.DeleteClientAsync(User, dto);

            if (client.IsSucced is false)
                return StatusCode(client.StatusCode, client.Message);

            return Ok(client);
        }

        private string UserId => HttpContext.User.Claims.First(x => x.Type == CustomClaimTypes.Id).Value;
    }
}
