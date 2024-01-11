using Kurskcartuning.Server_v2.Config;
using Kurskcartuning.Server_v2.Core.Constants;
using Kurskcartuning.Server_v2.Core.Dtos.Auth;
using Kurskcartuning.Server_v2.Core.Dtos.General;
using Kurskcartuning.Server_v2.Core.Entities.Application;
using Kurskcartuning.Server_v2.Core.Entities.UserStoreCustom;
using Kurskcartuning.Server_v2.Core.Enum;
using Kurskcartuning.Server_v2.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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
        var isExistsUserPhone = await _userStoreCustom.FindByPhoneNumberAsync(registerDto.PhoneNumber);

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
            PhoneNumber = registerDto.PhoneNumber,
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
        if (user is null)
            return null;

        var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, loginDto.Password);
        if (!isPasswordCorrect)
            return null;

        var newToken = await GenerateJWTTokenAsync(user);
        var roles = await _userManager.GetRolesAsync(user);
        var userInfo = GenerateUserInfoObject(user, roles);
        await _logService.SaveNewLog(user.UserName, "Вошел в систему");

        return new LoginServiceResponceDto()
        {
            NewToken = newToken,
            UserInfo = userInfo
        };

    }

    public async Task<GeneralServiceResponceDto> UpdateRoleAsync(ClaimsPrincipal User, UpdateRoleDto updateRoleDto)
    {
        var user = await _userManager.FindByNameAsync(updateRoleDto.UserName);
        if (user is null)
            return new GeneralServiceResponceDto()
            {
                IsSucced = false,
                StatusCode = 404,
                Message = "Неверное имя пользователя"
            };

        var userRoles = await _userManager.GetRolesAsync(user);

        if (User.IsInRole(StaticUserRoles.Admin))
        {
            if (updateRoleDto.NewRole.Equals(RoleType.User) || updateRoleDto.NewRole.Equals(RoleType.UserPremium))
            {
                if (userRoles.Any(q => q.Equals(StaticUserRoles.Owner) || q.Equals(StaticUserRoles.Admin)))
                {
                    return new GeneralServiceResponceDto()
                    {
                        IsSucced = false,
                        StatusCode = 403,
                        Message = "Нет доступа к изменению роли пользователя",
                    };
                }
                else
                {
                    await _userManager.RemoveFromRolesAsync(user, userRoles);
                    await _userManager.AddToRoleAsync(user, updateRoleDto.NewRole.ToString());
                    await _logService.SaveNewLog(user.UserName, "Роли пользователя обновлены");
                    return new GeneralServiceResponceDto()
                    {
                        IsSucced = true,
                        StatusCode = 200,
                        Message = "Роли успешно обновлены",
                    };
                }
            }
            else return new GeneralServiceResponceDto()
            {
                IsSucced = false,
                StatusCode = 403,
                Message = "Нет доступа к изменению роли пользователя"
            };

        }
        else
        {
            if (userRoles.Any(q => q.Equals(StaticUserRoles.Owner)))
            {
                return new GeneralServiceResponceDto()
                {
                    IsSucced = false,
                    StatusCode = 403,
                    Message = "Нет доступа к изменению роли пользователя"
                };
            }

            else
            {
                await _userManager.RemoveFromRolesAsync(user, userRoles);
                await _userManager.AddToRoleAsync(user, updateRoleDto.NewRole.ToString());
                await _logService.SaveNewLog(user.UserName, "Роли пользователя обновлены");


                return new GeneralServiceResponceDto()
                {
                    IsSucced = true,
                    StatusCode = 200,
                    Message = "Роли успешно обновлены"
                };
            }
            
        }
    }

    public async Task<LoginServiceResponceDto?> MeAsync(MeDto meDto)
    {
        ClaimsPrincipal handler = new JwtSecurityTokenHandler().ValidateToken(meDto.Token, new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = _configuration["JWT:ValidIssuer"],
            ValidAudience = _configuration["JWT:ValidAudience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]))
        }, out SecurityToken securityToken);

        string decodedUserName = handler.Claims.First(q => q.Type == ClaimTypes.Name).Value;
        if(decodedUserName is null)
            return null;

        var user = await _userManager.FindByNameAsync(decodedUserName);
        if (user is null)
            return null;

        var newToken = await GenerateJWTTokenAsync(user);
        var roles = await _userManager.GetRolesAsync(user);
        var userInfo = GenerateUserInfoObject(user, roles);
        await _logService.SaveNewLog(user.UserName, "Сгенерирован новый Токен");

        return new LoginServiceResponceDto()
        {
            NewToken = newToken,
            UserInfo = userInfo,
        };

    }

    public async Task<IEnumerable<UserInfoResult>> GetUsersListAsync()
    {
        var users = await _userManager.Users.ToListAsync();

        List<UserInfoResult> userInfoResults = new List<UserInfoResult>();

        foreach(var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var userInfo = GenerateUserInfoObject(user, roles);
            userInfoResults.Add(userInfo);
        }

        return userInfoResults;
    }

    public async Task<UserInfoResult?> GetUserDetailsByUserNameAsync(string userName)
    {
        var user = await _userManager.FindByNameAsync(userName);
        if(user is null)
            return null;

        var roles = await _userManager.GetRolesAsync(user);
        var userInfo = GenerateUserInfoObject(user, roles);

        return userInfo;
    }

    public async Task<IEnumerable<string>> GetUsernamesListAsync()
    {
        var userNames = await _userManager.Users.Select(q => q.UserName).ToListAsync();

        return userNames;
    }


    private async Task<string> GenerateJWTTokenAsync(ApplicationUser user)
    {
        var userRoles = await _userManager.GetRolesAsync(user);

        var authClaims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(CustomClaimTypes.Id, user.Id),
            new Claim("FirstName", user.FirstName),
            new Claim("LastName", user.LastName),
        };

        foreach (var userRole in userRoles)
        {
            authClaims.Add(new Claim(ClaimTypes.Role, userRole));
        }

        var authSecret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
        var signinCredentials = new SigningCredentials(authSecret, SecurityAlgorithms.HmacSha256);

        var tokenObject = new JwtSecurityToken(
            issuer: _configuration["JWT:ValidIssuer"],
            audience: _configuration["JWT:ValidAudience"],
            notBefore: DateTime.Now,
            expires: DateTime.Now.AddHours(5),
            claims: authClaims,
            signingCredentials: signinCredentials
            );


        string token = new JwtSecurityTokenHandler().WriteToken(tokenObject);
        return token;
    }

    private UserInfoResult GenerateUserInfoObject(ApplicationUser user, IEnumerable<string> Roles)
    {
        return new UserInfoResult()
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            UserName = user.UserName,
            Email = user.Email,
            Roles = Roles,
            CreatedAt = user.CreatedAt,
            PhoneNumber = user.PhoneNumber,
        };
    }
}

