using Kurskcartuning.Server_v2.Core.Entities.AppDB;

namespace Kurskcartuning.Server_v2.Core.Dtos.App.Client;

public class ClientResponceDto
{
    public long Id { get; set; }

    public string FirstName { get; set; }

    public string Phone {  get; set; }

    public List<Entities.AppDB.Vehicle>? Vehicles { get; set; } = new();
}

