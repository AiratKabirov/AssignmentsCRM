using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SampleCRM.Models;

namespace SampleCRM.Services
{
    public interface IAssignmentsService
    {
        Task<IReadOnlyList<Assignment>> ListAssignments();

        Task<Assignment> GetAssignment(string id);

        Task<Assignment> CreateAssignment(Assignment assignment);

        Task<Assignment> UpdateAssignment(string id, Assignment assignment);

        Task DeleteAssignment(string id);
    }
}
