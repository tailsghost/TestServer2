namespace Kurskcartuning.Server_v2.Core.Dtos.App.Vehicle;

public class VehicleResponceIdDto
{
    public string RegistrationNumber { get; set; }

    public long Year { get; set; }

    public string? Configuration { get; set; }

    public long? EnginePower { get; set; }

    public string ManufacturerValue { get; set; }

    public string ModelValue { get; set; }
}

