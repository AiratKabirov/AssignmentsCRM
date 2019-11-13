using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Logging;
using SampleCRM.Models;

namespace SampleCRM.Utilities
{
    public class AzureTableClient : ITableClient
    {
        private ILogger logger { get; }

        public AzureTableClient(ILogger<AzureTableClient> logger)
        {
            this.logger = logger;
        }

        public async Task<IEnumerable<T>> ListAllEntities<T>(string tableName) where T : ITableEntity, new()
        {
            try
            {
                TableContinuationToken token = null;
                var entities = new List<T>();

                do
                {
                    var table = GetTable(tableName);
                    var queryResult = await table.ExecuteQuerySegmentedAsync(new TableQuery<T>(), token);
                    entities.AddRange(queryResult.Results);
                    token = queryResult.ContinuationToken;
                }
                while (token != null);

                return entities;
            }
            catch (StorageException e)
            {
                logger.LogError(e.ToString());
                throw;
            }
        }

        public async Task<T> GetEntityById<T>(string tableName, string outerId, string innerId) where T : ITableEntity
        {
            try
            {
                var table = GetTable(tableName);
                TableOperation retrieveOperation = TableOperation.Retrieve<T>(outerId, innerId);
                TableResult result = await table.ExecuteAsync(retrieveOperation);
                return (T)result.Result;
            }
            catch (StorageException e)
            {
                logger.LogError(e.ToString());
                throw;
            }
        }

        public async Task<T> InsertOrMergeEntityAsync<T>(string tableName, T entity) where T : ITableEntity
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            try
            {
                if (string.IsNullOrWhiteSpace(entity.PartitionKey))
                {
                    entity.PartitionKey = "f564303b-49f7-4bcd-9f3b-e5199fc9d354";
                }

                if (string.IsNullOrWhiteSpace(entity.RowKey))
                {
                    entity.RowKey = Guid.NewGuid().ToString();
                }

                var table = GetTable(tableName);
                TableOperation insertOrMergeOperation = TableOperation.InsertOrMerge(entity);
                TableResult result = await table.ExecuteAsync(insertOrMergeOperation);

                return (T)result.Result;
            }
            catch (StorageException e)
            {
                logger.LogError(e.ToString());
                throw;
            }
        }

        public async Task DeleteEntityAsync<T>(string tableName, string outerId, string innerId) where T : ITableEntity
        {
            try
            {
                var table = GetTable(tableName);
                var deleteEntity = await this.GetEntityById<T>(tableName, outerId, innerId);
                TableOperation deleteOperation = TableOperation.Delete(deleteEntity);
                TableResult result = await table.ExecuteAsync(deleteOperation);
            }
            catch (ArgumentNullException ex)
            {
                logger.LogError(ex.ToString());
                throw new NotFoundException("Entity with such id was not found");
            }
            catch (StorageException ex)
            {
                logger.LogError(ex.ToString());
                if (ex?.Message?.Contains("does not exist") ?? false)
                {
                    throw new NotFoundException("Entity with such id was not found");
                }

                throw;
            }
        }

        private CloudTable GetTable(string tableName)
        {
            var settings = AppSettings.LoadAppSettings();
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(settings.StorageConnectionString);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient(new TableClientConfiguration());
            CloudTable table = tableClient.GetTableReference(tableName);
            return table;
        }
    }
}
