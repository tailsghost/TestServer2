namespace Kurskcartuning.Server_v2.Core.Entities.Application;

public class Log : BaseEntity<long>
{
    public string UserName { get; set; }

    public string Description { get; set; }

}


