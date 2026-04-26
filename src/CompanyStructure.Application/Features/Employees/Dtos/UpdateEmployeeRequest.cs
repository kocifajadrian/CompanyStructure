namespace CompanyStructure.Application.Features.Employees.Dtos
{
    public class UpdateEmployeeRequest
    {
        public string? Title { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? Email { get; set; }
    }
}
