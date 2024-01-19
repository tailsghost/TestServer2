using Kurskcartuning.Server_v2.Core.Dtos.App;
using Kurskcartuning.Server_v2.Core.Dtos.App.Client;
using System.Security.Claims;

namespace Kurskcartuning.Server_v2.Core.Interfaces.App;

public interface IClientService
    {
    Task<GeneralAppServiceResponceDto> PostClientAsync(ClaimsPrincipal User, ClientPostDto dto, string id);

    Task<GeneralAppServiceResponceDto> GetClientsAsync(string id);

    Task<GeneralAppServiceResponceDto> GetClientIdAsync(long id);

    Task<GeneralAppServiceResponceDto> PutClientAsync(ClaimsPrincipal User, ClientPutDto dto);

    Task<GeneralAppServiceResponceDto> DeleteClientAsync(ClaimsPrincipal User, ClientDeleteDto dto);
}

