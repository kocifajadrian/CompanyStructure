using CompanyStructure.Application.Features.Companies.Dtos;
using FluentValidation;

namespace CompanyStructure.Application.Features.Companies.Validators
{
    public class UpdateCompanyRequestValidator : AbstractValidator<UpdateCompanyRequest>
    {
        public UpdateCompanyRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.");

            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("Code is required.");
        }
    }
}
