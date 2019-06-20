using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimelinesAPI.Settings;

namespace TimelinesAPI.DataVaults
{
    public class RecordTableStorageVaults : TableStorageVaultsBase<RecordEntity>
    {
        public RecordTableStorageVaults(StorageAccountSettings settings, SimpleCacheService<List<RecordEntity>> cacheService) : base(settings, cacheService)
        {
        }

        protected override string TableName => "records";
    }
}
