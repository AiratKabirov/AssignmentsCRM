using System.Collections.Generic;
using System.Threading.Tasks;
using SampleCRM.ViewModels;

namespace SampleCRM.Services
{
    public interface IAssignmentsService
    {
        Task<IEnumerable<AssignmentViewModel>> ListAssignments();

        Task<AssignmentViewModel> GetAssignment(string id);

        Task<AssignmentViewModel> CreateAssignment(AssignmentViewModel assignment);

        Task<AssignmentViewModel> UpdateAssignment(string id, AssignmentViewModel assignment);

        Task DeleteAssignment(string id);
    }
}
