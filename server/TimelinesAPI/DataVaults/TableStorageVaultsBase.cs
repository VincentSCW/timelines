using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using TimelinesAPI.Settings;

namespace TimelinesAPI.DataVaults
{
    public abstract class TableStorageVaultsBase<TEntity> where TEntity : TableEntity, new()
    {
		private readonly StorageAccountSettings _settings;
	    private readonly SimpleCacheService<List<TEntity>> _cacheService;

		protected abstract string TableName { get; }
		public TableStorageVaultsBase(IOptions<StorageAccountSettings> settings,
			SimpleCacheService<List<TEntity>> cacheService)
		{
			_settings = settings.Value;
			_cacheService = cacheService;
		}

		private CloudStorageAccount GetAccount()
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

		protected async Task<CloudTable> CreateTableAsync(string tableName)
		{
			var tableClient = GetAccount().CreateCloudTableClient();
			var table = tableClient.GetTableReference(tableName);
			await table.CreateIfNotExistsAsync();
			return table;
		}

		public virtual async Task<bool> InsertOrReplaceAsync(TEntity entity)
		{
			var table = await CreateTableAsync(TableName);
			var operation = TableOperation.InsertOrReplace(entity);

			try
			{
				var result = await table.ExecuteAsync(operation);
				_cacheService.RemoveFromCache(entity.PartitionKey);
				return result.HttpStatusCode == 204;
			}
			catch //(Exception e)
			{
				return false;
			}
		}

		public virtual async Task<bool> DeleteAsync(string partitionKey, string rowKey)
		{
			var table = await CreateTableAsync(TableName);
			var retriveOpt = TableOperation.Retrieve<TEntity>(partitionKey, rowKey);
			var retriveResult = await table.ExecuteAsync(retriveOpt);

			if (retriveResult.Result is TEntity entity)
			{
				var deleteOpt = TableOperation.Delete(entity);
				var result = await table.ExecuteAsync(deleteOpt);
				_cacheService.RemoveFromCache(entity.PartitionKey);
				return result.HttpStatusCode == 204;
			}
			else
			{
				return false;
			}
		}

		public virtual async Task<List<TEntity>> GetListAsync(string partitionKey)
		{
			if (_cacheService.TryGetFromCache(partitionKey, out List<TEntity> value))
				return value;

			var tableClient = GetAccount().CreateCloudTableClient();
			var table = tableClient.GetTableReference(TableName);

			var list = new List<TEntity>();
			var query = new TableQuery<TEntity>().Where(
				TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey));
			TableContinuationToken token = null;
			do
			{
				var results = await table.ExecuteQuerySegmentedAsync(query, token);
				token = results.ContinuationToken;
				list.AddRange(results);
			} while (token != null);

			_cacheService.StoreInCache(partitionKey, list);
			return list;
		}
	}
}
