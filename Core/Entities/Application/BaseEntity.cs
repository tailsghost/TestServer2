﻿namespace Kurskcartuning.Server_v2.Core.Entities.Application;

public class BaseEntity<TID>
{
    public TID Id { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public DateTime UpdateAt { get; set; } = DateTime.Now;

    public bool IsActive { get; set; } = true;

    public bool IsDeleted { get; set; } = false;
}

