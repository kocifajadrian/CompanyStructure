namespace CompanyStructure.Application.Features.Companies.Dtos
{
    public class CreateCompanyRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public Guid? ManagerId { get; set; }
    }
}
