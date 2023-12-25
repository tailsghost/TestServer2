using Kurskcartuning.Server_v2.Core.Constants;
using Kurskcartuning.Server_v2.Core.Dtos.Auth;
using Kurskcartuning.Server_v2.Core.Dtos.General;
using Kurskcartuning.Server_v2.Core.Entities;
using Kurskcartuning.Server_v2.Core.Entities.UserStoreCustom;
using Kurskcartuning.Server_v2.Core.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Kurskcartuning.Server_v2.Core.Services;

public class AuthService : IAuthService
{

    private readonly UserManager<ApplicationUser> _userManager;
    private readonly UserStoreCustom _userStoreCustom;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ILogService _logService;
    private readonly IConfiguration _configuration;

    public AuthService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ILogService logService, IConfiguration configuration, UserStoreCustom userStoreCustom)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _logService = logService;
        _configuration = configuration;
        _userStoreCustom = userStoreCustom;
    }


    public async Task<GeneralServiceResponceDto> SeedRolesAsync()
    {
        bool isOwnerRoleExists = await _roleManager.RoleExistsAsync(StaticUserRoles.Owner);
        bool isAdminRoleExists = await _roleManager.RoleExistsAsync(StaticUserRoles.Admin);
        bool isUserPremium = await _roleManager.RoleExistsAsync(StaticUserRoles.UserPremium);
        bool isUser = await _roleManager.RoleExistsAsync(StaticUserRoles.User);

        if (isOwnerRoleExists && isAdminRoleExists && isUserPremium && isUser)
            return new GeneralServiceResponceDto()
            {
                IsSucced = true,
                StatusCode = 200,
                Message = "Все роли уже существуют"
            };

        await _roleManager.CreateAsync(new IdentityRole(StaticUserRoles.Owner));
        await _roleManager.CreateAsync(new IdentityRole(StaticUserRoles.Admin));
        await _roleManager.CreateAsync(new IdentityRole(StaticUserRoles.UserPremium));
        await _roleManager.CreateAsync(new IdentityRole(StaticUserRoles.User));

        return new GeneralServiceResponceDto()
        {
            IsSucced = true,
            StatusCode = 201,
            Message = "Все роли добавлены"
        };
    }

    public async Task<GeneralServiceResponceDto> RegisterAsync(RegisterDto registerDto)
    {
        var isExistsUser = await _userManager.FindByNameAsync(registerDto.UserName);
        var isExistsUserPhone = await _userStoreCustom.FindByPhoneNumberAsync(registerDto.Phone);

        if (isExistsUser is not null)
            return new GeneralServiceResponceDto()
            {
                IsSucced = false,
                StatusCode = 409,
                Message = "Пользователь с таким именем уже существует"
            };

        if (isExistsUserPhone is not null)
            return new GeneralServiceResponceDto()
            {
                IsSucced = false,
                StatusCode = 409,
                Message = "Пользователь с таким номером телефона уже существует"
            };


        ApplicationUser newUser = new ApplicationUser()
        {
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName,
            Email = registerDto.Email,
            UserName = registerDto.UserName,
            Address = registerDto.Address,
            Phone = registerDto.Phone,
            SecurityStamp = Guid.NewGuid().ToString(),
        };


        var createUserResult = await _userManager.CreateAsync(newUser, registerDto.Password);

        if (!createUserResult.Succeeded)
        {
            var errorString = "Ошибка создания пользователя";
            foreach (var error in createUserResult.Errors)
            {
                errorString += " #" + error.Description;
            }

            return new GeneralServiceResponceDto()
            {
                IsSucced = false,
                StatusCode = 400,
                Message = errorString
            };
        }


        await _userManager.AddToRoleAsync(newUser, StaticUserRoles.User);
        await _logService.SaveNewLog(newUser.UserName, "Зарегистрирован в приложении.");

        return new GeneralServiceResponceDto()
        {
            IsSucced = true,
            StatusCode = 201,
            Message = "Пользователь успешно зарегистрирован!"
        };

    }

    public async Task<LoginServiceResponceDto?> LoginAsync(LoginDto loginDto)
    {
        var user = await _userManager.FindByNameAsync(loginDto.UserName);
        if(user is null)
            return null;

        var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, loginDto.Password);
        if (!isPasswordCorrect)
            return null;

        
        
    }

    public Task<UserInfoResult> GetUserDetailsByUserName(string userName)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<string>> GetUsernamesListAsync()
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<UserInfoResult>> GetUsersListAsync()
    {
        throw new NotImplementedException();
    }

    public Task<LoginServiceResponceDto> MeAsync(MeDto meDto)
    {
        throw new NotImplementedException();
    }

    public Task<GeneralServiceResponceDto> UpdateRoleAsync(ClaimsPrincipal User, UpdateRoleDto updateRoleDto)
    {
        throw new NotImplementedException();
    }
}

