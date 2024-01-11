using Kurskcartuning.Server_v2.Core.Constants;
using Kurskcartuning.Server_v2.Core.Dtos.Feedback;
using Kurskcartuning.Server_v2.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Kurskcartuning.Server_v2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackService _feedbackService;

        public FeedbackController(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }


        [HttpPost("create")]
        [Authorize]
        public async Task<IActionResult> CreateNewFeedback([FromBody] CreateFeedbackDto dto)
        {
            var result = await _feedbackService.CreateNewFeedbackAsync(User, dto);

            if (result.IsSucced)
                return Ok(result.Message);

            return StatusCode(result.StatusCode, result.Message);
        }

        [HttpGet("mine")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<GetFeedbackDto>>> GetMyFeedback ()
        {
            var feedback = await _feedbackService.GetMyFeedbackAsync(User);
            return Ok(feedback);
        }

        [HttpGet]
        [Authorize(Roles = StaticUserRoles.OwnerAdmin)]
        public async Task<ActionResult<IEnumerable<GetFeedbackDto>>> GetFeedback ()
        {
            var feedback = await _feedbackService.GetFeedbackAsync();
            return Ok(feedback);
        }
    }
}
