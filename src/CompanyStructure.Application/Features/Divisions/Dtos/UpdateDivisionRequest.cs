namespace CompanyStructure.Application.Features.Divisions.Dtos
{
    public class UpdateDivisionRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public Guid? ManagerId { get; set; }
    }
}
