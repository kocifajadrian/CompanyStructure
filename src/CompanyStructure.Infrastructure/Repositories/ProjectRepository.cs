using CompanyStructure.Application.Interfaces.Repositories;
using CompanyStructure.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CompanyStructure.Infrastructure.Repositories
{
    public class ProjectRepository : Repository<Project>, IProjectRepository
    {
        public ProjectRepository(ApplicationDbContext db) : base(db)
        {
        }

        public async Task<List<Project>> GetAllByDivisionIdAsync(Guid divisionId)
        {
            return await _db.Projects
                .Where(p => p.DivisionId == divisionId)
                .ToListAsync();
        }

        public async Task<bool> HasDepartmentsAsync(Guid id)
        {
            return await _db.Departments.AnyAsync(d => d.ProjectId == id);
        }
    }
}
