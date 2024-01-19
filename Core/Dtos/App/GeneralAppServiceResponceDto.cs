namespace Kurskcartuning.Server_v2.Core.Dtos.App;

public class GeneralAppServiceResponceDto
{
    public bool IsSucced { get; set; }

    public int StatusCode { get; set; }

    public string Message { get; set; }

    public object? Responce { get; set; }
}

