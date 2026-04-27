using CompanyStructure.Application.Exceptions;
using CompanyStructure.Application.Features.Departments;
using CompanyStructure.Application.Features.Departments.Dtos;
using CompanyStructure.Application.Interfaces;
using CompanyStructure.Application.Interfaces.Repositories;
using CompanyStructure.Domain.Entities;
using Moq;

namespace CompanyStructure.Application.Tests.ServiceTests
{
    public class DepartmentServiceTests
    {
        private readonly Mock<IDepartmentRepository> _departmentRepositoryMock = new();
        private readonly Mock<IProjectRepository> _projectRepositoryMock = new();
        private readonly Mock<IDivisionRepository> _divisionRepositoryMock = new();
        private readonly Mock<IEmployeeRepository> _employeeRepositoryMock = new();
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
        private readonly DepartmentService _service;

        public DepartmentServiceTests()
        {
            _service = new DepartmentService(
                _departmentRepositoryMock.Object,
                _projectRepositoryMock.Object,
                _divisionRepositoryMock.Object,
                _employeeRepositoryMock.Object,
                _unitOfWorkMock.Object);
        }

        [Fact]
        public async Task GetByIdAsync_Should_ReturnDepartment_When_DepartmentExists()
        {
            // Arrange
            var departmentId = Guid.NewGuid();
            var department = new Department
            {
                Id = departmentId,
                Name = "Backend",
                Code = "BE",
                ProjectId = Guid.NewGuid()
            };

            _departmentRepositoryMock
                .Setup(r => r.GetByIdAsync(departmentId))
                .ReturnsAsync(department);

            // Act
            var result = await _service.GetByIdAsync(departmentId);

            // Assert
            Assert.Equal(department.Id, result.Id);
            Assert.Equal(department.Name, result.Name);
            Assert.Equal(department.Code, result.Code);
            Assert.Equal(department.ProjectId, result.ProjectId);
        }

        [Fact]
        public async Task GetByIdAsync_Should_ThrowNotFoundException_When_DepartmentDoesNotExist()
        {
            // Arrange
            var departmentId = Guid.NewGuid();

            _departmentRepositoryMock
                .Setup(r => r.GetByIdAsync(departmentId))
                .ReturnsAsync((Department?)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(
                () => _service.GetByIdAsync(departmentId));
        }

        [Fact]
        public async Task GetAllByProjectIdAsync_Should_ReturnAllDepartments_When_ProjectExists()
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
            var departments = new List<Department>
            {
                new() { Id = Guid.NewGuid(), Name = "Backend", Code = "BE", ProjectId = projectId },
                new() { Id = Guid.NewGuid(), Name = "Frontend", Code = "FE", ProjectId = projectId }
            };

            _projectRepositoryMock
                .Setup(r => r.GetByIdAsync(projectId))
                .ReturnsAsync(project);
            _departmentRepositoryMock
                .Setup(r => r.GetAllByProjectIdAsync(projectId))
                .ReturnsAsync(departments);

            // Act
            var result = await _service.GetAllByProjectIdAsync(projectId);

            // Assert
            Assert.Equal(departments.Count, result.Count);
            Assert.Equal(departments[0].Name, result[0].Name);
            Assert.Equal(departments[1].Name, result[1].Name);
        }

