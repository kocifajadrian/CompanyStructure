namespace CompanyStructure.Application.Features.Projects.Dtos
{
    public class ProjectResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public Guid DivisionId { get; set; }
        public Guid? ManagerId { get; set; }
    }
}
