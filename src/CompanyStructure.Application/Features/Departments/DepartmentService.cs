using CompanyStructure.Application.Exceptions;
using CompanyStructure.Application.Features.Departments.Dtos;
using CompanyStructure.Application.Interfaces.Repositories;
using CompanyStructure.Application.Interfaces;
using CompanyStructure.Domain.Entities;

namespace CompanyStructure.Application.Features.Departments
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IDivisionRepository _divisionRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DepartmentService(
            IDepartmentRepository departmentRepository,
            IProjectRepository projectRepository,
            IDivisionRepository divisionRepository,
            IEmployeeRepository employeeRepository,
            IUnitOfWork unitOfWork)
        {
            _departmentRepository = departmentRepository;
            _projectRepository = projectRepository;
            _divisionRepository = divisionRepository;
            _employeeRepository = employeeRepository;
            _unitOfWork = unitOfWork;
        }

        private static DepartmentResponse MapToResponse(Department department)
        {
            return new DepartmentResponse
            {
                Id = department.Id,
                Name = department.Name,
                Code = department.Code,
                ProjectId = department.ProjectId,
                ManagerId = department.ManagerId
            };
        }

        public async Task<List<DepartmentResponse>> GetAllByProjectIdAsync(Guid projectId)
        {
            var project = await _projectRepository.GetByIdAsync(projectId)
                ?? throw new NotFoundException(nameof(Project), projectId);

            var departments = await _departmentRepository.GetAllByProjectIdAsync(projectId);
            return departments.Select(MapToResponse).ToList();
        }

        public async Task<DepartmentResponse> GetByIdAsync(Guid id)
        {
            var department = await _departmentRepository.GetByIdAsync(id)
                ?? throw new NotFoundException(nameof(Department), id);

            return MapToResponse(department);
        }

        public async Task<DepartmentResponse> CreateAsync(CreateDepartmentRequest request)
        {
            var project = await _projectRepository.GetByIdAsync(request.ProjectId)
                ?? throw new NotFoundException(nameof(Project), request.ProjectId);

            if (request.ManagerId.HasValue)
            {
                var manager = await _employeeRepository.GetByIdAsync(request.ManagerId.Value)
                    ?? throw new NotFoundException(nameof(Employee), request.ManagerId.Value);

                var division = await _divisionRepository.GetByIdAsync(project.DivisionId)
                    ?? throw new NotFoundException(nameof(Division), project.DivisionId);

                if (manager.CompanyId != division.CompanyId)
                {
                    throw new ConflictException(
                        "Manager must be an employee of the same company as the department.");
                }
            }

            var department = new Department
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Code = request.Code,
                ProjectId = request.ProjectId,
                ManagerId = request.ManagerId
            };

            _departmentRepository.Create(department);
            await _unitOfWork.SaveChangesAsync();

            return MapToResponse(department);
        }

        public async Task UpdateAsync(Guid id, UpdateDepartmentRequest request)
        {
            var department = await _departmentRepository.GetByIdAsync(id)
                ?? throw new NotFoundException(nameof(Department), id);

            if (request.ManagerId.HasValue)
            {
                var manager = await _employeeRepository.GetByIdAsync(request.ManagerId.Value)
                    ?? throw new NotFoundException(nameof(Employee), request.ManagerId.Value);

                var project = await _projectRepository.GetByIdAsync(department.ProjectId)
                    ?? throw new NotFoundException(nameof(Project), department.ProjectId);

                var division = await _divisionRepository.GetByIdAsync(project.DivisionId)
                    ?? throw new NotFoundException(nameof(Division), project.DivisionId);

                if (manager.CompanyId != division.CompanyId)
                {
                    throw new ConflictException(
                        "Manager must be an employee of the same company as the department.");
                }
            }

            department.Name = request.Name;
            department.Code = request.Code;
            department.ManagerId = request.ManagerId;

            _departmentRepository.Update(department);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var department = await _departmentRepository.GetByIdAsync(id)
                ?? throw new NotFoundException(nameof(Department), id);

            _departmentRepository.Delete(department);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
