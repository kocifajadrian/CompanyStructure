using CompanyStructure.Application.Features.Projects.Dtos;

namespace CompanyStructure.Application.Features.Projects
{
    public interface IProjectService
    {
        Task<List<ProjectResponse>> GetAllByDivisionIdAsync(Guid divisionId);
        Task<ProjectResponse> GetByIdAsync(Guid id);
        Task<ProjectResponse> CreateAsync(CreateProjectRequest request);
        Task UpdateAsync(Guid id, UpdateProjectRequest request);
        Task DeleteAsync(Guid id);
    }
}
