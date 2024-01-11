using System.ComponentModel.DataAnnotations;

namespace Kurskcartuning.Server_v2.Core.Entities.AppDB;

public class Manufacturer: BaseEntity<long>
{
    public string Value { get; set; }

    public long VehicleId { get; set; }

    public Vehicle Vehicle { get; set; }

    public List<Model> Models { get; set; }
}

