using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using TimelinesAPI.Settings;

namespace TimelinesAPI.DataVaults
{
    public class MomentTableStorageVaults : TableStorageVaultsBase<MomentEntity>
    {
        public MomentTableStorageVaults(StorageAccountSettings settings,
	        SimpleCacheService<List<MomentEntity>> cacheService)
			: base(settings, cacheService)
        {
        }

	    protected override string TableName => "moments";
    }
}
