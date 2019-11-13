using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using SampleCRM.Utilities;
using SampleCRM.ViewModels;
using SampleCRM.Models;
using System;

namespace SampleCRM.Services
{
    public class AssignmentsService : IDataService<AssignmentViewModel>
    {
        ITableClient tableClient;
        private readonly string assignmentsTableName = "Assignments";
        private readonly string projectsTableName = "Projects";
        private readonly string defaultProjectId = "Default_a83122c3-74e4-4005-bd0a-29064625e15c";

        public AssignmentsService(ITableClient tableClient)
        {
            this.tableClient = tableClient;
        }

        public async Task<IEnumerable<AssignmentViewModel>> ListEntities()
        {
            var assignments = await tableClient.ListAllEntities<Assignment>(assignmentsTableName);
            return assignments.Select(assignment => assignment.GetAssignmentViewModel());
        }

        public async Task<AssignmentViewModel> GetEntity(string outerId, string innerId)
        {
            var assignment = await this.tableClient.GetEntityById<Assignment>(assignmentsTableName, outerId, innerId);

            if (assignment == null)
            {
                throw new NotFoundException("Entity with such id was not found");
            }

            return assignment.GetAssignmentViewModel();
        }

        public async Task<AssignmentViewModel> CreateEntity(AssignmentViewModel assignmentViewModel)
        {
            if (string.IsNullOrWhiteSpace(assignmentViewModel.ProjectId))
            {
                assignmentViewModel.ProjectId = defaultProjectId;
            }
            else
            {
                await CheckIfSuchProjectExists(assignmentViewModel.ProjectId);
            }

            var existingAssignment = string.IsNullOrWhiteSpace(assignmentViewModel.Id) 
                ? null 
                : await this.tableClient.GetEntityById<Assignment>(assignmentsTableName, assignmentViewModel.ProjectId, assignmentViewModel.Id);

            if (existingAssignment != null)
            {
                throw new BadRequestException("Assignment this such id already exists");
            }

            if (string.IsNullOrWhiteSpace(assignmentViewModel.Id))
            {
                assignmentViewModel.Id = Guid.NewGuid().ToString();
            }

            var assignment = await this.tableClient.InsertOrMergeEntityAsync(assignmentsTableName, assignmentViewModel.GetAssignment());
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

            await CheckIfSuchProjectExists(assignmentViewModel.ProjectId);

            assignmentViewModel.Id = innerId;
            var assignment = await this.tableClient.InsertOrMergeEntityAsync(assignmentsTableName, assignmentViewModel.GetAssignment());
            return assignment.GetAssignmentViewModel();
        }

        public async Task DeleteEntity(string outerId, string innerId)
        {
            await this.tableClient.DeleteEntityAsync<Assignment>(assignmentsTableName, outerId, innerId);
        }

        private async Task CheckIfSuchProjectExists(string projectId)
        {
            var projectPartitionKey = string.Concat(projectId.TakeWhile(c => c != '_'));
            var project = await this.tableClient.GetEntityById<Project>(projectsTableName, projectPartitionKey, projectId);
            
            if (project == null)
            {
                throw new NotFoundException("Project does not exist");
            }
        }
    }
}
