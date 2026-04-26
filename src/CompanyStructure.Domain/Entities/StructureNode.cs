namespace CompanyStructure.Domain.Entities
{
    public abstract class StructureNode
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public Guid? ManagerId { get; set; }
        public Employee? Manager { get; set; }
    }
}
