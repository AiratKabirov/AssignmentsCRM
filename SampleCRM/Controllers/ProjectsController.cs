using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SampleCRM.Services;
using SampleCRM.ViewModels;

namespace SampleCRM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProjectsController : ControllerBase
    {
        private IDataService<ProjectViewModel> dataService { get; }

        public ProjectsController(IDataService<ProjectViewModel> dataService)
        {
            this.dataService = dataService;
        }

        /// <summary>
        /// Gets all projects
        /// </summary>
        /// <returns>List of projects</returns>
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProjectViewModel>>> Get()
        {
            return Ok(await dataService.ListEntities());
        }
    }
}
