using CompanyStructure.Application.Exceptions;
using CompanyStructure.Application.Features.Projects;
using CompanyStructure.Application.Features.Projects.Dtos;
using CompanyStructure.Application.Interfaces;
using CompanyStructure.Application.Interfaces.Repositories;
using CompanyStructure.Domain.Entities;
using Moq;

namespace CompanyStructure.Application.Tests.ServiceTests
{
    public class ProjectServiceTests
    {
        private readonly Mock<IProjectRepository> _projectRepositoryMock = new();
        private readonly Mock<IDivisionRepository> _divisionRepositoryMock = new();
        private readonly Mock<IEmployeeRepository> _employeeRepositoryMock = new();
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
        private readonly ProjectService _service;

        public ProjectServiceTests()
        {
            _service = new ProjectService(
                _projectRepositoryMock.Object,
                _divisionRepositoryMock.Object,
                _employeeRepositoryMock.Object,
                _unitOfWorkMock.Object);
        }

        [Fact]
        public async Task GetByIdAsync_Should_ReturnProject_When_ProjectExists()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            var project = new Project
            {
                Id = projectId,
                Name = "Application",
                Code = "APP",
                DivisionId = Guid.NewGuid()
            };

            _projectRepositoryMock
                .Setup(r => r.GetByIdAsync(projectId))
                .ReturnsAsync(project);

            // Act
            var result = await _service.GetByIdAsync(projectId);

            // Assert
            Assert.Equal(project.Id, result.Id);
            Assert.Equal(project.Name, result.Name);
            Assert.Equal(project.Code, result.Code);
            Assert.Equal(project.DivisionId, result.DivisionId);
        }

