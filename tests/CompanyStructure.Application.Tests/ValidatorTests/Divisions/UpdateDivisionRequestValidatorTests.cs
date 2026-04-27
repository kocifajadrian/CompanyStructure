using CompanyStructure.Application.Features.Divisions.Dtos;
using CompanyStructure.Application.Features.Divisions.Validators;
using FluentValidation.TestHelper;

namespace CompanyStructure.Application.Tests.ValidatorTests.Divisions
{
    public class UpdateDivisionRequestValidatorTests
    {
        private readonly UpdateDivisionRequestValidator _validator = new();

        [Fact]
        public void Should_HaveError_When_NameIsEmpty()
        {
            var request = new UpdateDivisionRequest
            {
                Name = string.Empty,
                Code = "SWDEV"
            };

            var result = _validator.TestValidate(request);

            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Should_HaveError_When_NameIsNull()
        {
            var request = new UpdateDivisionRequest
            {
                Name = null!,
                Code = "SWDEV"
            };

            var result = _validator.TestValidate(request);

            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Should_HaveError_When_CodeIsEmpty()
        {
            var request = new UpdateDivisionRequest
            {
                Name = "Software Development",
                Code = string.Empty
            };

            var result = _validator.TestValidate(request);

            result.ShouldHaveValidationErrorFor(x => x.Code);
        }

        [Fact]
        public void Should_HaveError_When_CodeIsNull()
        {
            var request = new UpdateDivisionRequest
            {
                Name = "Software Development",
                Code = null!
            };

            var result = _validator.TestValidate(request);

            result.ShouldHaveValidationErrorFor(x => x.Code);
        }

        [Fact]
        public void Should_NotHaveError_When_ManagerIdIsNull()
        {
            var request = new UpdateDivisionRequest
            {
                Name = "Software Development",
                Code = "SWDEV",
                ManagerId = null
            };

            var result = _validator.TestValidate(request);

            result.ShouldNotHaveValidationErrorFor(x => x.ManagerId);
        }

        [Fact]
        public void Should_NotHaveError_When_ManagerIdIsProvided()
        {
            var request = new UpdateDivisionRequest
            {
                Name = "Software Development",
                Code = "SWDEV",
                ManagerId = Guid.NewGuid()
            };

            var result = _validator.TestValidate(request);

            result.ShouldNotHaveValidationErrorFor(x => x.ManagerId);
        }
    }
}
