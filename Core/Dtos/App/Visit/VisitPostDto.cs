using Kurskcartuning.Server_v2.Core.Entities.AppDB.VisitInside;

namespace Kurskcartuning.Server_v2.Core.Dtos.App.Visit;

public class VisitPostDto
{
    public long CLientId { get; set; }
    public long VehicleId { get; set; }
    public IFormFile? FileNew { get; set; }
    public IFormFile? FileOld { get; set; }
    public long Mileage { get; set; }
    public string Condition { get; set; }
    public List<IFormFile> ScreenShots { get; set; }
    public Dictionary<string, long>? ListWorks { get; set; } // Список работ
    public List<string>? Malfunctions { get; set; } // Список неисправностей авто
    public List<string>? Suggestions { get; set; } // Список рекомендаций после визита
}

