using Kurskcartuning.Server_v2.Core.Dtos.App.Client;
using System.Security.Claims;

namespace Kurskcartuning.Server_v2.Core.Interfaces.App;

    public interface IClientService
    {
    Task<ClientServiceResponceDto> PostClientAsync(ClaimsPrincipal User, ClientPostDto dto, string id);

    Task<ClientServiceResponceDto> GetClientsAsync(string id);

    Task<ClientServiceResponceDto> GetClientIdAsync(long id);

    Task<ClientServiceResponceDto> PutClientAsync(ClaimsPrincipal User, ClientPutDto dto);

    Task<ClientServiceResponceDto> DeleteClientAsync(ClaimsPrincipal User, ClientDeleteDto dto);
}

