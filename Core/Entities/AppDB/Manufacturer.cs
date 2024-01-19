using System.ComponentModel.DataAnnotations;

namespace Kurskcartuning.Server_v2.Core.Entities.AppDB;

public class Manufacturer: BaseEntity<long>
{
    public string Value { get; set; }

    public List<Vehicle> Vehicles { get; set; } = new();

    public List<Model> Models { get; set; } = new();
}

