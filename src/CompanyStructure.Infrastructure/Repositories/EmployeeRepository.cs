using CompanyStructure.Application.Interfaces.Repositories;
using CompanyStructure.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CompanyStructure.Infrastructure.Repositories
{
    public class EmployeeRepository : Repository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(ApplicationDbContext db) : base(db)
        {
        }

        public async Task<List<Employee>> GetAllByCompanyIdAsync(Guid companyId)
        {
            return await _db.Employees
                .Where(e => e.CompanyId == companyId)
                .ToListAsync();
        }
    }
}
