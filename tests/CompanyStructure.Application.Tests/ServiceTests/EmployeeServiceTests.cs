using CompanyStructure.Application.Exceptions;
using CompanyStructure.Application.Features.Employees;
using CompanyStructure.Application.Features.Employees.Dtos;
using CompanyStructure.Application.Interfaces;
using CompanyStructure.Application.Interfaces.Repositories;
using CompanyStructure.Domain.Entities;
using Moq;

namespace CompanyStructure.Application.Tests.ServiceTests
{
    public class EmployeeServiceTests
    {
        private readonly Mock<IEmployeeRepository> _employeeRepositoryMock = new();
        private readonly Mock<ICompanyRepository> _companyRepositoryMock = new();
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
        private readonly EmployeeService _service;

        public EmployeeServiceTests()
        {
            _service = new EmployeeService(
                _employeeRepositoryMock.Object,
                _companyRepositoryMock.Object,
                _unitOfWorkMock.Object);
        }

        [Fact]
        public async Task GetByIdAsync_Should_ReturnEmployee_When_EmployeeExists()
        {
            // Arrange
            var employeeId = Guid.NewGuid();
            var employee = new Employee
            {
                Id = employeeId,
                Title = "Ing.",
                FirstName = "John",
                LastName = "Doe",
                Phone = "+421 123 456 789",
                Email = "john.doe@example.com",
                CompanyId = Guid.NewGuid()
            };

            _employeeRepositoryMock
                .Setup(r => r.GetByIdAsync(employeeId))
                .ReturnsAsync(employee);

            // Act
            var result = await _service.GetByIdAsync(employeeId);

            // Assert
            Assert.Equal(employee.Id, result.Id);
            Assert.Equal(employee.Title, result.Title);
            Assert.Equal(employee.FirstName, result.FirstName);
            Assert.Equal(employee.LastName, result.LastName);
            Assert.Equal(employee.Phone, result.Phone);
            Assert.Equal(employee.Email, result.Email);
            Assert.Equal(employee.CompanyId, result.CompanyId);
        }

