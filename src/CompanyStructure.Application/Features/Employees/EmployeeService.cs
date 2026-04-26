using CompanyStructure.Application.Exceptions;
using CompanyStructure.Application.Features.Employees.Dtos;
using CompanyStructure.Application.Interfaces.Repositories;
using CompanyStructure.Application.Interfaces;
using CompanyStructure.Domain.Entities;

namespace CompanyStructure.Application.Features.Employees
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IUnitOfWork _unitOfWork;

        public EmployeeService(
            IEmployeeRepository employeeRepository,
            ICompanyRepository companyRepository,
            IUnitOfWork unitOfWork)
        {
            _employeeRepository = employeeRepository;
            _companyRepository = companyRepository;
            _unitOfWork = unitOfWork;
        }

        private static EmployeeResponse MapToResponse(Employee employee)
        {
            return new EmployeeResponse
            {
                Id = employee.Id,
                Title = employee.Title,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Phone = employee.Phone,
                Email = employee.Email,
                CompanyId = employee.CompanyId
            };
        }

        public async Task<List<EmployeeResponse>> GetAllByCompanyIdAsync(Guid companyId)
        {
            var company = await _companyRepository.GetByIdAsync(companyId)
                ?? throw new NotFoundException(nameof(Company), companyId);

            var employees = await _employeeRepository.GetAllByCompanyIdAsync(companyId);
            return employees.Select(MapToResponse).ToList();
        }

        public async Task<EmployeeResponse> GetByIdAsync(Guid id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id)
                ?? throw new NotFoundException(nameof(Employee), id);

            return MapToResponse(employee);
        }

        public async Task<EmployeeResponse> CreateAsync(CreateEmployeeRequest request)
        {
            var company = await _companyRepository.GetByIdAsync(request.CompanyId)
                ?? throw new NotFoundException(nameof(Company), request.CompanyId);

            var employee = new Employee
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Phone = request.Phone,
                Email = request.Email,
                CompanyId = request.CompanyId
            };

            _employeeRepository.Create(employee);
            await _unitOfWork.SaveChangesAsync();

            return MapToResponse(employee);
        }

        public async Task UpdateAsync(Guid id, UpdateEmployeeRequest request)
        {
            var employee = await _employeeRepository.GetByIdAsync(id)
                ?? throw new NotFoundException(nameof(Employee), id);

            employee.Title = request.Title;
            employee.FirstName = request.FirstName;
            employee.LastName = request.LastName;
            employee.Phone = request.Phone;
            employee.Email = request.Email;

            _employeeRepository.Update(employee);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id)
                ?? throw new NotFoundException(nameof(Employee), id);

            _employeeRepository.Delete(employee);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
