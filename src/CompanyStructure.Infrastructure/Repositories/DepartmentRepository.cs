using CompanyStructure.Application.Interfaces.Repositories;
using CompanyStructure.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CompanyStructure.Infrastructure.Repositories
{
    public class DepartmentRepository : Repository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(ApplicationDbContext db) : base(db)
        {
        }

        public async Task<List<Department>> GetAllByProjectIdAsync(Guid projectId)
        {
            return await _db.Departments
                .Where(d => d.ProjectId == projectId)
                .ToListAsync();
        }
    }
}
