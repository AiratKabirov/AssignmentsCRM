using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SampleCRM.Models;
using SampleCRM.Services;

namespace SampleCRM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssignmentsController : ControllerBase
    {
        private IAssignmentsService dataProvider { get; }

        public AssignmentsController(IAssignmentsService dataProvider)
        {
            this.dataProvider = dataProvider;
        }

        /// <summary>
        /// Gets all assignments
        /// </summary>
        /// <returns>List of assignments</returns>
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IReadOnlyList<Assignment>>> Get()
        {
            return Ok(await dataProvider.ListAssignments());
        }

        /// <summary>
        /// Gets an assignment with id specified
        /// </summary>
        /// <param name="id">Id of the assignemnt to modify</param>
        /// <returns>Assignment</returns>
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Assignment>> Get(string id)
        {
            return Ok(await dataProvider.GetAssignment(id));
        }

        /// <summary>
        /// Creates assignment
        /// </summary>
        /// <param name="assignment">Assignment to create</param>
        /// <returns>Assignment</returns>
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Assignment>> Post([FromBody] Assignment assignment)
        {
            var result = await dataProvider.CreateOrUpdateAssignment(assignment);
            return Created(this.Request.Path + $"/{result.RowKey}", result);
        }

        /// <summary>
        /// Modifies assignment with specified id
        /// </summary>
        /// <param name="id">Id of the assignemnt to modify</param>
        /// <param name="assignment">Assignment with modified properties</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult> Put(string id, [FromBody] Assignment assignment)
        {
            if (!string.IsNullOrWhiteSpace(assignment.RowKey) && id != assignment.RowKey)
            {
                throw new BadRequestException("Cannot update the id field");
            }

            assignment.RowKey = id;
            var result = await dataProvider.CreateOrUpdateAssignment(assignment);
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
            await dataProvider.DeleteAssignment(id);

            return NoContent();
        }
    }
}
