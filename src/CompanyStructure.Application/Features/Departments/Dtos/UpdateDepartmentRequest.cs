namespace CompanyStructure.Application.Features.Departments.Dtos
{
    public class UpdateDepartmentRequest
    {
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
        /// The identifier of the department's manager.
        /// </summary>
        public Guid? ManagerId { get; set; }
    }
}
