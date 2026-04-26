using CompanyStructure.Domain.Entities;

namespace CompanyStructure.Application.Interfaces.Repositories
{
    public interface IProjectRepository : IRepository<Project>
    {
        Task<List<Project>> GetAllByDivisionIdAsync(Guid divisionId);
        Task<bool> HasDepartmentsAsync(Guid id);
    }
}
