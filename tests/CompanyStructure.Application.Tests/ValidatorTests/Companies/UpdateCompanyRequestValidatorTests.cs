using CompanyStructure.Application.Features.Companies.Dtos;
using CompanyStructure.Application.Features.Companies.Validators;
using FluentValidation.TestHelper;

namespace CompanyStructure.Application.Tests.ValidatorTests.Companies
{
    public class UpdateCompanyRequestValidatorTests
    {
        private readonly UpdateCompanyRequestValidator _validator = new();

        [Fact]
        public void Should_HaveError_When_NameIsEmpty()
        {
            var request = new UpdateCompanyRequest
            {
                Name = string.Empty,
                Code = "SWCOMP"
            };

            var result = _validator.TestValidate(request);

            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Should_HaveError_When_NameIsNull()
        {
            var request = new UpdateCompanyRequest
            {
                Name = null!,
                Code = "SWCOMP"
            };

            var result = _validator.TestValidate(request);

            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Should_HaveError_When_CodeIsEmpty()
        {
            var request = new UpdateCompanyRequest
            {
                Name = "Software Company",
                Code = string.Empty
            };

            var result = _validator.TestValidate(request);

            result.ShouldHaveValidationErrorFor(x => x.Code);
        }

        [Fact]
        public void Should_HaveError_When_CodeIsNull()
        {
            var request = new UpdateCompanyRequest
            {
                Name = "Software Company",
                Code = null!
            };

            var result = _validator.TestValidate(request);

            result.ShouldHaveValidationErrorFor(x => x.Code);
        }

        [Fact]
        public void Should_NotHaveError_When_ManagerIdIsNull()
        {
            var request = new UpdateCompanyRequest
            {
                Name = "Software Company",
                Code = "SWCOMP",
                ManagerId = null
            };

            var result = _validator.TestValidate(request);

            result.ShouldNotHaveValidationErrorFor(x => x.ManagerId);
        }

        [Fact]
        public void Should_NotHaveError_When_ManagerIdIsProvided()
        {
            var request = new UpdateCompanyRequest
            {
                Name = "Software Company",
                Code = "SWCOMP",
                ManagerId = Guid.NewGuid()
            };

            var result = _validator.TestValidate(request);

            result.ShouldNotHaveValidationErrorFor(x => x.ManagerId);
        }
    }
}
