using CompanyStructure.Application.Exceptions;
using CompanyStructure.Application.Features.Companies.Dtos;
using CompanyStructure.Application.Interfaces.Repositories;
using CompanyStructure.Application.Interfaces;
using CompanyStructure.Domain.Entities;

namespace CompanyStructure.Application.Features.Companies
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CompanyService(
            ICompanyRepository companyRepository,
            IEmployeeRepository employeeRepository,
            IUnitOfWork unitOfWork)
        {
            _companyRepository = companyRepository;
            _employeeRepository = employeeRepository;
            _unitOfWork = unitOfWork;
        }

        private static CompanyResponse MapToResponse(Company company)
        {
            return new CompanyResponse
            {
                Id = company.Id,
                Name = company.Name,
                Code = company.Code,
                ManagerId = company.ManagerId
            };
        }

        public async Task<List<CompanyResponse>> GetAllAsync()
        {
            var companies = await _companyRepository.GetAllAsync();
            return companies.Select(MapToResponse).ToList();
        }

        public async Task<CompanyResponse> GetByIdAsync(Guid id)
        {
            var company = await _companyRepository.GetByIdAsync(id)
                ?? throw new NotFoundException(nameof(Company), id);

            return MapToResponse(company);
        }

        public async Task<CompanyResponse> CreateAsync(CreateCompanyRequest request)
        {
            if (request.ManagerId.HasValue)
            {
                var manager = await _employeeRepository.GetByIdAsync(request.ManagerId.Value)
                    ?? throw new NotFoundException(nameof(Employee), request.ManagerId.Value);
            }

            var company = new Company
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Code = request.Code,
                ManagerId = request.ManagerId
            };

            _companyRepository.Create(company);
            await _unitOfWork.SaveChangesAsync();

            return MapToResponse(company);
        }

        public async Task UpdateAsync(Guid id, UpdateCompanyRequest request)
        {
            var company = await _companyRepository.GetByIdAsync(id)
                ?? throw new NotFoundException(nameof(Company), id);

            if (request.ManagerId.HasValue)
            {
                var manager = await _employeeRepository.GetByIdAsync(request.ManagerId.Value)
                    ?? throw new NotFoundException(nameof(Employee), request.ManagerId.Value);

                if (manager.CompanyId != id)
                {
                    throw new ConflictException(
                        "Manager must be an employee of this company.");
                }
            }

            company.Name = request.Name;
            company.Code = request.Code;
            company.ManagerId = request.ManagerId;

            _companyRepository.Update(company);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var company = await _companyRepository.GetByIdAsync(id)
                ?? throw new NotFoundException(nameof(Company), id);

            if (await _companyRepository.HasDivisionsAsync(id))
            {
                throw new ConflictException(
                    "Cannot delete company with existing divisions.");
            }

            if (await _companyRepository.HasEmployeesAsync(id))
            {
                throw new ConflictException(
                    "Cannot delete company with existing employees.");
            }

            _companyRepository.Delete(company);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
