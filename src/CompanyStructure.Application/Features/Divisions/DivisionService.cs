using CompanyStructure.Application.Exceptions;
using CompanyStructure.Application.Features.Divisions.Dtos;
using CompanyStructure.Application.Interfaces.Repositories;
using CompanyStructure.Application.Interfaces;
using CompanyStructure.Domain.Entities;

namespace CompanyStructure.Application.Features.Divisions
{
    public class DivisionService : IDivisionService
    {
        private readonly IDivisionRepository _divisionRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DivisionService(
            IDivisionRepository divisionRepository,
            ICompanyRepository companyRepository,
            IEmployeeRepository employeeRepository,
            IUnitOfWork unitOfWork)
        {
            _divisionRepository = divisionRepository;
            _companyRepository = companyRepository;
            _employeeRepository = employeeRepository;
            _unitOfWork = unitOfWork;
        }

        private static DivisionResponse MapToResponse(Division division)
        {
            return new DivisionResponse
            {
                Id = division.Id,
                Name = division.Name,
                Code = division.Code,
                CompanyId = division.CompanyId,
                ManagerId = division.ManagerId
            };
        }

        public async Task<List<DivisionResponse>> GetAllByCompanyIdAsync(Guid companyId)
        {
            var company = await _companyRepository.GetByIdAsync(companyId)
                ?? throw new NotFoundException(nameof(Company), companyId);

            var divisions = await _divisionRepository.GetAllByCompanyIdAsync(companyId);
            return divisions.Select(MapToResponse).ToList();
        }

        public async Task<DivisionResponse> GetByIdAsync(Guid id)
        {
            var division = await _divisionRepository.GetByIdAsync(id)
                ?? throw new NotFoundException(nameof(Division), id);

            return MapToResponse(division);
        }

        public async Task<DivisionResponse> CreateAsync(CreateDivisionRequest request)
        {
            var company = await _companyRepository.GetByIdAsync(request.CompanyId)
                ?? throw new NotFoundException(nameof(Company), request.CompanyId);

            if (request.ManagerId.HasValue)
            {
                var manager = await _employeeRepository.GetByIdAsync(request.ManagerId.Value)
                    ?? throw new NotFoundException(nameof(Employee), request.ManagerId.Value);

                if (manager.CompanyId != request.CompanyId)
                {
                    throw new ConflictException(
                        "Manager must be an employee of the same company as the division.");
                }
            }

            var division = new Division
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Code = request.Code,
                CompanyId = request.CompanyId,
                ManagerId = request.ManagerId
            };

            _divisionRepository.Create(division);
            await _unitOfWork.SaveChangesAsync();

            return MapToResponse(division);
        }

        public async Task UpdateAsync(Guid id, UpdateDivisionRequest request)
        {
            var division = await _divisionRepository.GetByIdAsync(id)
                ?? throw new NotFoundException(nameof(Division), id);

            if (request.ManagerId.HasValue)
            {
                var manager = await _employeeRepository.GetByIdAsync(request.ManagerId.Value)
                    ?? throw new NotFoundException(nameof(Employee), request.ManagerId.Value);

                if (manager.CompanyId != division.CompanyId)
                {
                    throw new ConflictException(
                        "Manager must be an employee of the same company as the division.");
                }
            }

            division.Name = request.Name;
            division.Code = request.Code;
            division.ManagerId = request.ManagerId;

            _divisionRepository.Update(division);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var division = await _divisionRepository.GetByIdAsync(id)
                ?? throw new NotFoundException(nameof(Division), id);

            if (await _divisionRepository.HasProjectsAsync(id))
            {
                throw new ConflictException(
                    "Cannot delete division with existing projects.");
            }

            _divisionRepository.Delete(division);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
