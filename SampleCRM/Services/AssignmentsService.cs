using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using SampleCRM.Utilities;
using SampleCRM.ViewModels;
using SampleCRM.Models;

namespace SampleCRM.Services
{
    public class AssignmentsService : IDataService<AssignmentViewModel>
    {
        ITableClient tableClient;
        private readonly string tableName = "Assignments";

        public AssignmentsService(ITableClient tableClient)
        {
            this.tableClient = tableClient;
        }

        public async Task<IEnumerable<AssignmentViewModel>> ListEntities()
        {
            var assignments = await tableClient.ListAllEntities<Assignment>(tableName);
            return assignments.Select(assignment => assignment.GetAssignmentViewModel());
        }

        public async Task<AssignmentViewModel> GetEntity(string outerId, string innerId)
        {
            var assignment = await this.tableClient.GetEntityById<Assignment>(tableName, outerId, innerId);

            if (assignment == null)
            {
                throw new NotFoundException("Entity with such id was not found");
            }

            return assignment.GetAssignmentViewModel();
        }

        public async Task<AssignmentViewModel> CreateEntity(AssignmentViewModel assignmentViewModel)
        {
            var existingAssignment = string.IsNullOrWhiteSpace(assignmentViewModel.Id) 
                ? null 
                : await this.tableClient.GetEntityById<Assignment>(tableName, assignmentViewModel.ProjectId, assignmentViewModel.Id);

            if (existingAssignment != null)
            {
                throw new BadRequestException("Assignment this such id already exists");
            }

            var assignment = await this.tableClient.InsertOrMergeEntityAsync(tableName, assignmentViewModel.GetAssignment());
            return assignment.GetAssignmentViewModel();
        }

        public async Task<AssignmentViewModel> UpdateEntity(string outerId, string innerId, AssignmentViewModel assignmentViewModel)
        {
            // project id in URI and assignment's project id should be the same
            if (!string.IsNullOrWhiteSpace(assignmentViewModel.ProjectId) && outerId != assignmentViewModel.ProjectId)
            {
                throw new BadRequestException("Cannot update the project id field");
            }

            // id in URI and assignment's row key should be the same
            if (!string.IsNullOrWhiteSpace(assignmentViewModel.Id) && innerId != assignmentViewModel.Id)
            {
                throw new BadRequestException("Cannot update the id field");
            }

            assignmentViewModel.ProjectId = outerId;
            assignmentViewModel.Id = innerId;
            var assignment = await this.tableClient.InsertOrMergeEntityAsync(tableName, assignmentViewModel.GetAssignment());
            return assignment.GetAssignmentViewModel();
        }

        public async Task DeleteEntity(string outerId, string innerId)
        {
            await this.tableClient.DeleteEntityAsync<Assignment>(tableName, outerId, innerId);
        }
    }
}
