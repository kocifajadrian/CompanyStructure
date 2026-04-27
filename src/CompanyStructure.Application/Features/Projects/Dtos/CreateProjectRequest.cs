namespace CompanyStructure.Application.Features.Projects.Dtos
{
    public class CreateProjectRequest
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
        /// The identifier of the division the project belongs to.
        /// </summary>
        /// <example>3fa85f64-5717-4562-b3fc-2c963f66afa6</example>
        public Guid DivisionId { get; set; }

        /// <summary>
        /// The identifier of the project's manager.
        /// </summary>
        public Guid? ManagerId { get; set; }
    }
}
