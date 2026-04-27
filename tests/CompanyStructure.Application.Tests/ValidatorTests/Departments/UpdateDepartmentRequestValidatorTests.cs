using CompanyStructure.Application.Features.Departments.Dtos;
using CompanyStructure.Application.Features.Departments.Validators;
using FluentValidation.TestHelper;

namespace CompanyStructure.Application.Tests.ValidatorTests.Departments
{
    public class UpdateDepartmentRequestValidatorTests
    {
        private readonly UpdateDepartmentRequestValidator _validator = new();

        [Fact]
        public void Should_HaveError_When_NameIsEmpty()
        {
            var request = new UpdateDepartmentRequest
            {
                Name = string.Empty,
                Code = "BE"
            };

            var result = _validator.TestValidate(request);

            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Should_HaveError_When_NameIsNull()
        {
            var request = new UpdateDepartmentRequest
            {
                Name = null!,
                Code = "BE"
            };

            var result = _validator.TestValidate(request);

            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Should_HaveError_When_CodeIsEmpty()
        {
            var request = new UpdateDepartmentRequest
            {
                Name = "Backend",
                Code = string.Empty
            };

            var result = _validator.TestValidate(request);

            result.ShouldHaveValidationErrorFor(x => x.Code);
        }

        [Fact]
        public void Should_HaveError_When_CodeIsNull()
        {
            var request = new UpdateDepartmentRequest
            {
                Name = "Backend",
                Code = null!
            };

            var result = _validator.TestValidate(request);

            result.ShouldHaveValidationErrorFor(x => x.Code);
        }

        [Fact]
        public void Should_NotHaveError_When_ManagerIdIsNull()
        {
            var request = new UpdateDepartmentRequest
            {
                Name = "Backend",
                Code = "BE",
                ManagerId = null
            };

            var result = _validator.TestValidate(request);

            result.ShouldNotHaveValidationErrorFor(x => x.ManagerId);
        }

        [Fact]
        public void Should_NotHaveError_When_ManagerIdIsProvided()
        {
            var request = new UpdateDepartmentRequest
            {
                Name = "Backend",
                Code = "BE",
                ManagerId = Guid.NewGuid()
            };

            var result = _validator.TestValidate(request);

            result.ShouldNotHaveValidationErrorFor(x => x.ManagerId);
        }
    }
}
