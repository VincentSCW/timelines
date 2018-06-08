using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using TimelinesAPI.Settings;

namespace TimelinesAPI.DataVaults
{
    public abstract class StorageVaultsBase
    {
	    private readonly StorageAccountSettings _settings;
		public StorageVaultsBase(StorageAccountSettings settings)
	    {
		    _settings = settings;
	    }

		protected CloudStorageAccount GetAccount()
	    {
		    if (CloudStorageAccount.TryParse(
			    _settings.ConnectionString,
			    out CloudStorageAccount storagetAccount))
		    {
			    return storagetAccount;
		    }
		    else
		    {
			    throw new Exception();
		    }
	    }
	}
}
