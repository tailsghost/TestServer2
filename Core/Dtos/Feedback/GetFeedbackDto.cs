namespace Kurskcartuning.Server_v2.Core.Dtos.Feedback;

public class GetFeedbackDto
{
    public long Id { get; set; }

    public string UserName { get; set; }

    public string VehicleName { get; set; }

    public string VisitString { get; set; }

    public string Text { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

