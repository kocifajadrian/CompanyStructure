using CompanyStructure.Application.Interfaces.Repositories;
using CompanyStructure.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CompanyStructure.Infrastructure.Repositories
{
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        public CompanyRepository(ApplicationDbContext db) : base(db)
        {
        }

        public async Task<List<Company>> GetAllAsync()
        {
            return await _db.Companies.ToListAsync();
        }

        public async Task<bool> HasDivisionsAsync(Guid id)
        {
            return await _db.Divisions.AnyAsync(d => d.CompanyId == id);
        }

        public async Task<bool> HasEmployeesAsync(Guid id)
        {
            return await _db.Employees.AnyAsync(e => e.CompanyId == id);
        }
    }
}
