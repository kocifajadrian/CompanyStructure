using CompanyStructure.Domain.Entities;

namespace CompanyStructure.Application.Interfaces.Repositories
{
    public interface IDivisionRepository : IRepository<Division>
    {
        Task<List<Division>> GetAllByCompanyIdAsync(Guid companyId);
        Task<bool> HasProjectsAsync(Guid id);
    }
}
