using System.ComponentModel.DataAnnotations;

namespace Kurskcartuning.Server_v2.Core.Dtos.Auth;

public class RegisterDto
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    [Required(ErrorMessage = "Введите имя пользователя")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "Введите номер телефона")]
    public string Phone {  get; set; }

    public string Email { get; set; }

    [Required(ErrorMessage = "Введите пароль")]
    public string Password { get; set; }

    public string Address { get; set; }
}

