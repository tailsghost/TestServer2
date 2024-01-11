namespace Kurskcartuning.Server_v2.Core.Entities.AppDB.VisitInside;

public class ListOfWorksPrices: BaseEntity<long>
{
    public long ListOfWorksId { get; set; }

    public long Price { get; set; }

    public ListOfWorks ListOfWorks { get; set; }
}

