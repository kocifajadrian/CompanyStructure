using CompanyStructure.Application.Features.Divisions.Dtos;
using FluentValidation;

namespace CompanyStructure.Application.Features.Divisions.Validators
{
    public class UpdateDivisionRequestValidator : AbstractValidator<UpdateDivisionRequest>
    {
        public UpdateDivisionRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.");

            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("Code is required.");
        }
    }
}
