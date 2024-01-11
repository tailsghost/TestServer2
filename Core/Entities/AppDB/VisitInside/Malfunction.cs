using System.ComponentModel.DataAnnotations;


namespace Kurskcartuning.Server_v2.Core.Entities.AppDB.VisitInside;

public class Malfunction: BaseEntity<long>
{
    public long VisitId { get; set; }

    public string Value { get; set; }

    public Visit Visit { get; set; }
}

