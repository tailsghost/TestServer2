using System.ComponentModel.DataAnnotations;

namespace Kurskcartuning.Server_v2.Core.Dtos.Auth;

public class LoginDto
{
    [Required(ErrorMessage = "Введите имя пользователя")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "Введите пароль")]
    public string Password { get; set; }
}

