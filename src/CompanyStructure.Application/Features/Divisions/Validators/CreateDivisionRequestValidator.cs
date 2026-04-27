using CompanyStructure.Application.Features.Divisions.Dtos;
using FluentValidation;

namespace CompanyStructure.Application.Features.Divisions.Validators
{
    public class CreateDivisionRequestValidator : AbstractValidator<CreateDivisionRequest>
    {
        public CreateDivisionRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.");

            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("Code is required.");

            RuleFor(x => x.CompanyId)
                .NotEmpty().WithMessage("CompanyId is required.");
        }
    }
}
