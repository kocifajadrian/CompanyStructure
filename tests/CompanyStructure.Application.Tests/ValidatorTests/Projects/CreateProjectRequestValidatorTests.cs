using CompanyStructure.Application.Features.Projects.Dtos;
using CompanyStructure.Application.Features.Projects.Validators;
using FluentValidation.TestHelper;

namespace CompanyStructure.Application.Tests.ValidatorTests.Projects
{
    public class CreateProjectRequestValidatorTests
    {
        private readonly CreateProjectRequestValidator _validator = new();

        [Fact]
        public void Should_HaveError_When_NameIsEmpty()
        {
            var request = new CreateProjectRequest
            {
                Name = string.Empty,
                Code = "APP",
                DivisionId = Guid.NewGuid()
            };

            var result = _validator.TestValidate(request);

            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Should_HaveError_When_NameIsNull()
        {
            var request = new CreateProjectRequest
            {
                Name = null!,
                Code = "APP",
                DivisionId = Guid.NewGuid()
            };

            var result = _validator.TestValidate(request);

            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Should_HaveError_When_CodeIsEmpty()
        {
            var request = new CreateProjectRequest
            {
                Name = "Application",
                Code = string.Empty,
                DivisionId = Guid.NewGuid()
            };

            var result = _validator.TestValidate(request);

            result.ShouldHaveValidationErrorFor(x => x.Code);
        }

        [Fact]
        public void Should_HaveError_When_CodeIsNull()
        {
            var request = new CreateProjectRequest
            {
                Name = "Application",
                Code = null!,
                DivisionId = Guid.NewGuid()
            };

            var result = _validator.TestValidate(request);

            result.ShouldHaveValidationErrorFor(x => x.Code);
        }

        [Fact]
        public void Should_HaveError_When_DivisionIdIsEmpty()
        {
            var request = new CreateProjectRequest
            {
                Name = "Application",
                Code = "APP",
                DivisionId = Guid.Empty
            };

            var result = _validator.TestValidate(request);

            result.ShouldHaveValidationErrorFor(x => x.DivisionId);
        }

        [Fact]
        public void Should_NotHaveError_When_ManagerIdIsNull()
        {
            var request = new CreateProjectRequest
            {
                Name = "Application",
                Code = "APP",
                DivisionId = Guid.NewGuid(),
                ManagerId = null
            };

            var result = _validator.TestValidate(request);

            result.ShouldNotHaveValidationErrorFor(x => x.ManagerId);
        }

        [Fact]
        public void Should_NotHaveError_When_ManagerIdIsProvided()
        {
            var request = new CreateProjectRequest
            {
                Name = "Application",
                Code = "APP",
                DivisionId = Guid.NewGuid(),
                ManagerId = Guid.NewGuid()
            };

            var result = _validator.TestValidate(request);

            result.ShouldNotHaveValidationErrorFor(x => x.ManagerId);
        }
    }
}
