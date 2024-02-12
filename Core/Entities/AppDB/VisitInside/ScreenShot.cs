using System.ComponentModel.DataAnnotations;

namespace Kurskcartuning.Server_v2.Core.Entities.AppDB.VisitInside;

public class ScreenShot: BaseEntity<long>
{
    public long VisitId { get; set; }

    public string? Screen { get; set; }

    public Visit Visit { get; set; }
}