        [Fact]
        public async Task GetByIdAsync_Should_ThrowNotFoundException_When_EmployeeDoesNotExist()
        {
            // Arrange
            var employeeId = Guid.NewGuid();

            _employeeRepositoryMock
                .Setup(r => r.GetByIdAsync(employeeId))
                .ReturnsAsync((Employee?)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(
                () => _service.GetByIdAsync(employeeId));
        }

        [Fact]
        public async Task GetAllByCompanyIdAsync_Should_ReturnAllEmployees_When_CompanyExists()
        {
            // Arrange
            var companyId = Guid.NewGuid();
            var company = new Company { Id = companyId, Name = "Software Company", Code = "SWCOMP" };
            var employees = new List<Employee>
            {
                new() { Id = Guid.NewGuid(), FirstName = "John", LastName = "Doe", CompanyId = companyId },
                new() { Id = Guid.NewGuid(), FirstName = "Jane", LastName = "Smith", CompanyId = companyId }
            };

            _companyRepositoryMock
                .Setup(r => r.GetByIdAsync(companyId))
                .ReturnsAsync(company);
            _employeeRepositoryMock
                .Setup(r => r.GetAllByCompanyIdAsync(companyId))
                .ReturnsAsync(employees);

            // Act
            var result = await _service.GetAllByCompanyIdAsync(companyId);

            // Assert
            Assert.Equal(employees.Count, result.Count);
            Assert.Equal(employees[0].FirstName, result[0].FirstName);
            Assert.Equal(employees[1].FirstName, result[1].FirstName);
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
        public async Task CreateAsync_Should_CreateEmployee_When_RequestIsValid()
        {
            // Arrange
            var companyId = Guid.NewGuid();
            var company = new Company { Id = companyId, Name = "Software Company", Code = "SWCOMP" };

            var request = new CreateEmployeeRequest
            {
                Title = "Ing.",
                FirstName = "John",
                LastName = "Doe",
                Phone = "+421 123 456 789",
                Email = "john.doe@example.com",
                CompanyId = companyId
            };

            _companyRepositoryMock
                .Setup(r => r.GetByIdAsync(companyId))
                .ReturnsAsync(company);

            // Act
            var result = await _service.CreateAsync(request);

            // Assert
            Assert.Equal(request.Title, result.Title);
            Assert.Equal(request.FirstName, result.FirstName);
            Assert.Equal(request.LastName, result.LastName);
            Assert.Equal(request.Phone, result.Phone);
            Assert.Equal(request.Email, result.Email);
            Assert.Equal(request.CompanyId, result.CompanyId);
            Assert.NotEqual(Guid.Empty, result.Id);

            _employeeRepositoryMock.Verify(r => r.Create(It.IsAny<Employee>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_Should_ThrowNotFoundException_When_CompanyDoesNotExist()
        {
            // Arrange
            var request = new CreateEmployeeRequest
            {
                FirstName = "John",
                LastName = "Doe",
                CompanyId = Guid.NewGuid()
            };

            _companyRepositoryMock
                .Setup(r => r.GetByIdAsync(request.CompanyId))
                .ReturnsAsync((Company?)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(
                () => _service.CreateAsync(request));

            _employeeRepositoryMock.Verify(r => r.Create(It.IsAny<Employee>()), Times.Never);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_Should_UpdateEmployee_When_RequestIsValid()
        {
            // Arrange
            var employeeId = Guid.NewGuid();
            var existingEmployee = new Employee
            {
                Id = employeeId,
                Title = "Ing.",
                FirstName = "John",
                LastName = "Doe",
                Phone = "+421 123 456 789",
                Email = "john.doe@example.com",
                CompanyId = Guid.NewGuid()
            };

            var request = new UpdateEmployeeRequest
            {
                Title = "Mgr.",
                FirstName = "Jane",
                LastName = "Smith",
                Phone = "+421 987 654 321",
                Email = "jane.smith@example.com"
            };

            _employeeRepositoryMock
                .Setup(r => r.GetByIdAsync(employeeId))
                .ReturnsAsync(existingEmployee);

            // Act
            await _service.UpdateAsync(employeeId, request);

            // Assert
            Assert.Equal(request.Title, existingEmployee.Title);
            Assert.Equal(request.FirstName, existingEmployee.FirstName);
            Assert.Equal(request.LastName, existingEmployee.LastName);
            Assert.Equal(request.Phone, existingEmployee.Phone);
            Assert.Equal(request.Email, existingEmployee.Email);

            _employeeRepositoryMock.Verify(r => r.Update(existingEmployee), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_Should_ThrowNotFoundException_When_EmployeeDoesNotExist()
        {
            // Arrange
            var employeeId = Guid.NewGuid();
            var request = new UpdateEmployeeRequest
            {
                FirstName = "John",
                LastName = "Doe"
            };

            _employeeRepositoryMock
                .Setup(r => r.GetByIdAsync(employeeId))
                .ReturnsAsync((Employee?)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(
                () => _service.UpdateAsync(employeeId, request));
        }

        [Fact]
        public async Task DeleteAsync_Should_DeleteEmployee_When_EmployeeExists()
        {
            // Arrange
            var employeeId = Guid.NewGuid();
            var employee = new Employee
            {
                Id = employeeId,
                FirstName = "John",
                LastName = "Doe",
                CompanyId = Guid.NewGuid()
            };

            _employeeRepositoryMock
                .Setup(r => r.GetByIdAsync(employeeId))
                .ReturnsAsync(employee);

            // Act
            await _service.DeleteAsync(employeeId);

            // Assert
            _employeeRepositoryMock.Verify(r => r.Delete(employee), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_Should_ThrowNotFoundException_When_EmployeeDoesNotExist()
        {
            // Arrange
            var employeeId = Guid.NewGuid();

            _employeeRepositoryMock
                .Setup(r => r.GetByIdAsync(employeeId))
                .ReturnsAsync((Employee?)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(
                () => _service.DeleteAsync(employeeId));
        }
    }
}
