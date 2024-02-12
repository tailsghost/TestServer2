using Kurskcartuning.Server_v2.Core.Constants;
using Kurskcartuning.Server_v2.Core.Dtos.App.Client;
using Kurskcartuning.Server_v2.Core.Dtos.App;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Kurskcartuning.Server_v2.Core.DbContext;
using Kurskcartuning.Server_v2.Core.Interfaces.App;
using Kurskcartuning.Server_v2.Config;
using Kurskcartuning.Server_v2.Core.Dtos.App.Visit;

namespace Kurskcartuning.Server_v2.Controllers;

[Route("api/[controller]")]
[ApiController]
public class VisitController : ControllerBase
{

    private readonly IVisitService _visitService;

    public VisitController(IVisitService service)
    {
        _visitService = service;
    }

    [HttpPost("new-client")]
    [Authorize(Roles = StaticUserRoles.OwnerAdminUserPremium)]
    public async Task<ActionResult<GeneralAppServiceResponceDto>> PostVisit([FromForm] VisitPostDto dto)
    {
        var newVisit = await _visitService.PostVisit(User, dto, UserId);

        if (newVisit.IsSucced is false)
            return StatusCode(newVisit.StatusCode, newVisit.Message);

        return newVisit;
    }
    private string UserId => HttpContext.User.Claims.First(x => x.Type == CustomClaimTypes.Id).Value;
}

