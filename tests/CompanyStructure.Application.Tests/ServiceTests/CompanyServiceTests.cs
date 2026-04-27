using CompanyStructure.Application.Exceptions;
using CompanyStructure.Application.Features.Companies;
using CompanyStructure.Application.Features.Companies.Dtos;
using CompanyStructure.Application.Interfaces;
using CompanyStructure.Application.Interfaces.Repositories;
using CompanyStructure.Domain.Entities;
using Moq;

namespace CompanyStructure.Application.Tests.ServiceTests
{
    public class CompanyServiceTests
    {
        private readonly Mock<ICompanyRepository> _companyRepositoryMock = new();
        private readonly Mock<IEmployeeRepository> _employeeRepositoryMock = new();
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
        private readonly CompanyService _service;

        public CompanyServiceTests()
        {
            _service = new CompanyService(
                _companyRepositoryMock.Object,
                _employeeRepositoryMock.Object,
                _unitOfWorkMock.Object);
        }

        [Fact]
        public async Task GetByIdAsync_Should_ReturnCompany_When_CompanyExists()
        {
            // Arrange
            var companyId = Guid.NewGuid();
            var company = new Company
            {
                Id = companyId,
                Name = "Software Company",
                Code = "SWCOMP"
            };

            _companyRepositoryMock
                .Setup(r => r.GetByIdAsync(companyId))
                .ReturnsAsync(company);

            // Act
            var result = await _service.GetByIdAsync(companyId);

            // Assert
            Assert.Equal(companyId, result.Id);
            Assert.Equal(company.Name, result.Name);
            Assert.Equal(company.Code, result.Code);
        }

        [Fact]
        public async Task GetByIdAsync_Should_ThrowNotFoundException_When_CompanyDoesNotExist()
        {
            // Arrange
            var companyId = Guid.NewGuid();

            _companyRepositoryMock
                .Setup(r => r.GetByIdAsync(companyId))
                .ReturnsAsync((Company?)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _service.GetByIdAsync(companyId));
        }

        [Fact]
        public async Task GetAllAsync_Should_ReturnAllCompanies()
        {
            // Arrange
            var companies = new List<Company>
            {
                new() { Id = Guid.NewGuid(), Name = "Software Company", Code = "SWCOMP" },
                new() { Id = Guid.NewGuid(), Name = "Hardware Company", Code = "HWCOMP" }
            };

            _companyRepositoryMock
                .Setup(r => r.GetAllAsync())
                .ReturnsAsync(companies);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.Equal(companies.Count, result.Count);
            Assert.Equal(companies[0].Name, result[0].Name);
            Assert.Equal(companies[1].Name, result[1].Name);
        }

