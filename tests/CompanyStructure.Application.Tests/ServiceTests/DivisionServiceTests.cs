using CompanyStructure.Application.Exceptions;
using CompanyStructure.Application.Features.Divisions;
using CompanyStructure.Application.Features.Divisions.Dtos;
using CompanyStructure.Application.Interfaces;
using CompanyStructure.Application.Interfaces.Repositories;
using CompanyStructure.Domain.Entities;
using Moq;

namespace CompanyStructure.Application.Tests.ServiceTests
{
    public class DivisionServiceTests
    {
        private readonly Mock<IDivisionRepository> _divisionRepositoryMock = new();
        private readonly Mock<ICompanyRepository> _companyRepositoryMock = new();
        private readonly Mock<IEmployeeRepository> _employeeRepositoryMock = new();
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
        private readonly DivisionService _service;

        public DivisionServiceTests()
        {
            _service = new DivisionService(
                _divisionRepositoryMock.Object,
                _companyRepositoryMock.Object,
                _employeeRepositoryMock.Object,
                _unitOfWorkMock.Object);
        }

        [Fact]
        public async Task GetByIdAsync_Should_ReturnDivision_When_DivisionExists()
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

            _divisionRepositoryMock
                .Setup(r => r.GetByIdAsync(divisionId))
                .ReturnsAsync(division);

            // Act
            var result = await _service.GetByIdAsync(divisionId);

            // Assert
            Assert.Equal(division.Id, result.Id);
            Assert.Equal(division.Name, result.Name);
            Assert.Equal(division.Code, result.Code);
            Assert.Equal(division.CompanyId, result.CompanyId);
        }