        [Fact]
        public async Task GetAllByProjectIdAsync_Should_ThrowNotFoundException_When_ProjectDoesNotExist()
        {
            // Arrange
            var projectId = Guid.NewGuid();

            _projectRepositoryMock
                .Setup(r => r.GetByIdAsync(projectId))
                .ReturnsAsync((Project?)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(
                () => _service.GetAllByProjectIdAsync(projectId));
        }

        [Fact]
        public async Task CreateAsync_Should_CreateDepartment_When_RequestIsValid()
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

            var request = new CreateDepartmentRequest
            {
                Name = "Backend",
                Code = "BE",
                ProjectId = projectId
            };

            _projectRepositoryMock
                .Setup(r => r.GetByIdAsync(projectId))
                .ReturnsAsync(project);

            // Act
            var result = await _service.CreateAsync(request);

            // Assert
            Assert.Equal(request.Name, result.Name);
            Assert.Equal(request.Code, result.Code);
            Assert.Equal(request.ProjectId, result.ProjectId);
            Assert.NotEqual(Guid.Empty, result.Id);

            _departmentRepositoryMock.Verify(r => r.Create(It.IsAny<Department>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_Should_ThrowNotFoundException_When_ProjectDoesNotExist()
        {
            // Arrange
            var request = new CreateDepartmentRequest
            {
                Name = "Backend",
                Code = "BE",
                ProjectId = Guid.NewGuid()
            };

            _projectRepositoryMock
                .Setup(r => r.GetByIdAsync(request.ProjectId))
                .ReturnsAsync((Project?)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(
                () => _service.CreateAsync(request));

            _departmentRepositoryMock.Verify(r => r.Create(It.IsAny<Department>()), Times.Never);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task CreateAsync_Should_ThrowNotFoundException_When_ManagerDoesNotExist()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            var managerId = Guid.NewGuid();
            var project = new Project
            {
                Id = projectId,
                Name = "Application",
                Code = "APP",
                DivisionId = Guid.NewGuid()
            };

            var request = new CreateDepartmentRequest
            {
                Name = "Backend",
                Code = "BE",
                ProjectId = projectId,
                ManagerId = managerId
            };

            _projectRepositoryMock
                .Setup(r => r.GetByIdAsync(projectId))
                .ReturnsAsync(project);
            _employeeRepositoryMock
                .Setup(r => r.GetByIdAsync(managerId))
                .ReturnsAsync((Employee?)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(
                () => _service.CreateAsync(request));

            _departmentRepositoryMock.Verify(r => r.Create(It.IsAny<Department>()), Times.Never);
        }

        [Fact]
        public async Task CreateAsync_Should_ThrowConflictException_When_ManagerIsFromDifferentCompany()
        {
            // Arrange
            var companyId = Guid.NewGuid();
            var differentCompanyId = Guid.NewGuid();
            var divisionId = Guid.NewGuid();
            var projectId = Guid.NewGuid();
            var managerId = Guid.NewGuid();

            var project = new Project
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

            var request = new CreateDepartmentRequest
            {
                Name = "Backend",
                Code = "BE",
                ProjectId = projectId,
                ManagerId = managerId
            };

            _projectRepositoryMock
                .Setup(r => r.GetByIdAsync(projectId))
                .ReturnsAsync(project);
            _employeeRepositoryMock
                .Setup(r => r.GetByIdAsync(managerId))
                .ReturnsAsync(manager);
            _divisionRepositoryMock
                .Setup(r => r.GetByIdAsync(divisionId))
                .ReturnsAsync(division);

            // Act & Assert
            await Assert.ThrowsAsync<ConflictException>(
                () => _service.CreateAsync(request));

            _departmentRepositoryMock.Verify(r => r.Create(It.IsAny<Department>()), Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_Should_UpdateDepartment_When_RequestIsValid()
        {
            // Arrange
            var departmentId = Guid.NewGuid();
            var projectId = Guid.NewGuid();

            var existingDepartment = new Department
            {
                Id = departmentId,
                Name = "Backend",
                Code = "BE",
                ProjectId = projectId
            };

            var request = new UpdateDepartmentRequest
            {
                Name = "Frontend",
                Code = "FE"
            };

            _departmentRepositoryMock
                .Setup(r => r.GetByIdAsync(departmentId))
                .ReturnsAsync(existingDepartment);

            // Act
            await _service.UpdateAsync(departmentId, request);

            // Assert
            Assert.Equal(request.Name, existingDepartment.Name);
            Assert.Equal(request.Code, existingDepartment.Code);

            _departmentRepositoryMock.Verify(r => r.Update(existingDepartment), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_Should_ThrowNotFoundException_When_DepartmentDoesNotExist()
        {
            // Arrange
            var departmentId = Guid.NewGuid();
            var request = new UpdateDepartmentRequest
            {
                Name = "Backend",
                Code = "BE"
            };

            _departmentRepositoryMock
                .Setup(r => r.GetByIdAsync(departmentId))
                .ReturnsAsync((Department?)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(
                () => _service.UpdateAsync(departmentId, request));
        }

        [Fact]
        public async Task UpdateAsync_Should_ThrowConflictException_When_ManagerIsFromDifferentCompany()
        {
            // Arrange
            var departmentId = Guid.NewGuid();
            var companyId = Guid.NewGuid();
            var differentCompanyId = Guid.NewGuid();
            var divisionId = Guid.NewGuid();
            var projectId = Guid.NewGuid();
            var managerId = Guid.NewGuid();

            var existingDepartment = new Department
            {
                Id = departmentId,
                Name = "Backend",
                Code = "BE",
                ProjectId = projectId
            };

            var project = new Project
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

            var request = new UpdateDepartmentRequest
            {
                Name = "Backend",
                Code = "BE",
                ManagerId = managerId
            };

            _departmentRepositoryMock
                .Setup(r => r.GetByIdAsync(departmentId))
                .ReturnsAsync(existingDepartment);
            _employeeRepositoryMock
                .Setup(r => r.GetByIdAsync(managerId))
                .ReturnsAsync(manager);
            _projectRepositoryMock
                .Setup(r => r.GetByIdAsync(projectId))
                .ReturnsAsync(project);
            _divisionRepositoryMock
                .Setup(r => r.GetByIdAsync(divisionId))
                .ReturnsAsync(division);

            // Act & Assert
            await Assert.ThrowsAsync<ConflictException>(
                () => _service.UpdateAsync(departmentId, request));
        }

        [Fact]
        public async Task DeleteAsync_Should_DeleteDepartment_When_DepartmentExists()
        {
            // Arrange
            var departmentId = Guid.NewGuid();
            var department = new Department
            {
                Id = departmentId,
                Name = "Backend",
                Code = "BE",
                ProjectId = Guid.NewGuid()
            };

            _departmentRepositoryMock
                .Setup(r => r.GetByIdAsync(departmentId))
                .ReturnsAsync(department);

            // Act
            await _service.DeleteAsync(departmentId);

            // Assert
            _departmentRepositoryMock.Verify(r => r.Delete(department), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_Should_ThrowNotFoundException_When_DepartmentDoesNotExist()
        {
            // Arrange
            var departmentId = Guid.NewGuid();

            _departmentRepositoryMock
                .Setup(r => r.GetByIdAsync(departmentId))
                .ReturnsAsync((Department?)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(
                () => _service.DeleteAsync(departmentId));
        }
    }
}
