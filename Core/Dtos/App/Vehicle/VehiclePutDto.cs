namespace Kurskcartuning.Server_v2.Core.Dtos.App.Vehicle;

public class VehiclePutDto
{
    public string RegistrationNumber { get; set; }

    public string? Configuration { get; set; }

    public long? EnginePower { get; set; }
}

