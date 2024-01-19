using Kurskcartuning.Server_v2.Core.Entities.AppDB;

namespace Kurskcartuning.Server_v2.Core.Dtos.App.Vehicle;

public class VehicleResponceLineDto
{
    public long? ClientId { get; set; }

    public string ManufacturerValue { get; set; }

    public string ModelValue { get; set; }

    public string RegistrationNumber { get; set; }
}

