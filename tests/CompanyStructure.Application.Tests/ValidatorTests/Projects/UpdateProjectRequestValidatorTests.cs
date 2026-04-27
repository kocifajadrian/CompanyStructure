using CompanyStructure.Application.Features.Projects.Dtos;
using CompanyStructure.Application.Features.Projects.Validators;
using FluentValidation.TestHelper;

namespace CompanyStructure.Application.Tests.ValidatorTests.Projects
{
    public class UpdateProjectRequestValidatorTests
    {
        private readonly UpdateProjectRequestValidator _validator = new();

        [Fact]
        public void Should_HaveError_When_NameIsEmpty()
        {
            var request = new UpdateProjectRequest
            {
                Name = string.Empty,
                Code = "APP"
            };

            var result = _validator.TestValidate(request);

            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Should_HaveError_When_NameIsNull()
        {
            var request = new UpdateProjectRequest
            {
                Name = null!,
                Code = "APP"
            };

            var result = _validator.TestValidate(request);

            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Should_HaveError_When_CodeIsEmpty()
        {
            var request = new UpdateProjectRequest
            {
                Name = "Application",
                Code = string.Empty
            };

            var result = _validator.TestValidate(request);

            result.ShouldHaveValidationErrorFor(x => x.Code);
        }

        [Fact]
        public void Should_HaveError_When_CodeIsNull()
        {
            var request = new UpdateProjectRequest
            {
                Name = "Application",
                Code = null!
            };

            var result = _validator.TestValidate(request);

            result.ShouldHaveValidationErrorFor(x => x.Code);
        }

        [Fact]
        public void Should_NotHaveError_When_ManagerIdIsNull()
        {
            var request = new UpdateProjectRequest
            {
                Name = "Application",
                Code = "APP",
                ManagerId = null
            };

            var result = _validator.TestValidate(request);

            result.ShouldNotHaveValidationErrorFor(x => x.ManagerId);
        }

        [Fact]
        public void Should_NotHaveError_When_ManagerIdIsProvided()
        {
            var request = new UpdateProjectRequest
            {
                Name = "Application",
                Code = "APP",
                ManagerId = Guid.NewGuid()
            };

            var result = _validator.TestValidate(request);

            result.ShouldNotHaveValidationErrorFor(x => x.ManagerId);
        }
    }
}
