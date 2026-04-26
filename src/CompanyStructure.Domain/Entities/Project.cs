namespace CompanyStructure.Domain.Entities
{
    public class Project : StructureNode
    {
        public Guid DivisionId { get; set; }
        public Division? Division { get; set; }
        public ICollection<Department> Departments { get; set; } = [];
    }
}
