using Microsoft.Azure.Cosmos.Table;
using Newtonsoft.Json;
using System;

namespace SampleCRM.Models
{
    public class Assignment : TableEntity
    {
        public Assignment()
        {
        }

        /// <summary>
        /// Assignment name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Assignment description
        /// </summary>
        public string Description { get; set; }
    }
}
