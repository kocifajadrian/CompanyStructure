using CompanyStructure.Application.Features.Divisions.Dtos;
using CompanyStructure.Application.Features.Divisions.Validators;
using FluentValidation.TestHelper;

namespace CompanyStructure.Application.Tests.ValidatorTests.Divisions
{
    public class CreateDivisionRequestValidatorTests
    {
        private readonly CreateDivisionRequestValidator _validator = new();

        [Fact]
        public void Should_HaveError_When_NameIsEmpty()
        {
            var request = new CreateDivisionRequest
            {
                Name = string.Empty,
                Code = "SWDEV",
                CompanyId = Guid.NewGuid()
            };

            var result = _validator.TestValidate(request);

            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Should_HaveError_When_NameIsNull()
        {
            var request = new CreateDivisionRequest
            {
                Name = null!,
                Code = "SWDEV",
                CompanyId = Guid.NewGuid()
            };

            var result = _validator.TestValidate(request);

            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Should_HaveError_When_CodeIsEmpty()
        {
            var request = new CreateDivisionRequest
            {
                Name = "Software Development",
                Code = string.Empty,
                CompanyId = Guid.NewGuid()
            };

            var result = _validator.TestValidate(request);

            result.ShouldHaveValidationErrorFor(x => x.Code);
        }

        [Fact]
        public void Should_HaveError_When_CodeIsNull()
        {
            var request = new CreateDivisionRequest
            {
                Name = "Software Development",
                Code = null!,
                CompanyId = Guid.NewGuid()
            };

            var result = _validator.TestValidate(request);

            result.ShouldHaveValidationErrorFor(x => x.Code);
        }

        [Fact]
        public void Should_HaveError_When_CompanyIdIsEmpty()
        {
            var request = new CreateDivisionRequest
            {
                Name = "Software Development",
                Code = "SWDEV",
                CompanyId = Guid.Empty
            };

            var result = _validator.TestValidate(request);

            result.ShouldHaveValidationErrorFor(x => x.CompanyId);
        }

        [Fact]
        public void Should_NotHaveError_When_ManagerIdIsNull()
        {
            var request = new CreateDivisionRequest
            {
                Name = "Software Development",
                Code = "SWDEV",
                CompanyId = Guid.NewGuid(),
                ManagerId = null
            };

            var result = _validator.TestValidate(request);

            result.ShouldNotHaveValidationErrorFor(x => x.ManagerId);
        }

        [Fact]
        public void Should_NotHaveError_When_ManagerIdIsProvided()
        {
            var request = new CreateDivisionRequest
            {
                Name = "Software Development",
                Code = "SWDEV",
                CompanyId = Guid.NewGuid(),
                ManagerId = Guid.NewGuid()
            };

            var result = _validator.TestValidate(request);

            result.ShouldNotHaveValidationErrorFor(x => x.ManagerId);
        }
    }
}
