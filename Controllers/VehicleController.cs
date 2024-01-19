using Kurskcartuning.Server_v2.Config;
using Kurskcartuning.Server_v2.Core.Constants;
using Kurskcartuning.Server_v2.Core.Dtos.App;
using Kurskcartuning.Server_v2.Core.Dtos.App.Vehicle;
using Kurskcartuning.Server_v2.Core.Entities.AppDB;
using Kurskcartuning.Server_v2.Core.Interfaces.App;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Kurskcartuning.Server_v2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;

        public VehicleController(IVehicleService service)
        {
            _vehicleService = service;
        }

        [HttpPost("new-vehicle")]
        [Authorize(Roles = StaticUserRoles.OwnerAdminUserPremium)]
        public async Task<ActionResult<GeneralAppServiceResponceDto>> NewVehicle(VehiclePostDto dto)
        {
            var newVehicle = await _vehicleService.PostVehicleAsync(User, dto, UserId);
            if (newVehicle.IsSucced is false )
                return StatusCode(newVehicle.StatusCode, newVehicle.Message);

            return Ok(newVehicle);
        }


        [HttpGet("get-vehicle-id/{id}")]
        [Authorize(Roles = StaticUserRoles.OwnerAdminUserPremium)]
        public async Task<IActionResult> GetVehicleId(long id)
        {
           var newVehicleId = await _vehicleService.GetVehicleIdAsync(id, UserId);
            
            if (newVehicleId.IsSucced is false ) 
                return StatusCode(newVehicleId.StatusCode, newVehicleId.Message);

            return Ok(newVehicleId);
        }

        [HttpPut("put-vehicle-id/{id}")]
        [Authorize(Roles = StaticUserRoles.OwnerAdminUserPremium)]
        public async Task<IActionResult> PutVehicleId(long id, VehiclePutDto dto)
        {
            var newVehicle = await _vehicleService.PutVehicleAsync(id, UserId, dto);

            if (newVehicle.IsSucced is false)
                return StatusCode(newVehicle.StatusCode, newVehicle.Message);

            return Ok(newVehicle);
        }


        [HttpGet("get-vehicles")]
        [Authorize(Roles = StaticUserRoles.OwnerAdminUserPremium)]
        public async Task<IActionResult> GetVehicles(long id)
        {
            var vehicles = await _vehicleService.GetVehicleListAsync(id, UserId);

            if (vehicles.IsSucced is false)
                return StatusCode(vehicles.StatusCode, vehicles.Message);

            return Ok(vehicles);
        }


        private string UserId => HttpContext.User.Claims.First(x => x.Type == CustomClaimTypes.Id).Value;
    }
}
