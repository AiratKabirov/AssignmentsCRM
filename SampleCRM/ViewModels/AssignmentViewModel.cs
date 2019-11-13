namespace SampleCRM.ViewModels
{
    public class AssignmentViewModel
    {
        /// <summary>
        /// Id of the project assignment belongs to
        /// </summary>
        public string ProjectId { get; set; }

        /// <summary>
        /// Assignment id
        /// </summary>
        public string Id { get; set; }

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