        [Fact]
        public async Task CreateAsync_Should_CreateCompany_When_RequestIsValid()
        {
            // Arrange
            var request = new CreateCompanyRequest
            {
                Name = "Software Company",
                Code = "SWCOMP"
            };

            // Act
            var result = await _service.CreateAsync(request);

            // Assert
            Assert.Equal(request.Name, result.Name);
            Assert.Equal(request.Code, result.Code);
            Assert.NotEqual(Guid.Empty, result.Id);

            _companyRepositoryMock.Verify(r => r.Create(It.IsAny<Company>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_Should_ThrowNotFoundException_When_ManagerDoesNotExist()
        {
            // Arrange
            var managerId = Guid.NewGuid();
            var request = new CreateCompanyRequest
            {
                Name = "Software Company",
                Code = "SWCOMP",
                ManagerId = managerId
            };

            _employeeRepositoryMock
                .Setup(r => r.GetByIdAsync(managerId))
                .ReturnsAsync((Employee?)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(
                () => _service.CreateAsync(request));

            _companyRepositoryMock.Verify(r => r.Create(It.IsAny<Company>()), Times.Never);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_Should_UpdateCompany_When_RequestIsValid()
        {
            // Arrange
            var companyId = Guid.NewGuid();
            var existingCompany = new Company
            {
                Id = companyId,
                Name = "Software Company",
                Code = "SWCOMP"
            };

            var request = new UpdateCompanyRequest
            {
                Name = "Hardware Company",
                Code = "HWCOMP"
            };

            _companyRepositoryMock
                .Setup(r => r.GetByIdAsync(companyId))
                .ReturnsAsync(existingCompany);

            // Act
            await _service.UpdateAsync(companyId, request);

            // Assert
            Assert.Equal(request.Name, existingCompany.Name);
            Assert.Equal(request.Code, existingCompany.Code);

            _companyRepositoryMock.Verify(r => r.Update(existingCompany), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_Should_ThrowNotFoundException_When_CompanyDoesNotExist()
        {
            // Arrange
            var companyId = Guid.NewGuid();
            var request = new UpdateCompanyRequest
            {
                Name = "Software Company",
                Code = "SWCOMP"
            };

            _companyRepositoryMock
                .Setup(r => r.GetByIdAsync(companyId))
                .ReturnsAsync((Company?)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(
                () => _service.UpdateAsync(companyId, request));
        }

        [Fact]
        public async Task UpdateAsync_Should_ThrowConflictException_When_ManagerIsNotInCompany()
        {
            // Arrange
            var companyId = Guid.NewGuid();
            var managerId = Guid.NewGuid();
            var differentCompanyId = Guid.NewGuid();

            var existingCompany = new Company
            {
                Id = companyId,
                Name = "Software Company",
                Code = "SWCOMP"
            };

            var manager = new Employee
            {
                Id = managerId,
                FirstName = "John",
                LastName = "Doe",
                CompanyId = differentCompanyId
            };

            var request = new UpdateCompanyRequest
            {
                Name = "Software Company",
                Code = "SWCOMP",
                ManagerId = managerId
            };

            _companyRepositoryMock
                .Setup(r => r.GetByIdAsync(companyId))
                .ReturnsAsync(existingCompany);
            _employeeRepositoryMock
                .Setup(r => r.GetByIdAsync(managerId))
                .ReturnsAsync(manager);

            // Act & Assert
            await Assert.ThrowsAsync<ConflictException>(
                () => _service.UpdateAsync(companyId, request));
        }

        [Fact]
        public async Task DeleteAsync_Should_DeleteCompany_When_NoDivisionsAndNoEmployeesExist()
        {
            // Arrange
            var companyId = Guid.NewGuid();
            var company = new Company
            {
                Id = companyId,
                Name = "Software Company",
                Code = "SWCOMP"
            };

            _companyRepositoryMock
                .Setup(r => r.GetByIdAsync(companyId))
                .ReturnsAsync(company);
            _companyRepositoryMock
                .Setup(r => r.HasDivisionsAsync(companyId))
                .ReturnsAsync(false);
            _companyRepositoryMock
                .Setup(r => r.HasEmployeesAsync(companyId))
                .ReturnsAsync(false);

            // Act
            await _service.DeleteAsync(companyId);

            // Assert
            _companyRepositoryMock.Verify(r => r.Delete(company), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_Should_ThrowNotFoundException_When_CompanyDoesNotExist()
        {
            // Arrange
            var companyId = Guid.NewGuid();

            _companyRepositoryMock
                .Setup(r => r.GetByIdAsync(companyId))
                .ReturnsAsync((Company?)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(
                () => _service.DeleteAsync(companyId));
        }

        [Fact]
        public async Task DeleteAsync_Should_ThrowConflictException_When_CompanyHasDivisions()
        {
            // Arrange
            var companyId = Guid.NewGuid();
            var company = new Company
            {
                Id = companyId,
                Name = "Software Company",
                Code = "SWCOMP"
            };

            _companyRepositoryMock
                .Setup(r => r.GetByIdAsync(companyId))
                .ReturnsAsync(company);
            _companyRepositoryMock
                .Setup(r => r.HasDivisionsAsync(companyId))
                .ReturnsAsync(true);

            // Act & Assert
            await Assert.ThrowsAsync<ConflictException>(
                () => _service.DeleteAsync(companyId));

            _companyRepositoryMock.Verify(r => r.Delete(It.IsAny<Company>()), Times.Never);
        }

        [Fact]
        public async Task DeleteAsync_Should_ThrowConflictException_When_CompanyHasEmployees()
        {
            // Arrange
            var companyId = Guid.NewGuid();
            var company = new Company
            {
                Id = companyId,
                Name = "Software Company",
                Code = "SWCOMP"
            };

            _companyRepositoryMock
                .Setup(r => r.GetByIdAsync(companyId))
                .ReturnsAsync(company);
            _companyRepositoryMock
                .Setup(r => r.HasDivisionsAsync(companyId))
                .ReturnsAsync(false);
            _companyRepositoryMock
                .Setup(r => r.HasEmployeesAsync(companyId))
                .ReturnsAsync(true);

            // Act & Assert
            await Assert.ThrowsAsync<ConflictException>(
                () => _service.DeleteAsync(companyId));

            _companyRepositoryMock.Verify(r => r.Delete(It.IsAny<Company>()), Times.Never);
        }
    }
}
