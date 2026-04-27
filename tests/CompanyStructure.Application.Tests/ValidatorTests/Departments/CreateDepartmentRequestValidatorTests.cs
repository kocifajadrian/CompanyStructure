using CompanyStructure.Application.Features.Departments.Dtos;
using CompanyStructure.Application.Features.Departments.Validators;
using FluentValidation.TestHelper;

namespace CompanyStructure.Application.Tests.ValidatorTests.Departments
{
    public class CreateDepartmentRequestValidatorTests
    {
        private readonly CreateDepartmentRequestValidator _validator = new();

        [Fact]
        public void Should_HaveError_When_NameIsEmpty()
        {
            var request = new CreateDepartmentRequest
            {
                Name = string.Empty,
                Code = "BE",
                ProjectId = Guid.NewGuid()
            };

            var result = _validator.TestValidate(request);

            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Should_HaveError_When_NameIsNull()
        {
            var request = new CreateDepartmentRequest
            {
                Name = null!,
                Code = "BE",
                ProjectId = Guid.NewGuid()
            };

            var result = _validator.TestValidate(request);

            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Should_HaveError_When_CodeIsEmpty()
        {
            var request = new CreateDepartmentRequest
            {
                Name = "Backend",
                Code = string.Empty,
                ProjectId = Guid.NewGuid()
            };

            var result = _validator.TestValidate(request);

            result.ShouldHaveValidationErrorFor(x => x.Code);
        }

        [Fact]
        public void Should_HaveError_When_CodeIsNull()
        {
            var request = new CreateDepartmentRequest
            {
                Name = "Backend",
                Code = null!,
                ProjectId = Guid.NewGuid()
            };

            var result = _validator.TestValidate(request);

            result.ShouldHaveValidationErrorFor(x => x.Code);
        }

        [Fact]
        public void Should_HaveError_When_ProjectIdIsEmpty()
        {
            var request = new CreateDepartmentRequest
            {
                Name = "Backend",
                Code = "BE",
                ProjectId = Guid.Empty
            };

            var result = _validator.TestValidate(request);

            result.ShouldHaveValidationErrorFor(x => x.ProjectId);
        }

        [Fact]
        public void Should_NotHaveError_When_ManagerIdIsNull()
        {
            var request = new CreateDepartmentRequest
            {
                Name = "Backend",
                Code = "BE",
                ProjectId = Guid.NewGuid(),
                ManagerId = null
            };

            var result = _validator.TestValidate(request);

            result.ShouldNotHaveValidationErrorFor(x => x.ManagerId);
        }

        [Fact]
        public void Should_NotHaveError_When_ManagerIdIsProvided()
        {
            var request = new CreateDepartmentRequest
            {
                Name = "Backend",
                Code = "BE",
                ProjectId = Guid.NewGuid(),
                ManagerId = Guid.NewGuid()
            };

            var result = _validator.TestValidate(request);

            result.ShouldNotHaveValidationErrorFor(x => x.ManagerId);
        }
    }
}
