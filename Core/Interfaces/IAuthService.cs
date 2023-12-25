using Kurskcartuning.Server_v2.Core.Dtos.Auth;
using Kurskcartuning.Server_v2.Core.Dtos.General;
using System.Security.Claims;

namespace Kurskcartuning.Server_v2.Core.Interfaces;

public interface IAuthService
{
    Task<GeneralServiceResponceDto> SeedRolesAsync();

    Task<GeneralServiceResponceDto> RegisterAsync(RegisterDto registerDto);

    Task<LoginServiceResponceDto?> LoginAsync(LoginDto loginDto);

    Task<GeneralServiceResponceDto> UpdateRoleAsync(ClaimsPrincipal User, UpdateRoleDto updateRoleDto);

    Task<LoginServiceResponceDto?> MeAsync(MeDto meDto);

    Task<IEnumerable<UserInfoResult>> GetUsersListAsync();

    Task<UserInfoResult?> GetUserDetailsByUserNameAsync(string userName);

    Task<IEnumerable<string>> GetUsernamesListAsync();
}

