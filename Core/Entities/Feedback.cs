namespace Kurskcartuning.Server_v2.Core.Entities;

public class Feedback: BaseEntity<long>
{
    public string UserName { get; set; }

    public string ClientName { get; set; }

    public string VehicleName { get; set; }

    public string VisitName { get; set; }

    public string Text { get; set; }
}

