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
    public class TableStorageVaults
    {
        public const string MOMENT_TABLE_NAME = "moments";

        private readonly StorageAccountSettings _settings;
        public TableStorageVaults(IOptions<StorageAccountSettings> settings)
        {
            _settings = settings.Value;
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

        public async Task<CloudTable> CreateTableAsync(string tableName)
        {
            var tableClient = GetAccount().CreateCloudTableClient();
            var table = tableClient.GetTableReference(tableName);
            await table.CreateIfNotExistsAsync();
            return table;
        }

        public async Task<bool> InsertOrReplaceMomentAsync(MomentEntity entity)
        {
            var table = await CreateTableAsync(MOMENT_TABLE_NAME);
            var operation = TableOperation.InsertOrReplace(entity);

            try
            {
                var result = await table.ExecuteAsync(operation);
                return result.HttpStatusCode == 204;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<bool> DeleteMomentAsync(string partitionKey, string rowKey)
        {
            var table = await CreateTableAsync(MOMENT_TABLE_NAME);
            var retriveOpt = TableOperation.Retrieve<MomentEntity>(partitionKey, rowKey);
            var retriveResult = await table.ExecuteAsync(retriveOpt);

            if (retriveResult.Result is MomentEntity entity)
            {
                var deleteOpt = TableOperation.Delete(entity);
                var result = await table.ExecuteAsync(deleteOpt);
                return result.HttpStatusCode == 204;
            }
            else
            {
                return false;
            }
        }

        public async Task<List<MomentEntity>> GetMomentsAsync(string topic)
        {
            var tableClient = GetAccount().CreateCloudTableClient();
            var table = tableClient.GetTableReference(MOMENT_TABLE_NAME);

            var list = new List<MomentEntity>();
            var query = new TableQuery<MomentEntity>().Where(
                TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, topic));
            TableContinuationToken token = null;
            do
            {
                var results = await table.ExecuteQuerySegmentedAsync(query, token);
                token = results.ContinuationToken;
                list.AddRange(results);
            } while (token != null);

            return list;
        }
    }
}
