﻿using System.ComponentModel.DataAnnotations;


namespace Kurskcartuning.Server_v2.Core.Entities.AppDB.VisitInside;

public class ListOfWorks: BaseEntity<long>
{
    public long VisitId { get; set; }

    public string Value { get; set; }

    public Visit Visit { get; set; }
    public List<ListOfWorksPrices> Prices { get; set; }
}

