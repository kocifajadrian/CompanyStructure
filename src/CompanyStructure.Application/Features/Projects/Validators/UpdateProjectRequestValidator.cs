using CompanyStructure.Application.Features.Projects.Dtos;
using FluentValidation;

namespace CompanyStructure.Application.Features.Projects.Validators
{
    public class UpdateProjectRequestValidator : AbstractValidator<UpdateProjectRequest>
    {
        public UpdateProjectRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.");

            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("Code is required.");
        }
    }
}
