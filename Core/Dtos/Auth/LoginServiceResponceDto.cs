namespace Kurskcartuning.Server_v2.Core.Dtos.Auth;

public class LoginServiceResponceDto
{
    public string NewToken { get; set; }

    public UserInfoResult UserInfo { get; set; }
}

