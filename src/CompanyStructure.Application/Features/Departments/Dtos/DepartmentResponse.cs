namespace CompanyStructure.Application.Features.Departments.Dtos
{
    public class DepartmentResponse
    {
        /// <summary>
        /// The identifier of the department.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The name of the department.
        /// </summary>
        /// <example>Backend</example>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// The code of the department.
        /// </summary>
        /// <example>BE</example>
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// The identifier of the project the department belongs to.
        /// </summary>
        /// <example>3fa85f64-5717-4562-b3fc-2c963f66afa6</example>
        public Guid ProjectId { get; set; }

        /// <summary>
        /// The identifier of the department's manager.
        /// </summary>
        public Guid? ManagerId { get; set; }
    }
}
