using System.Collections.Generic;
using System.Threading.Tasks;
using SampleCRM.Models;
using SampleCRM.Utilities;

namespace SampleCRM.Services
{
    public class AssignmentsService : IAssignmentsService
    {
        ITableClient tableClient;

        public AssignmentsService(ITableClient tableClient)
        {
            this.tableClient = tableClient;
        }

        public async Task<IReadOnlyList<Assignment>> ListAssignments()
        {
            return await tableClient.ListAllEntities();
        }

        public async Task<Assignment> GetAssignment(string id)
        {
            return await this.tableClient.GetEntityById(id);
        }

        public async Task<Assignment> CreateOrUpdateAssignment(Assignment assignment)
        {
            return await this.tableClient.InsertOrMergeEntityAsync(assignment);
        }

        public async Task DeleteAssignment(string id)
        {
            await this.tableClient.DeleteEntityAsync(id);
        }
    }
}
