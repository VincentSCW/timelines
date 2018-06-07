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
    public abstract class TableStorageVaultsBase<TEntity> : StorageVaultsBase
	    where TEntity : TableEntity, new()
    {
	    private readonly SimpleCacheService<List<TEntity>> _cacheService;

		protected abstract string TableName { get; }
		public TableStorageVaultsBase(IOptions<StorageAccountSettings> settings,
			SimpleCacheService<List<TEntity>> cacheService)
			: base(settings)
		{
			_cacheService = cacheService;
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

	    public virtual async Task<TEntity> GetAsync(string partitionKey, string rowKey)
	    {
		    if (_cacheService.TryGetFromCache(partitionKey, out List<TEntity> value))
			    return value.FirstOrDefault(x => x.RowKey == rowKey);

		    var tableClient = GetAccount().CreateCloudTableClient();
		    var table = tableClient.GetTableReference(TableName);

			var retrieveOperation = TableOperation.Retrieve<TEntity>(partitionKey, rowKey);

			var retrieveResult = await table.ExecuteAsync(retrieveOperation);
		    return retrieveResult.Result as TEntity;
	    }
	}
}
