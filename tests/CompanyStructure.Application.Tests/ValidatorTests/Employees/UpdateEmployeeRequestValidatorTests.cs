using CompanyStructure.Application.Features.Employees.Dtos;
using CompanyStructure.Application.Features.Employees.Validators;
using FluentValidation.TestHelper;

namespace CompanyStructure.Application.Tests.ValidatorTests.Employees
{
    public class UpdateEmployeeRequestValidatorTests
    {
        private readonly UpdateEmployeeRequestValidator _validator = new();

        [Fact]
        public void Should_NotHaveError_When_TitleIsEmpty()
        {
            var request = new UpdateEmployeeRequest
            {
                FirstName = "John",
                LastName = "Doe",
                Title = string.Empty
            };

            var result = _validator.TestValidate(request);

            result.ShouldNotHaveValidationErrorFor(x => x.Title);
        }

        [Fact]
        public void Should_NotHaveError_When_TitleIsNull()
        {
            var request = new UpdateEmployeeRequest
            {
                FirstName = "John",
                LastName = "Doe",
                Title = null
            };

            var result = _validator.TestValidate(request);

            result.ShouldNotHaveValidationErrorFor(x => x.Title);
        }

        [Fact]
        public void Should_HaveError_When_FirstNameIsEmpty()
        {
            var request = new UpdateEmployeeRequest
            {
                FirstName = string.Empty,
                LastName = "Doe"
            };

            var result = _validator.TestValidate(request);

            result.ShouldHaveValidationErrorFor(x => x.FirstName);
        }

        [Fact]
        public void Should_HaveError_When_FirstNameIsNull()
        {
            var request = new UpdateEmployeeRequest
            {
                FirstName = null!,
                LastName = "Doe"
            };

            var result = _validator.TestValidate(request);

            result.ShouldHaveValidationErrorFor(x => x.FirstName);
        }

        [Fact]
        public void Should_HaveError_When_LastNameIsEmpty()
        {
            var request = new UpdateEmployeeRequest
            {
                FirstName = "John",
                LastName = string.Empty
            };

            var result = _validator.TestValidate(request);

            result.ShouldHaveValidationErrorFor(x => x.LastName);
        }

        [Fact]
        public void Should_HaveError_When_LastNameIsNull()
        {
            var request = new UpdateEmployeeRequest
            {
                FirstName = "John",
                LastName = null!
            };

            var result = _validator.TestValidate(request);

            result.ShouldHaveValidationErrorFor(x => x.LastName);
        }

        [Fact]
        public void Should_NotHaveError_When_PhoneIsEmpty()
        {
            var request = new UpdateEmployeeRequest
            {
                FirstName = "John",
                LastName = "Doe",
                Phone = string.Empty
            };

            var result = _validator.TestValidate(request);

            result.ShouldNotHaveValidationErrorFor(x => x.Phone);
        }

        [Fact]
        public void Should_NotHaveError_When_PhoneIsNull()
        {
            var request = new UpdateEmployeeRequest
            {
                FirstName = "John",
                LastName = "Doe",
                Phone = null
            };

            var result = _validator.TestValidate(request);

            result.ShouldNotHaveValidationErrorFor(x => x.Phone);
        }

        [Fact]
        public void Should_NotHaveError_When_EmailIsEmpty()
        {
            var request = new UpdateEmployeeRequest
            {
                FirstName = "John",
                LastName = "Doe",
                Email = string.Empty
            };

            var result = _validator.TestValidate(request);

            result.ShouldNotHaveValidationErrorFor(x => x.Email);
        }

        [Fact]
        public void Should_NotHaveError_When_EmailIsNull()
        {
            var request = new UpdateEmployeeRequest
            {
                FirstName = "John",
                LastName = "Doe",
                Email = null
            };

            var result = _validator.TestValidate(request);

            result.ShouldNotHaveValidationErrorFor(x => x.Email);
        }

        [Fact]
        public void Should_HaveError_When_EmailIsInvalid()
        {
            var request = new UpdateEmployeeRequest
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "invalid-email"
            };

            var result = _validator.TestValidate(request);

            result.ShouldHaveValidationErrorFor(x => x.Email);
        }

        [Fact]
        public void Should_NotHaveError_When_EmailIsValid()
        {
            var request = new UpdateEmployeeRequest
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com"
            };

            var result = _validator.TestValidate(request);

            result.ShouldNotHaveValidationErrorFor(x => x.Email);
        }
    }
}
