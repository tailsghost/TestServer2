using Kurskcartuning.Server_v2.Core.Dtos.Feedback;
using Kurskcartuning.Server_v2.Core.Dtos.General;
using System.Security.Claims;

namespace Kurskcartuning.Server_v2.Core.Interfaces;

public interface IFeedbackService
{
    Task<GeneralServiceResponceDto> CreateNewFeedbackAsync(ClaimsPrincipal User, CreateFeedbackDto CreateFeedbackDto);

    Task<IEnumerable<GetFeedbackDto>> GetFeedbackAsync();

    Task<IEnumerable<GetFeedbackDto>> GetMyFeedbackAsync(ClaimsPrincipal User);
}

