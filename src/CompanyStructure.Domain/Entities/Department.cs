namespace CompanyStructure.Domain.Entities
{
    public class Department : StructureNode
    {
        public Guid ProjectId { get; set; }
        public Project? Project { get; set; }
    }
}
