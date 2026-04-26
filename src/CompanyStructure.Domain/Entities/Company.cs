namespace CompanyStructure.Domain.Entities
{
    public class Company : StructureNode
    {
        public ICollection<Division> Divisions { get; set; } = [];
        public ICollection<Employee> Employees { get; set; } = [];
    }
}
