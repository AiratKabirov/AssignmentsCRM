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
                RowKey = assignmentViewModel?.Id,
                Name = assignmentViewModel?.Name,
                Description = assignmentViewModel?.Description
            };
        }

        public static AssignmentViewModel GetAssignmentViewModel(this Assignment assignment)
        {
            return new AssignmentViewModel
            {
                Id = assignment?.RowKey,
                Name = assignment?.Name,
                Description = assignment?.Description
            };
        }
    }
}
