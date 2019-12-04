using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SampleCRM.Services;
using SampleCRM.Utilities;
using SampleCRM.ViewModels;

namespace SampleCRM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AssignmentsController : ControllerBase
    {
        private IDataService<AssignmentViewModel> dataService { get; }

        public AssignmentsController(IDataService<AssignmentViewModel> dataService)
        {
            this.dataService = dataService;
        }

        /// <summary>
        /// Gets all assignments
        /// </summary>
        /// <returns>List of assignments</returns>
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<AssignmentViewModel>>> Get()
        {
            return Ok(await dataService.ListEntities());
        }

        /// <summary>
        /// Gets an assignment with id specified
        /// </summary>
        /// <param name="projectId">Id of the project assignment belongs to</param>
        /// <param name="assignmentId">Id of the assignment</param>
        /// <returns>Assignment</returns>
        [HttpGet("{projectId}/{assignmentId}")]
        public async Task<ActionResult<AssignmentViewModel>> Get(string projectId, string assignmentId)
        {
            var assignment = await dataService.GetEntity(projectId, assignmentId);
            return Ok(assignment);
        }

        /// <summary>
        /// Creates assignment
        /// </summary>
        /// <param name="assignment">Assignment to create</param>
        /// <param name="projectId">Id of the project assignment belongs to</param>
        /// <returns>Assignment</returns>
        [HttpPost("{projectId}")]
        public async Task<ActionResult<AssignmentViewModel>> Post(string projectId, [FromBody] AssignmentForUpsertViewModel assignment)
        {
            var assignmentToCreate = assignment.GetAssignmentViewModel();
            assignmentToCreate.ProjectId = projectId;
            var result = await dataService.CreateEntity(assignmentToCreate);
            return Created((this.Request?.Path ?? string.Empty) + $"/{result.Id}", result);
        }

        /// <summary>
        /// Modifies assignment with specified id
        /// </summary>
        /// <param name="projectId">Id of the project assignment belongs to</param>
        /// <param name="assignmentId">Id of the assignment</param>
        /// <param name="assignment">Assignment with modified properties</param>
        /// <returns></returns>
        [HttpPatch("{projectId}/{assignmentId}")]
        public async Task<ActionResult<AssignmentViewModel>> Patch(string projectId, string assignmentId, [FromBody] AssignmentForUpsertViewModel assignment)
        {
            var assignmentToUpdate = assignment.GetAssignmentViewModel();
            assignmentToUpdate.ProjectId = projectId;
            assignmentToUpdate.Id = assignmentId;
            var result = await dataService.UpdateEntity(projectId, assignmentId, assignmentToUpdate);
            return Ok(result);

        }

        /// <summary>
        /// Deletes assignment with specified id
        /// </summary>
        /// <param name="projectId">Id of the project assignment belongs to</param>
        /// <param name="assignmentId">Id of the assignment</param>
        /// <returns></returns>
        [HttpDelete("{projectId}/{assignmentId}")]
        public async Task<ActionResult> Delete(string projectId, string assignmentId)
        {
            await dataService.DeleteEntity(projectId, assignmentId);
            return NoContent();
        }
    }
}
