namespace CompanyStructure.Application.Features.Projects.Dtos
{
    public class UpdateProjectRequest
    {
        /// <summary>
        /// The name of the project.
        /// </summary>
        /// <example>Application</example>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// The code of the project.
        /// </summary>
        /// <example>APP</example>
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// The identifier of the project's manager.
        /// </summary>
        public Guid? ManagerId { get; set; }
    }
}
