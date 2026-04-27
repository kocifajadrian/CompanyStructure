namespace CompanyStructure.Application.Features.Divisions.Dtos
{
    public class UpdateDivisionRequest
    {
        /// <summary>
        /// The name of the division.
        /// </summary>
        /// <example>Software Development</example>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// The code of the division.
        /// </summary>
        /// <example>SWDEV</example>
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// The identifier of the division's manager.
        /// </summary>
        public Guid? ManagerId { get; set; }
    }
}
