using Microsoft.Azure.Cosmos.Table;

namespace SampleCRM.Models
{
    public class Project : TableEntity
    {
        public Project()
        {
        }

        /// <summary>
        /// Project name
        /// </summary>
        public string Name { get; set; }
    }
}
