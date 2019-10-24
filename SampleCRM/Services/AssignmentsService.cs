using System;
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

        public async Task<Assignment> CreateAssignment(Assignment assignment)
        {
            // ids for new assignments should be created and managed by our system, even if client sends particular id, override it here
            assignment.RowKey = Guid.NewGuid().ToString();
            return await this.tableClient.InsertOrMergeEntityAsync(assignment);
        }

        public async Task<Assignment> UpdateAssignment(string id, Assignment assignment)
        {
            // id in URI and assignment's row key should be the same
            assignment.RowKey = id;
            return await this.tableClient.InsertOrMergeEntityAsync(assignment);
        }

        public async Task DeleteAssignment(string id)
        {
            await this.tableClient.DeleteEntityAsync(id);
        }
    }
}
