using Kurskcartuning.Server_v2.Core.Constants;
using Kurskcartuning.Server_v2.Core.Dtos.Auth;
using Kurskcartuning.Server_v2.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Kurskcartuning.Server_v2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("seed-roles")]
        public async Task<IActionResult> SeedRoles()
        {
            var seedRoles = await _authService.SeedRolesAsync();
            return StatusCode(seedRoles.StatusCode, seedRoles.Message);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Registr([FromBody] RegisterDto dto)
        {
            var registerResult = await _authService.RegisterAsync(dto);
            return StatusCode(registerResult.StatusCode, registerResult.Message);
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginServiceResponceDto>> Login([FromBody] LoginDto dto)
        {
            var loginResult = await _authService.LoginAsync(dto);
            if (loginResult is null)
                return Unauthorized("Неверный логин или пароль. Пожалуйста, обратитесь к администратору");

            return Ok(loginResult);
        }

        [HttpPost("update-role")]
        [Authorize(Roles = StaticUserRoles.OwnerAdmin)]
        public async Task<IActionResult> UpdateRole([FromBody] UpdateRoleDto dto)
        {
            var updateRolesResult = await _authService.UpdateRoleAsync(User, dto);

            if (updateRolesResult.IsSucced)
                return Ok(updateRolesResult.Message);
            else
                return StatusCode(updateRolesResult.StatusCode, updateRolesResult.Message);
        }

        [HttpPost("me")]
        public async Task<ActionResult<LoginServiceResponceDto>> Me([FromBody] MeDto token)
        {
            try
            {
                var me = await _authService.MeAsync(token);
                if (me is not null)
                    return Ok(me);
                else
                    return Unauthorized("Неверный токен");
            }
            catch (Exception)
            {
                return Unauthorized("Неверный токен");
            }
        }

        [HttpGet("users")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<UserInfoResult>>> GetUsersList()
        {
            var userList = await _authService.GetUsersListAsync();

            return Ok(userList);
        }

        [HttpGet("users/{userName}")]
        public async Task<ActionResult<UserInfoResult>> GetUserDetailsByUserName(string userName)
        {
            var user = await _authService.GetUserDetailsByUserNameAsync(userName);
            if (user is not null)
                return Ok(user);
            else 
                return NotFound("Пользователь не найден");

        }

        [HttpGet("usernames")]
        public async Task<ActionResult<IEnumerable<string>>> GetUserNamesList()
        {
            var usernames = await _authService.GetUsernamesListAsync();

            return Ok(usernames);
        }
    }
}
