using System.ComponentModel.DataAnnotations;

namespace Kurskcartuning.Server_v2.Core.Entities.AppDB;

public class Vehicle: BaseEntity<long>
{
    public long? ClientId { get; set; }

    public long? ManufacturerId { get; set; }

    public string RegistrationNumber { get; set; }

    public long Year { get; set; }

    public string? Configuration { get; set; }

    public long? EnginePower { get; set; }

    public Client Client { get; set; }

    public List<Visit>? Visits { get; set; }

    public Manufacturer Manufacturer { get; set; }
}

