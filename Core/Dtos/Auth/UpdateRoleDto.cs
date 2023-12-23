using System.ComponentModel.DataAnnotations;

namespace Kurskcartuning.Server_v2.Core.Dtos.Auth;

public class UpdateRoleDto
{
    [Required(ErrorMessage = "Введите имя пользователя")]
    public string UserName { get; set; }

    public RoleType NewRole { get; set; }
}

public enum RoleType
{
    Admin,
    UserPremium,
    User
}

