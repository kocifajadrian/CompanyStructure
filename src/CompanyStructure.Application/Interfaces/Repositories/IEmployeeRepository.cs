using CompanyStructure.Domain.Entities;

namespace CompanyStructure.Application.Interfaces.Repositories
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
        Task<List<Employee>> GetAllByCompanyIdAsync(Guid companyId);
    }
}
