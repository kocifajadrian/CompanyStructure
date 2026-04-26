namespace CompanyStructure.Domain.Entities
{
    public class Employee
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public Guid CompanyId { get; set; }

        public Company? Company { get; set; }
    }
}
