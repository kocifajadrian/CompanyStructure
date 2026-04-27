using CompanyStructure.Application.Features.Employees.Dtos;
using CompanyStructure.Application.Features.Employees.Validators;
using FluentValidation.TestHelper;

namespace CompanyStructure.Application.Tests.ValidatorTests.Employees
{
    public class CreateEmployeeRequestValidatorTests
    {
        private readonly CreateEmployeeRequestValidator _validator = new();

        [Fact]
        public void Should_NotHaveError_When_TitleIsEmpty()
        {
            var request = new CreateEmployeeRequest
            {
                FirstName = "John",
                LastName = "Doe",
                Title = string.Empty,
                CompanyId = Guid.NewGuid()
            };

            var result = _validator.TestValidate(request);

            result.ShouldNotHaveValidationErrorFor(x => x.Title);
        }

        [Fact]
        public void Should_NotHaveError_When_TitleIsNull()
        {
            var request = new CreateEmployeeRequest
            {
                FirstName = "John",
                LastName = "Doe",
                Title = null,
                CompanyId = Guid.NewGuid()
            };

            var result = _validator.TestValidate(request);

            result.ShouldNotHaveValidationErrorFor(x => x.Title);
        }

        [Fact]
        public void Should_HaveError_When_FirstNameIsEmpty()
        {
            var request = new CreateEmployeeRequest
            {
                FirstName = string.Empty,
                LastName = "Doe",
                CompanyId = Guid.NewGuid()
            };

            var result = _validator.TestValidate(request);

            result.ShouldHaveValidationErrorFor(x => x.FirstName);
        }

        [Fact]
        public void Should_HaveError_When_FirstNameIsNull()
        {
            var request = new CreateEmployeeRequest
            {
                FirstName = null!,
                LastName = "Doe",
                CompanyId = Guid.NewGuid()
            };

            var result = _validator.TestValidate(request);

            result.ShouldHaveValidationErrorFor(x => x.FirstName);
        }

        [Fact]
        public void Should_HaveError_When_LastNameIsEmpty()
        {
            var request = new CreateEmployeeRequest
            {
                FirstName = "John",
                LastName = string.Empty,
                CompanyId = Guid.NewGuid()
            };

            var result = _validator.TestValidate(request);

            result.ShouldHaveValidationErrorFor(x => x.LastName);
        }

        [Fact]
        public void Should_HaveError_When_LastNameIsNull()
        {
            var request = new CreateEmployeeRequest
            {
                FirstName = "John",
                LastName = null!,
                CompanyId = Guid.NewGuid()
            };

            var result = _validator.TestValidate(request);

            result.ShouldHaveValidationErrorFor(x => x.LastName);
        }

        [Fact]
        public void Should_NotHaveError_When_PhoneIsEmpty()
        {
            var request = new CreateEmployeeRequest
            {
                FirstName = "John",
                LastName = "Doe",
                Phone = string.Empty,
                CompanyId = Guid.NewGuid()
            };

            var result = _validator.TestValidate(request);

            result.ShouldNotHaveValidationErrorFor(x => x.Phone);
        }

        [Fact]
        public void Should_NotHaveError_When_PhoneIsNull()
        {
            var request = new CreateEmployeeRequest
            {
                FirstName = "John",
                LastName = "Doe",
                Phone = null,
                CompanyId = Guid.NewGuid()
            };

            var result = _validator.TestValidate(request);

            result.ShouldNotHaveValidationErrorFor(x => x.Phone);
        }

        [Fact]
        public void Should_NotHaveError_When_EmailIsEmpty()
        {
            var request = new CreateEmployeeRequest
            {
                FirstName = "John",
                LastName = "Doe",
                Email = string.Empty,
                CompanyId = Guid.NewGuid()
            };

            var result = _validator.TestValidate(request);

            result.ShouldNotHaveValidationErrorFor(x => x.Email);
        }

        [Fact]
        public void Should_NotHaveError_When_EmailIsNull()
        {
            var request = new CreateEmployeeRequest
            {
                FirstName = "John",
                LastName = "Doe",
                Email = null,
                CompanyId = Guid.NewGuid()
            };

            var result = _validator.TestValidate(request);

            result.ShouldNotHaveValidationErrorFor(x => x.Email);
        }

        [Fact]
        public void Should_HaveError_When_EmailIsInvalid()
        {
            var request = new CreateEmployeeRequest
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "invalid-email",
                CompanyId = Guid.NewGuid()
            };

            var result = _validator.TestValidate(request);

            result.ShouldHaveValidationErrorFor(x => x.Email);
        }

        [Fact]
        public void Should_NotHaveError_When_EmailIsValid()
        {
            var request = new CreateEmployeeRequest
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                CompanyId = Guid.NewGuid()
            };

            var result = _validator.TestValidate(request);

            result.ShouldNotHaveValidationErrorFor(x => x.Email);
        }

        [Fact]
        public void Should_HaveError_When_CompanyIdIsEmpty()
        {
            var request = new CreateEmployeeRequest
            {
                FirstName = "John",
                LastName = "Doe",
                CompanyId = Guid.Empty
            };

            var result = _validator.TestValidate(request);

            result.ShouldHaveValidationErrorFor(x => x.CompanyId);
        }
    }
}
