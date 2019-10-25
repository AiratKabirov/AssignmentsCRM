using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using SampleCRM.Utilities;
using SampleCRM.ViewModels;
using SampleCRM.Models;

namespace SampleCRM.Services
{
    public class AssignmentsService : IAssignmentsService
    {
        ITableClient tableClient;

        public AssignmentsService(ITableClient tableClient)
        {
            this.tableClient = tableClient;
        }

        public async Task<IEnumerable<AssignmentViewModel>> ListAssignments()
        {
            var assignments = await tableClient.ListAllEntities();
            return assignments.Select(assignment => assignment.GetAssignmentViewModel());
        }

        public async Task<AssignmentViewModel> GetAssignment(string id)
        {
            var assignment = await this.tableClient.GetEntityById(id);

            if (assignment == null)
            {
                throw new NotFoundException("Entity with such id was not found");
            }

            return assignment.GetAssignmentViewModel();
        }

        public async Task<AssignmentViewModel> CreateAssignment(AssignmentViewModel assignmentViewModel)
        {
            var existingAssignment = string.IsNullOrWhiteSpace(assignmentViewModel.Id) 
                ? null 
                : await this.tableClient.GetEntityById(assignmentViewModel.Id);

            if (existingAssignment != null)
            {
                throw new BadRequestException("Assignment this such id already exists");
            }

            var assignment = await this.tableClient.InsertOrMergeEntityAsync(assignmentViewModel.GetAssignment());
            return assignment.GetAssignmentViewModel();
        }

        public async Task<AssignmentViewModel> UpdateAssignment(string id, AssignmentViewModel assignmentViewModel)
        {
            // id in URI and assignment's row key should be the same
            if (!string.IsNullOrWhiteSpace(assignmentViewModel.Id) && id != assignmentViewModel.Id)
            {
                throw new BadRequestException("Cannot update the id field");
            }

            assignmentViewModel.Id = id;
            var assignment = await this.tableClient.InsertOrMergeEntityAsync(assignmentViewModel.GetAssignment());
            return assignment.GetAssignmentViewModel();
        }

        public async Task DeleteAssignment(string id)
        {
            await this.tableClient.DeleteEntityAsync(id);
        }
    }
}
