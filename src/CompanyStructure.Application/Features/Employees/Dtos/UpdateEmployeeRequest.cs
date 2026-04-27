namespace CompanyStructure.Application.Features.Employees.Dtos
{
    public class UpdateEmployeeRequest
    {
        /// <summary>
        /// The title of the employee.
        /// </summary>
        /// <example>Ing.</example>
        public string? Title { get; set; }

        /// <summary>
        /// The first name of the employee.
        /// </summary>
        /// <example>John</example>
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// The last name of the employee.
        /// </summary>
        /// <example>Doe</example>
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// The phone number of the employee.
        /// </summary>
        /// <example>+421 123 456 789</example>
        public string? Phone { get; set; }

        /// <summary>
        /// The email address of the employee.
        /// </summary>
        /// <example>john.doe@example.com</example>
        public string? Email { get; set; }
    }
}
