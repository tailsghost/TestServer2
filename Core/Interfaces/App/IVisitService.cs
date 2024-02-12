using Kurskcartuning.Server_v2.Core.Dtos.App;
using Kurskcartuning.Server_v2.Core.Dtos.App.Visit;
using System.Security.Claims;

namespace Kurskcartuning.Server_v2.Core.Interfaces.App;

public interface IVisitService
{
    Task<GeneralAppServiceResponceDto> PostVisit(ClaimsPrincipal User, VisitPostDto dto, string id);
}

