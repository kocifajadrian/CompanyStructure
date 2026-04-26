using CompanyStructure.Application.Features.Departments.Dtos;

namespace CompanyStructure.Application.Features.Departments
{
    public interface IDepartmentService
    {
        Task<List<DepartmentResponse>> GetAllByProjectIdAsync(Guid projectId);
        Task<DepartmentResponse> GetByIdAsync(Guid id);
        Task<DepartmentResponse> CreateAsync(CreateDepartmentRequest request);
        Task UpdateAsync(Guid id, UpdateDepartmentRequest request);
        Task DeleteAsync(Guid id);
    }
}