        [Fact]
        public async Task GetByIdAsync_Should_ThrowNotFoundException_When_ProjectDoesNotExist()
        {
            // Arrange
            var projectId = Guid.NewGuid();

            _projectRepositoryMock
                .Setup(r => r.GetByIdAsync(projectId))
                .ReturnsAsync((Project?)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(
                () => _service.GetByIdAsync(projectId));
        }

        [Fact]
        public async Task GetAllByDivisionIdAsync_Should_ReturnAllProjects_When_DivisionExists()
        {
            // Arrange
            var divisionId = Guid.NewGuid();
            var division = new Division
            {
                Id = divisionId,
                Name = "Software Development",
                Code = "SWDEV",
                CompanyId = Guid.NewGuid()
            };
            var projects = new List<Project>
            {
                new() { Id = Guid.NewGuid(), Name = "Application", Code = "APP", DivisionId = divisionId },
                new() { Id = Guid.NewGuid(), Name = "Website", Code = "WEB", DivisionId = divisionId }
            };

            _divisionRepositoryMock
                .Setup(r => r.GetByIdAsync(divisionId))
                .ReturnsAsync(division);
            _projectRepositoryMock
                .Setup(r => r.GetAllByDivisionIdAsync(divisionId))
                .ReturnsAsync(projects);

            // Act
            var result = await _service.GetAllByDivisionIdAsync(divisionId);

            // Assert
            Assert.Equal(projects.Count, result.Count);
            Assert.Equal(projects[0].Name, result[0].Name);
            Assert.Equal(projects[1].Name, result[1].Name);
        }

        [Fact]
        public async Task GetAllByDivisionIdAsync_Should_ThrowNotFoundException_When_DivisionDoesNotExist()
        {
            // Arrange
            var divisionId = Guid.NewGuid();

            _divisionRepositoryMock
                .Setup(r => r.GetByIdAsync(divisionId))
                .ReturnsAsync((Division?)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(
                () => _service.GetAllByDivisionIdAsync(divisionId));
        }

        [Fact]
        public async Task CreateAsync_Should_CreateProject_When_RequestIsValid()
        {
            // Arrange
            var divisionId = Guid.NewGuid();
            var division = new Division
            {
                Id = divisionId,
                Name = "Software Development",
                Code = "SWDEV",
                CompanyId = Guid.NewGuid()
            };

            var request = new CreateProjectRequest
            {
                Name = "Application",
                Code = "APP",
                DivisionId = divisionId
            };

            _divisionRepositoryMock
                .Setup(r => r.GetByIdAsync(divisionId))
                .ReturnsAsync(division);

            // Act
            var result = await _service.CreateAsync(request);

            // Assert
            Assert.Equal(request.Name, result.Name);
            Assert.Equal(request.Code, result.Code);
            Assert.Equal(request.DivisionId, result.DivisionId);
            Assert.NotEqual(Guid.Empty, result.Id);

            _projectRepositoryMock.Verify(r => r.Create(It.IsAny<Project>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_Should_ThrowNotFoundException_When_DivisionDoesNotExist()
        {
            // Arrange
            var request = new CreateProjectRequest
            {
                Name = "Application",
                Code = "APP",
                DivisionId = Guid.NewGuid()
            };

            _divisionRepositoryMock
                .Setup(r => r.GetByIdAsync(request.DivisionId))
                .ReturnsAsync((Division?)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(
                () => _service.CreateAsync(request));

            _projectRepositoryMock.Verify(r => r.Create(It.IsAny<Project>()), Times.Never);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task CreateAsync_Should_ThrowNotFoundException_When_ManagerDoesNotExist()
        {
            // Arrange
            var divisionId = Guid.NewGuid();
            var managerId = Guid.NewGuid();
            var division = new Division
            {
                Id = divisionId,
                Name = "Software Development",
                Code = "SWDEV",
                CompanyId = Guid.NewGuid()
            };

            var request = new CreateProjectRequest
            {
                Name = "Application",
                Code = "APP",
                DivisionId = divisionId,
                ManagerId = managerId
            };

            _divisionRepositoryMock
                .Setup(r => r.GetByIdAsync(divisionId))
                .ReturnsAsync(division);
            _employeeRepositoryMock
                .Setup(r => r.GetByIdAsync(managerId))
                .ReturnsAsync((Employee?)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(
                () => _service.CreateAsync(request));

            _projectRepositoryMock.Verify(r => r.Create(It.IsAny<Project>()), Times.Never);
        }

        [Fact]
        public async Task CreateAsync_Should_ThrowConflictException_When_ManagerIsFromDifferentCompany()
        {
            // Arrange
            var companyId = Guid.NewGuid();
            var differentCompanyId = Guid.NewGuid();
            var divisionId = Guid.NewGuid();
            var managerId = Guid.NewGuid();

            var division = new Division
            {
                Id = divisionId,
                Name = "Software Development",
                Code = "SWDEV",
                CompanyId = companyId
            };

            var manager = new Employee
            {
                Id = managerId,
                FirstName = "John",
                LastName = "Doe",
                CompanyId = differentCompanyId
            };

            var request = new CreateProjectRequest
            {
                Name = "Application",
                Code = "APP",
                DivisionId = divisionId,
                ManagerId = managerId
            };

            _divisionRepositoryMock
                .Setup(r => r.GetByIdAsync(divisionId))
                .ReturnsAsync(division);
            _employeeRepositoryMock
                .Setup(r => r.GetByIdAsync(managerId))
                .ReturnsAsync(manager);

            // Act & Assert
            await Assert.ThrowsAsync<ConflictException>(
                () => _service.CreateAsync(request));

            _projectRepositoryMock.Verify(r => r.Create(It.IsAny<Project>()), Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_Should_UpdateProject_When_RequestIsValid()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            var divisionId = Guid.NewGuid();

            var existingProject = new Project
            {
                Id = projectId,
                Name = "Application",
                Code = "APP",
                DivisionId = divisionId
            };

            var request = new UpdateProjectRequest
            {
                Name = "Website",
                Code = "WEB"
            };

            _projectRepositoryMock
                .Setup(r => r.GetByIdAsync(projectId))
                .ReturnsAsync(existingProject);

            // Act
            await _service.UpdateAsync(projectId, request);

            // Assert
            Assert.Equal(request.Name, existingProject.Name);
            Assert.Equal(request.Code, existingProject.Code);

            _projectRepositoryMock.Verify(r => r.Update(existingProject), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_Should_ThrowNotFoundException_When_ProjectDoesNotExist()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            var request = new UpdateProjectRequest
            {
                Name = "Application",
                Code = "APP"
            };

            _projectRepositoryMock
                .Setup(r => r.GetByIdAsync(projectId))
                .ReturnsAsync((Project?)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(
                () => _service.UpdateAsync(projectId, request));
        }

        [Fact]
        public async Task UpdateAsync_Should_ThrowConflictException_When_ManagerIsFromDifferentCompany()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            var companyId = Guid.NewGuid();
            var differentCompanyId = Guid.NewGuid();
            var divisionId = Guid.NewGuid();
            var managerId = Guid.NewGuid();

            var existingProject = new Project
            {
                Id = projectId,
                Name = "Application",
                Code = "APP",
                DivisionId = divisionId
            };

            var division = new Division
            {
                Id = divisionId,
                Name = "Software Development",
                Code = "SWDEV",
                CompanyId = companyId
            };

            var manager = new Employee
            {
                Id = managerId,
                FirstName = "John",
                LastName = "Doe",
                CompanyId = differentCompanyId
            };

            var request = new UpdateProjectRequest
            {
                Name = "Application",
                Code = "APP",
                ManagerId = managerId
            };

            _projectRepositoryMock
                .Setup(r => r.GetByIdAsync(projectId))
                .ReturnsAsync(existingProject);
            _employeeRepositoryMock
                .Setup(r => r.GetByIdAsync(managerId))
                .ReturnsAsync(manager);
            _divisionRepositoryMock
                .Setup(r => r.GetByIdAsync(divisionId))
                .ReturnsAsync(division);

            // Act & Assert
            await Assert.ThrowsAsync<ConflictException>(
                () => _service.UpdateAsync(projectId, request));
        }

        [Fact]
        public async Task DeleteAsync_Should_DeleteProject_When_NoDepartmentsExist()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            var project = new Project
            {
                Id = projectId,
                Name = "Application",
                Code = "APP",
                DivisionId = Guid.NewGuid()
            };

            _projectRepositoryMock
                .Setup(r => r.GetByIdAsync(projectId))
                .ReturnsAsync(project);
            _projectRepositoryMock
                .Setup(r => r.HasDepartmentsAsync(projectId))
                .ReturnsAsync(false);

            // Act
            await _service.DeleteAsync(projectId);

            // Assert
            _projectRepositoryMock.Verify(r => r.Delete(project), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_Should_ThrowNotFoundException_When_ProjectDoesNotExist()
        {
            // Arrange
            var projectId = Guid.NewGuid();

            _projectRepositoryMock
                .Setup(r => r.GetByIdAsync(projectId))
                .ReturnsAsync((Project?)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(
                () => _service.DeleteAsync(projectId));
        }

        [Fact]
        public async Task DeleteAsync_Should_ThrowConflictException_When_ProjectHasDepartments()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            var project = new Project
            {
                Id = projectId,
                Name = "Application",
                Code = "APP",
                DivisionId = Guid.NewGuid()
            };

            _projectRepositoryMock
                .Setup(r => r.GetByIdAsync(projectId))
                .ReturnsAsync(project);
            _projectRepositoryMock
                .Setup(r => r.HasDepartmentsAsync(projectId))
                .ReturnsAsync(true);

            // Act & Assert
            await Assert.ThrowsAsync<ConflictException>(
                () => _service.DeleteAsync(projectId));

            _projectRepositoryMock.Verify(r => r.Delete(It.IsAny<Project>()), Times.Never);
        }
    }
}
