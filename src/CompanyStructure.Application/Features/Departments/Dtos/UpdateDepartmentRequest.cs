namespace CompanyStructure.Application.Features.Departments.Dtos
{
    public class UpdateDepartmentRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public Guid? ManagerId { get; set; }
    }
}
