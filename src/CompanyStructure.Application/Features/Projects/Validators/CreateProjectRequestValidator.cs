using CompanyStructure.Application.Features.Projects.Dtos;
using FluentValidation;

namespace CompanyStructure.Application.Features.Projects.Validators
{
    public class CreateProjectRequestValidator : AbstractValidator<CreateProjectRequest>
    {
        public CreateProjectRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.");

            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("Code is required.");

            RuleFor(x => x.DivisionId)
                .NotEmpty().WithMessage("DivisionId is required.");
        }
    }
}
