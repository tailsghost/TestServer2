using Kurskcartuning.Server_v2.Core.Dtos.App;
using Kurskcartuning.Server_v2.Core.Dtos.App.Vehicle;
using System.Security.Claims;

namespace Kurskcartuning.Server_v2.Core.Interfaces.App;
public interface IVehicleService
{
    Task<GeneralAppServiceResponceDto> PostVehicleAsync(ClaimsPrincipal User, VehiclePostDto dto, string id);
    Task<GeneralAppServiceResponceDto> GetVehicleIdAsync(long id, string userId);
    Task<GeneralAppServiceResponceDto> PutVehicleAsync(long id, string userId, VehiclePutDto dto);
    Task<GeneralAppServiceResponceDto> GetVehicleListAsync(long clientId, string userId);
}

