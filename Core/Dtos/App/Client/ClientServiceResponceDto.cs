namespace Kurskcartuning.Server_v2.Core.Dtos.App.Client;

public class ClientServiceResponceDto
{
    public bool IsSucced { get; set; }

    public int StatusCode { get; set; }

    public string Message { get; set; }

    public object? Client {  get; set; }
}

