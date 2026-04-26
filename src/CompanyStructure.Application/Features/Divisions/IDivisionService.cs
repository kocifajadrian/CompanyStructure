using CompanyStructure.Application.Features.Divisions.Dtos;

namespace CompanyStructure.Application.Features.Divisions
{
    public interface IDivisionService
    {
        Task<List<DivisionResponse>> GetAllByCompanyIdAsync(Guid companyId);
        Task<DivisionResponse> GetByIdAsync(Guid id);
        Task<DivisionResponse> CreateAsync(CreateDivisionRequest request);
        Task UpdateAsync(Guid id, UpdateDivisionRequest request);
        Task DeleteAsync(Guid id);
    }
}
