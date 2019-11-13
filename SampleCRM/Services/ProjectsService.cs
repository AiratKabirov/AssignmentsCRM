using SampleCRM.Models;
using SampleCRM.Utilities;
using SampleCRM.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleCRM.Services
{
    public class ProjectsService : IDataService<ProjectViewModel>
    {
        ITableClient tableClient;
        private readonly string tableName = "Projects";

        public ProjectsService(ITableClient tableClient)
        {
            this.tableClient = tableClient;
        }

        public async Task<IEnumerable<ProjectViewModel>> ListEntities()
        {
            var projects = await tableClient.ListAllEntities<Project>(tableName);
            return projects.Select(project => project.GetProjectViewModel());
        }

        public async Task<ProjectViewModel> GetEntity(string outerId, string innerId)
        {
            throw new NotImplementedException();
        }

        public async Task<ProjectViewModel> CreateEntity(ProjectViewModel projectViewModel)
        {
            throw new NotImplementedException();
        }

        public async Task<ProjectViewModel> UpdateEntity(string outerId, string innerId, ProjectViewModel assignmentViewModel)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteEntity(string outerId, string innerId)
        {
            throw new NotImplementedException();
        }
    }
}
