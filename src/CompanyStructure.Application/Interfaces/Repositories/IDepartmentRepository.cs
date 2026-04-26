using CompanyStructure.Domain.Entities;

namespace CompanyStructure.Application.Interfaces.Repositories
{
    public interface IDepartmentRepository : IRepository<Department>
    {
        Task<List<Department>> GetAllByProjectIdAsync(Guid projectId);
    }
}
