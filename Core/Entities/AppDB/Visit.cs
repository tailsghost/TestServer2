using Kurskcartuning.Server_v2.Core.Entities.AppDB.VisitInside;

namespace Kurskcartuning.Server_v2.Core.Entities.AppDB;

public class Visit: BaseEntity<long>
{
    public long VehicleId { get; set; }

    public string? ChiptuningFileOld { get; set; }

    public string? ChiptuningFileNew { get; set; }

    public long Mileage { get; set; }

    public string Condition { get; set; }

    public Vehicle Vehicle { get; set; }

    public List<ScreenShot>? ScreenShots { get; set; } // Список скриншотов

    public List<ListOfWorks>? ListWorks { get; set; } // Список работ

    public List<Malfunction>? Malfunctions { get; set; } // Список неисправностей авто

    public List<Suggestion>? Suggestions { get; set; } // Список рекомендаций после визита
}

