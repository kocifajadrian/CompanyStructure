using CompanyStructure.Application.Features.Companies;
using CompanyStructure.Application.Features.Departments;
using CompanyStructure.Application.Features.Divisions;
using CompanyStructure.Application.Features.Employees;
using CompanyStructure.Application.Features.Projects;
using Microsoft.Extensions.DependencyInjection;

namespace CompanyStructure.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<IDivisionService, DivisionService>();
            services.AddScoped<IProjectService, ProjectService>();
            services.AddScoped<IDepartmentService, DepartmentService>();
            services.AddScoped<IEmployeeService, EmployeeService>();

            return services;
        }
    }
}