        [Fact]
        public async Task GetByIdAsync_Should_ThrowNotFoundException_When_DivisionDoesNotExist()
        {
            // Arrange
            var divisionId = Guid.NewGuid();

            _divisionRepositoryMock
                .Setup(r => r.GetByIdAsync(divisionId))
                .ReturnsAsync((Division?)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(
                () => _service.GetByIdAsync(divisionId));
        }

        [Fact]
        public async Task GetAllByCompanyIdAsync_Should_ReturnAllDivisions_When_CompanyExists()
        {
            // Arrange
            var companyId = Guid.NewGuid();
            var company = new Company { Id = companyId, Name = "Software Company", Code = "SWCOMP" };
            var divisions = new List<Division>
            {
                new() { Id = Guid.NewGuid(), Name = "Software Development", Code = "SWDEV", CompanyId = companyId },
                new() { Id = Guid.NewGuid(), Name = "Hardware Development", Code = "HWDEV", CompanyId = companyId }
            };

            _companyRepositoryMock
                .Setup(r => r.GetByIdAsync(companyId))
                .ReturnsAsync(company);
            _divisionRepositoryMock
                .Setup(r => r.GetAllByCompanyIdAsync(companyId))
                .ReturnsAsync(divisions);

            // Act
            var result = await _service.GetAllByCompanyIdAsync(companyId);

            // Assert
            Assert.Equal(divisions.Count, result.Count);
            Assert.Equal(divisions[0].Name, result[0].Name);
            Assert.Equal(divisions[1].Name, result[1].Name);
        }

        [Fact]
        public async Task GetAllByCompanyIdAsync_Should_ThrowNotFoundException_When_CompanyDoesNotExist()
        {
            // Arrange
            var companyId = Guid.NewGuid();

            _companyRepositoryMock
                .Setup(r => r.GetByIdAsync(companyId))
                .ReturnsAsync((Company?)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(
                () => _service.GetAllByCompanyIdAsync(companyId));
        }

        [Fact]
        public async Task CreateAsync_Should_CreateDivision_When_RequestIsValid()
        {
            // Arrange
            var companyId = Guid.NewGuid();
            var company = new Company { Id = companyId, Name = "Software Company", Code = "SWCOMP" };

            var request = new CreateDivisionRequest
            {
                Name = "Software Development",
                Code = "SWDEV",
                CompanyId = companyId
            };

            _companyRepositoryMock
                .Setup(r => r.GetByIdAsync(companyId))
                .ReturnsAsync(company);

            // Act
            var result = await _service.CreateAsync(request);

            // Assert
            Assert.Equal(request.Name, result.Name);
            Assert.Equal(request.Code, result.Code);
            Assert.Equal(request.CompanyId, result.CompanyId);
            Assert.NotEqual(Guid.Empty, result.Id);

            _divisionRepositoryMock.Verify(r => r.Create(It.IsAny<Division>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_Should_ThrowNotFoundException_When_CompanyDoesNotExist()
        {
            // Arrange
            var request = new CreateDivisionRequest
            {
                Name = "Software Development",
                Code = "SWDEV",
                CompanyId = Guid.NewGuid()
            };

            _companyRepositoryMock
                .Setup(r => r.GetByIdAsync(request.CompanyId))
                .ReturnsAsync((Company?)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(
                () => _service.CreateAsync(request));

            _divisionRepositoryMock.Verify(r => r.Create(It.IsAny<Division>()), Times.Never);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task CreateAsync_Should_ThrowNotFoundException_When_ManagerDoesNotExist()
        {
            // Arrange
            var companyId = Guid.NewGuid();
            var managerId = Guid.NewGuid();
            var company = new Company { Id = companyId, Name = "Software Company", Code = "SWCOMP" };

            var request = new CreateDivisionRequest
            {
                Name = "Software Development",
                Code = "SWDEV",
                CompanyId = companyId,
                ManagerId = managerId
            };

            _companyRepositoryMock
                .Setup(r => r.GetByIdAsync(companyId))
                .ReturnsAsync(company);
            _employeeRepositoryMock
                .Setup(r => r.GetByIdAsync(managerId))
                .ReturnsAsync((Employee?)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(
                () => _service.CreateAsync(request));

            _divisionRepositoryMock.Verify(r => r.Create(It.IsAny<Division>()), Times.Never);
        }

        [Fact]
        public async Task CreateAsync_Should_ThrowConflictException_When_ManagerIsFromDifferentCompany()
        {
            // Arrange
            var companyId = Guid.NewGuid();
            var differentCompanyId = Guid.NewGuid();
            var managerId = Guid.NewGuid();

            var company = new Company { Id = companyId, Name = "Software Company", Code = "SWCOMP" };
            var manager = new Employee
            {
                Id = managerId,
                FirstName = "John",
                LastName = "Doe",
                CompanyId = differentCompanyId
            };

            var request = new CreateDivisionRequest
            {
                Name = "Software Development",
                Code = "SWDEV",
                CompanyId = companyId,
                ManagerId = managerId
            };

            _companyRepositoryMock
                .Setup(r => r.GetByIdAsync(companyId))
                .ReturnsAsync(company);
            _employeeRepositoryMock
                .Setup(r => r.GetByIdAsync(managerId))
                .ReturnsAsync(manager);

            // Act & Assert
            await Assert.ThrowsAsync<ConflictException>(
                () => _service.CreateAsync(request));

            _divisionRepositoryMock.Verify(r => r.Create(It.IsAny<Division>()), Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_Should_UpdateDivision_When_RequestIsValid()
        {
            // Arrange
            var divisionId = Guid.NewGuid();
            var companyId = Guid.NewGuid();

            var existingDivision = new Division
            {
                Id = divisionId,
                Name = "Software Development",
                Code = "SWDEV",
                CompanyId = companyId
            };

            var request = new UpdateDivisionRequest
            {
                Name = "Hardware Development",
                Code = "HWDEV"
            };

            _divisionRepositoryMock
                .Setup(r => r.GetByIdAsync(divisionId))
                .ReturnsAsync(existingDivision);

            // Act
            await _service.UpdateAsync(divisionId, request);

            // Assert
            Assert.Equal(request.Name, existingDivision.Name);
            Assert.Equal(request.Code, existingDivision.Code);

            _divisionRepositoryMock.Verify(r => r.Update(existingDivision), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_Should_ThrowNotFoundException_When_DivisionDoesNotExist()
        {
            // Arrange
            var divisionId = Guid.NewGuid();
            var request = new UpdateDivisionRequest
            {
                Name = "Software Development",
                Code = "SWDEV"
            };

            _divisionRepositoryMock
                .Setup(r => r.GetByIdAsync(divisionId))
                .ReturnsAsync((Division?)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(
                () => _service.UpdateAsync(divisionId, request));
        }

        [Fact]
        public async Task UpdateAsync_Should_ThrowConflictException_When_ManagerIsFromDifferentCompany()
        {
            // Arrange
            var divisionId = Guid.NewGuid();
            var companyId = Guid.NewGuid();
            var differentCompanyId = Guid.NewGuid();
            var managerId = Guid.NewGuid();

            var existingDivision = new Division
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

            var request = new UpdateDivisionRequest
            {
                Name = "Software Development",
                Code = "SWDEV",
                ManagerId = managerId
            };

            _divisionRepositoryMock
                .Setup(r => r.GetByIdAsync(divisionId))
                .ReturnsAsync(existingDivision);
            _employeeRepositoryMock
                .Setup(r => r.GetByIdAsync(managerId))
                .ReturnsAsync(manager);

            // Act & Assert
            await Assert.ThrowsAsync<ConflictException>(
                () => _service.UpdateAsync(divisionId, request));
        }

        [Fact]
        public async Task DeleteAsync_Should_DeleteDivision_When_NoProjectsExist()
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

            _divisionRepositoryMock
                .Setup(r => r.GetByIdAsync(divisionId))
                .ReturnsAsync(division);
            _divisionRepositoryMock
                .Setup(r => r.HasProjectsAsync(divisionId))
                .ReturnsAsync(false);

            // Act
            await _service.DeleteAsync(divisionId);

            // Assert
            _divisionRepositoryMock.Verify(r => r.Delete(division), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_Should_ThrowNotFoundException_When_DivisionDoesNotExist()
        {
            // Arrange
            var divisionId = Guid.NewGuid();

            _divisionRepositoryMock
                .Setup(r => r.GetByIdAsync(divisionId))
                .ReturnsAsync((Division?)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(
                () => _service.DeleteAsync(divisionId));
        }

        [Fact]
        public async Task DeleteAsync_Should_ThrowConflictException_When_DivisionHasProjects()
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

            _divisionRepositoryMock
                .Setup(r => r.GetByIdAsync(divisionId))
                .ReturnsAsync(division);
            _divisionRepositoryMock
                .Setup(r => r.HasProjectsAsync(divisionId))
                .ReturnsAsync(true);

            // Act & Assert
            await Assert.ThrowsAsync<ConflictException>(
                () => _service.DeleteAsync(divisionId));

            _divisionRepositoryMock.Verify(r => r.Delete(It.IsAny<Division>()), Times.Never);
        }
    }
}
