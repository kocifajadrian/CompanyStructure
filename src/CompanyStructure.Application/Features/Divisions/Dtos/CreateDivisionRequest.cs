namespace CompanyStructure.Application.Features.Divisions.Dtos
{
    public class CreateDivisionRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public Guid CompanyId { get; set; }
        public Guid? ManagerId { get; set; }
    }
}
