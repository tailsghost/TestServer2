using System.ComponentModel.DataAnnotations;

namespace Kurskcartuning.Server_v2.Core.Entities.AppDB;

public class BaseEntity<TID>
{
    [Key]
    public TID Id { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public DateTime UpdateAt { get; set; } = DateTime.Now;
}

