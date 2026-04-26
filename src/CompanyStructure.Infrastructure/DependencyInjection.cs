using CompanyStructure.Application.Interfaces;
using CompanyStructure.Application.Interfaces.Repositories;
using CompanyStructure.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CompanyStructure.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var dbConnection = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Database connection not found.");

            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(dbConnection));

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<IDivisionRepository, DivisionRepository>();
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();

            return services;
        }
    }
}
