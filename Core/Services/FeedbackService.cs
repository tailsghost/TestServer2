using Kurskcartuning.Server_v2.Core.DbContext;
using Kurskcartuning.Server_v2.Core.Dtos.Feedback;
using Kurskcartuning.Server_v2.Core.Dtos.General;
using Kurskcartuning.Server_v2.Core.Entities.Application;
using Kurskcartuning.Server_v2.Core.Entities.Application;
using Kurskcartuning.Server_v2.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Kurskcartuning.Server_v2.Core.Services;

public class FeedbackService : IFeedbackService
{

    private readonly ApplicationDbContext _context;
    private readonly ILogService _logService;
    private readonly UserManager<ApplicationUser> _userManager;

    public FeedbackService(ApplicationDbContext context, ILogService logger, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _logService = logger;
        _userManager = userManager;
    }

    public async Task<GeneralServiceResponceDto> CreateNewFeedbackAsync(ClaimsPrincipal User, CreateFeedbackDto CreateFeedbackDto)
    {
        if (User.Identity.Name == CreateFeedbackDto.UserName)
            return new GeneralServiceResponceDto()
            {
                IsSucced = false,
                StatusCode = 400,
                Message = "Отправитель и получатель не могут быть одинаковыми"
            };

        var isUserNameValid = _userManager.Users.Any(q => q.UserName == CreateFeedbackDto.UserName);
        if (isUserNameValid)
            return new GeneralServiceResponceDto()
            {
                IsSucced = false,
                StatusCode = 400,
                Message = "Пользователь не найден"
            };

        Feedback newFeedback = new()
        {
            UserName = User.Identity.Name,
            ClientName = CreateFeedbackDto.ClientName,
            Text = CreateFeedbackDto.Text,
        };

        await _context.AddAsync(newFeedback);
        await _context.SaveChangesAsync();
        await _logService.SaveNewLog(User.Identity.Name, "Отправлен отзыв о клиенте");

        return new GeneralServiceResponceDto()
        {
            IsSucced = true,
            StatusCode = 201,
            Message = "Отзыв успешно отправлен!"
        };
    }

    public async Task<IEnumerable<GetFeedbackDto>> GetFeedbackAsync()
    {
        var feedback = await _context.Feedbacks
            .Select(q => new GetFeedbackDto()
            {
                Id = q.Id,
                UserName = q.UserName,
                ClientName = q.ClientName,
                Text = q.Text,
                CreatedAt = q.CreatedAt,

            })
            .OrderByDescending(q => q.CreatedAt)
            .ToListAsync();

        return feedback;
    }

    public async Task<IEnumerable<GetFeedbackDto>> GetMyFeedbackAsync(ClaimsPrincipal User)
    {
        var loggedInUser = User.Identity.Name;
        var feedback = await _context.Feedbacks
            .Where(q => q.UserName.Equals(loggedInUser))
            .Select(q => new GetFeedbackDto()
            {
                Id = q.Id,
                UserName = q.UserName,
                ClientName = q.ClientName,
                Text = q.Text,
                CreatedAt = q.CreatedAt,
            })
            .OrderByDescending(q => q.CreatedAt)
            .ToListAsync();

        return feedback;
    }
}

