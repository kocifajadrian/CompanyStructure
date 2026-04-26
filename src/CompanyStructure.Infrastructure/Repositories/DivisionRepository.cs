using CompanyStructure.Application.Interfaces.Repositories;
using CompanyStructure.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CompanyStructure.Infrastructure.Repositories
{
    public class DivisionRepository : Repository<Division>, IDivisionRepository
    {
        public DivisionRepository(ApplicationDbContext db) : base(db)
        {
        }

        public async Task<List<Division>> GetAllByCompanyIdAsync(Guid companyId)
        {
            return await _db.Divisions
                .Where(d => d.CompanyId == companyId)
                .ToListAsync();
        }

        public async Task<bool> HasProjectsAsync(Guid id)
        {
            return await _db.Projects.AnyAsync(p => p.DivisionId == id);
        }
    }
}
