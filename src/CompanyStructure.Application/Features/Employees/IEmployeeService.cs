using CompanyStructure.Application.Features.Employees.Dtos;

namespace CompanyStructure.Application.Features.Employees
{
    public interface IEmployeeService
    {
        Task<List<EmployeeResponse>> GetAllByCompanyIdAsync(Guid companyId);
        Task<EmployeeResponse> GetByIdAsync(Guid id);
        Task<EmployeeResponse> CreateAsync(CreateEmployeeRequest request);
        Task UpdateAsync(Guid id, UpdateEmployeeRequest request);
        Task DeleteAsync(Guid id);
    }
}
