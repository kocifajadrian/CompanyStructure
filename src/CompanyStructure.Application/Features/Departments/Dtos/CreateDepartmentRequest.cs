namespace CompanyStructure.Application.Features.Departments.Dtos
{
    public class CreateDepartmentRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public Guid ProjectId { get; set; }
        public Guid? ManagerId { get; set; }
    }
}
