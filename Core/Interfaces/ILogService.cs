using Kurskcartuning.Server_v2.Core.Dtos.Log;
using System.Security.Claims;

namespace Kurskcartuning.Server_v2.Core.Interfaces;

public interface ILogService
{
    Task SaveNewLog(string UserName, string Description);

    Task<IEnumerable<GetLogDto>> GetLogsAsync();

    Task<IEnumerable<GetLogDto>> GetMyLogsAsync(ClaimsPrincipal User);

}

