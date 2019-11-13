using SampleCRM.Models;
using SampleCRM.ViewModels;

namespace SampleCRM.Utilities
{
    public static class Mapper
    {
        public static Assignment GetAssignment(this AssignmentViewModel assignmentViewModel)
        {
            return new Assignment
            {
                PartitionKey = assignmentViewModel?.ProjectId,
                RowKey = assignmentViewModel?.Id,
                Name = assignmentViewModel?.Name,
                Description = assignmentViewModel?.Description
            };
        }

        public static AssignmentViewModel GetAssignmentViewModel(this Assignment assignment)
        {
            return new AssignmentViewModel
            {
                ProjectId = assignment?.PartitionKey,
                Id = assignment?.RowKey,
                Name = assignment?.Name,
                Description = assignment?.Description
            };
        }

        public static ProjectViewModel GetProjectViewModel(this Project project)
        {
            return new ProjectViewModel
            {
                Code = project?.PartitionKey,
                Id = project?.RowKey,
                Name = project?.Name,
            };
        }
    }
}
