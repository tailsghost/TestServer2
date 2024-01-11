using System.ComponentModel.DataAnnotations;

namespace Kurskcartuning.Server_v2.Core.Entities.AppDB;

public class Model: BaseEntity<long>
{
    public long ManufacturerId { get; set; }

    public string Value { get; set; }

    public Manufacturer Manufacturer { get; set; }
}

