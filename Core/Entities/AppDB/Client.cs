namespace Kurskcartuning.Server_v2.Core.Entities.AppDB;

public class Client: BaseEntity<long>
{
    public string FirstName { get; set; }

    public string Phone { get; set; }

    public string UserId { get; set; }

    public List<Vehicle> Vehicles { get; set; } = new();
}

