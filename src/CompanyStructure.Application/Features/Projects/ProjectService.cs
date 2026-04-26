using CompanyStructure.Application.Exceptions;
using CompanyStructure.Application.Features.Projects.Dtos;
using CompanyStructure.Application.Interfaces.Repositories;
using CompanyStructure.Application.Interfaces;
using CompanyStructure.Domain.Entities;

namespace CompanyStructure.Application.Features.Projects
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IDivisionRepository _divisionRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ProjectService(
            IProjectRepository projectRepository,
            IDivisionRepository divisionRepository,
            IEmployeeRepository employeeRepository,
            IUnitOfWork unitOfWork)
        {
            _projectRepository = projectRepository;
            _divisionRepository = divisionRepository;
            _employeeRepository = employeeRepository;
            _unitOfWork = unitOfWork;
        }

        private static ProjectResponse MapToResponse(Project project)
        {
            return new ProjectResponse
            {
                Id = project.Id,
                Name = project.Name,
                Code = project.Code,
                DivisionId = project.DivisionId,
                ManagerId = project.ManagerId
            };
        }

        public async Task<List<ProjectResponse>> GetAllByDivisionIdAsync(Guid divisionId)
        {
            var division = await _divisionRepository.GetByIdAsync(divisionId)
                ?? throw new NotFoundException(nameof(Division), divisionId);

            var projects = await _projectRepository.GetAllByDivisionIdAsync(divisionId);
            return projects.Select(MapToResponse).ToList();
        }

        public async Task<ProjectResponse> GetByIdAsync(Guid id)
        {
            var project = await _projectRepository.GetByIdAsync(id)
                ?? throw new NotFoundException(nameof(Project), id);

            return MapToResponse(project);
        }

        public async Task<ProjectResponse> CreateAsync(CreateProjectRequest request)
        {
            var division = await _divisionRepository.GetByIdAsync(request.DivisionId)
                ?? throw new NotFoundException(nameof(Division), request.DivisionId);

            if (request.ManagerId.HasValue)
            {
                var manager = await _employeeRepository.GetByIdAsync(request.ManagerId.Value)
                    ?? throw new NotFoundException(nameof(Employee), request.ManagerId.Value);

                if (manager.CompanyId != division.CompanyId)
                {
                    throw new ConflictException(
                        "Manager must be an employee of the same company as the project.");
                }
            }

            var project = new Project
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Code = request.Code,
                DivisionId = request.DivisionId,
                ManagerId = request.ManagerId
            };

            _projectRepository.Create(project);
            await _unitOfWork.SaveChangesAsync();

            return MapToResponse(project);
        }

        public async Task UpdateAsync(Guid id, UpdateProjectRequest request)
        {
            var project = await _projectRepository.GetByIdAsync(id)
                ?? throw new NotFoundException(nameof(Project), id);

            if (request.ManagerId.HasValue)
            {
                var manager = await _employeeRepository.GetByIdAsync(request.ManagerId.Value)
                    ?? throw new NotFoundException(nameof(Employee), request.ManagerId.Value);

                var division = await _divisionRepository.GetByIdAsync(project.DivisionId)
                    ?? throw new NotFoundException(nameof(Division), project.DivisionId);

                if (manager.CompanyId != division.CompanyId)
                {
                    throw new ConflictException(
                        "Manager must be an employee of the same company as the project.");
                }
            }

            project.Name = request.Name;
            project.Code = request.Code;
            project.ManagerId = request.ManagerId;

            _projectRepository.Update(project);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var project = await _projectRepository.GetByIdAsync(id)
                ?? throw new NotFoundException(nameof(Project), id);

            if (await _projectRepository.HasDepartmentsAsync(id))
            {
                throw new ConflictException(
                    "Cannot delete project with existing departments.");
            }

            _projectRepository.Delete(project);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
