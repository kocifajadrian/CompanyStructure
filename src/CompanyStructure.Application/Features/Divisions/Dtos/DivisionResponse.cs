namespace CompanyStructure.Application.Features.Divisions.Dtos
{
    public class DivisionResponse
    {
        /// <summary>
        /// The identifier of the division.
        /// </summary>
        public Guid Id { get; set; }

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
        /// The identifier of the company the division belongs to.
        /// </summary>
        /// <example>3fa85f64-5717-4562-b3fc-2c963f66afa6</example>
        public Guid CompanyId { get; set; }

        /// <summary>
        /// The identifier of the division's manager.
        /// </summary>
        public Guid? ManagerId { get; set; }
    }
}
