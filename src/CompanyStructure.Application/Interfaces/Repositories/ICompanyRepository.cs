using CompanyStructure.Domain.Entities;

namespace CompanyStructure.Application.Interfaces.Repositories
{
    public interface ICompanyRepository : IRepository<Company>
    {
        Task<List<Company>> GetAllAsync();
        Task<bool> HasDivisionsAsync(Guid id);
        Task<bool> HasEmployeesAsync(Guid id);
    }
}
