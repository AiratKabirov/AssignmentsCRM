using Microsoft.Azure.Cosmos.Table;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SampleCRM.Utilities
{
    public interface ITableClient
    {
        Task<IEnumerable<T>> ListAllEntities<T>(string tableName) where T : ITableEntity, new();

        Task<T> GetEntityById<T>(string tableName, string outerId, string innerId) where T : ITableEntity;

        Task<T> InsertOrMergeEntityAsync<T>(string tableName, T entity) where T : ITableEntity;

        Task DeleteEntityAsync<T>(string tableName, string outerId, string innerId) where T : ITableEntity;
    }
}
