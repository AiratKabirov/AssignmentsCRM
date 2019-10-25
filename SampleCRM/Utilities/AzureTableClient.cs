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
        private string partitionKey { get; }
        private CloudTable cloudTable { get; }
        private ILogger logger { get; }

        public AzureTableClient(ILogger<AzureTableClient> logger)
        {
            this.logger = logger;
            var settings = AppSettings.LoadAppSettings();
            this.partitionKey = settings.PartitionKey;
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(settings.StorageConnectionString);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient(new TableClientConfiguration());
            CloudTable table = tableClient.GetTableReference(settings.TableName);
            this.cloudTable = table;
        }

        public async Task<IEnumerable<Assignment>> ListAllEntities()
        {
            try
            {
                TableContinuationToken token = null;
                var entities = new List<Assignment>();

                do
                {
                    var queryResult = await this.cloudTable.ExecuteQuerySegmentedAsync(new TableQuery<Assignment>(), token);
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

        public async Task<Assignment> GetEntityById(string id)
        {
            try
            {
                TableOperation retrieveOperation = TableOperation.Retrieve<Assignment>(this.partitionKey, id.ToString());
                TableResult result = await this.cloudTable.ExecuteAsync(retrieveOperation);
                return result.Result as Assignment;
            }
            catch (StorageException e)
            {
                logger.LogError(e.ToString());
                throw;
            }
        }

        public async Task<Assignment> InsertOrMergeEntityAsync(Assignment entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            try
            {
                if (string.IsNullOrWhiteSpace(entity.PartitionKey))
                {
                    entity.PartitionKey = this.partitionKey;
                }

                if (string.IsNullOrWhiteSpace(entity.RowKey))
                {
                    entity.RowKey = Guid.NewGuid().ToString();
                }

                TableOperation insertOrMergeOperation = TableOperation.InsertOrMerge(entity);
                TableResult result = await this.cloudTable.ExecuteAsync(insertOrMergeOperation);
                Assignment insertedCustomer = result.Result as Assignment;

                return insertedCustomer;
            }
            catch (StorageException e)
            {
                logger.LogError(e.ToString());
                throw;
            }
        }

        public async Task DeleteEntityAsync(string id)
        {
            try
            {
                var deleteEntity = await this.GetEntityById(id);
                if (deleteEntity == null)
                {
                    throw new NotFoundException("Entity with such id was not found");
                }

                TableOperation deleteOperation = TableOperation.Delete(deleteEntity);
                TableResult result = await this.cloudTable.ExecuteAsync(deleteOperation);
            }
            catch (StorageException e)
            {
                logger.LogError(e.ToString());
                throw;
            }
        }
    }
}
