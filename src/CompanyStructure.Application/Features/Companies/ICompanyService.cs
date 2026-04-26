using CompanyStructure.Application.Features.Companies.Dtos;

namespace CompanyStructure.Application.Features.Companies
{
    public interface ICompanyService
    {
        Task<List<CompanyResponse>> GetAllAsync();
        Task<CompanyResponse> GetByIdAsync(Guid id);
        Task<CompanyResponse> CreateAsync(CreateCompanyRequest request);
        Task UpdateAsync(Guid id, UpdateCompanyRequest request);
        Task DeleteAsync(Guid id);
    }
}
