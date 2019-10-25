using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SampleCRM.Models;
using SampleCRM.Services;
using SampleCRM.ViewModels;

namespace SampleCRM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssignmentsController : ControllerBase
    {
        private IAssignmentsService dataService { get; }

        public AssignmentsController(IAssignmentsService dataService)
        {
            this.dataService = dataService;
        }

        /// <summary>
        /// Gets all assignments
        /// </summary>
        /// <returns>List of assignments</returns>
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IReadOnlyList<AssignmentViewModel>>> Get()
        {
            return Ok(await dataService.ListAssignments());
        }

        /// <summary>
        /// Gets an assignment with id specified
        /// </summary>
        /// <param name="id">Id of the assignemnt to modify</param>
        /// <returns>Assignment</returns>
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<AssignmentViewModel>> Get(string id)
        {
            var assignment = await dataService.GetAssignment(id);
            return Ok(assignment);
        }

        /// <summary>
        /// Creates assignment
        /// </summary>
        /// <param name="assignment">Assignment to create</param>
        /// <returns>Assignment</returns>
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<AssignmentViewModel>> Post([FromBody] AssignmentViewModel assignment)
        {
            var result = await dataService.CreateAssignment(assignment);
            return Created(this.Request.Path + $"/{result.Id}", result);
        }

        /// <summary>
        /// Modifies assignment with specified id
        /// </summary>
        /// <param name="id">Id of the assignemnt to modify</param>
        /// <param name="assignment">Assignment with modified properties</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult> Put(string id, [FromBody] AssignmentViewModel assignment)
        {
            var result = await dataService.UpdateAssignment(id, assignment);
            return Ok(result);

        }

        /// <summary>
        /// Deletes assignment with specified id
        /// </summary>
        /// <param name="id">Id of the assignment to delete</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult> Delete(string id)
        {
            await dataService.DeleteAssignment(id);

            return NoContent();
        }
    }
}
