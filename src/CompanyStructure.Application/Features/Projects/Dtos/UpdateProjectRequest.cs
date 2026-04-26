namespace CompanyStructure.Application.Features.Projects.Dtos
{
    public class UpdateProjectRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public Guid? ManagerId { get; set; }
    }
}
