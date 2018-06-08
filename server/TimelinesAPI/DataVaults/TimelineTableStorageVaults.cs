using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using TimelinesAPI.Settings;

namespace TimelinesAPI.DataVaults
{
    public class TimelineTableStorageVaults : TableStorageVaultsBase<TimelineEntity>
    {
	    public TimelineTableStorageVaults(StorageAccountSettings settings,
		    SimpleCacheService<List<TimelineEntity>> cacheService) 
		    : base(settings, cacheService)
	    {
	    }

	    protected override string TableName => "timelines";
    }
}
