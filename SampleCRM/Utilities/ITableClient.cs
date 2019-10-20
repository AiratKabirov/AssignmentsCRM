using SampleCRM.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SampleCRM.Utilities
{
    public interface ITableClient
    {
        Task<IReadOnlyList<Assignment>> ListAllEntities();

        Task<Assignment> GetEntityById(string id);

        Task<Assignment> InsertOrMergeEntityAsync(Assignment entity);

        Task DeleteEntityAsync(string id);
    }
}
