namespace CompanyStructure.Domain.Entities
{
    public class Division : StructureNode
    {
        public Guid CompanyId { get; set; }
        public Company? Company { get; set; }
        public ICollection<Project> Projects { get; set; } = [];
    }
}
