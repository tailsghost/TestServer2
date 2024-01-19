namespace Kurskcartuning.Server_v2.Core.Dtos.App.Vehicle;

public class VehiclePostDto
{
    public long CLientId { get; set; }

    public long Year { get; set; }

    public string RegistrationNumber { get; set; }

    public string? Configuration { get; set; } = null;

    public long? EnginePower { get; set; } = null;

    public string Manufacturer { get; set; }

    public string Model { get; set; }

}

