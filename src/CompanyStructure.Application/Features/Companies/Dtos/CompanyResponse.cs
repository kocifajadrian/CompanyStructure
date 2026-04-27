namespace CompanyStructure.Application.Features.Companies.Dtos
{
    public class CompanyResponse
    {
        /// <summary>
        /// The identifier of the company.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The name of the company.
        /// </summary>
        /// <example>Software Company</example>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// The code of the company.
        /// </summary>
        /// <example>SWCOMP</example>
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// The identifier of the company's manager.
        /// </summary>
        public Guid? ManagerId { get; set; }
    }
}
