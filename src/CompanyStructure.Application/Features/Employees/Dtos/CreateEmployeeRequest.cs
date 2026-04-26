namespace CompanyStructure.Application.Features.Employees.Dtos
{
    public class CreateEmployeeRequest
    {
        public string? Title { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public Guid CompanyId { get; set; }
    }
}
