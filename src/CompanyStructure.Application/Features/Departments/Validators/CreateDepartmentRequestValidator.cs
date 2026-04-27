using CompanyStructure.Application.Features.Departments.Dtos;
using FluentValidation;

namespace CompanyStructure.Application.Features.Departments.Validators
{
    public class CreateDepartmentRequestValidator : AbstractValidator<CreateDepartmentRequest>
    {
        public CreateDepartmentRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.");

            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("Code is required.");

            RuleFor(x => x.ProjectId)
                .NotEmpty().WithMessage("ProjectId is required.");
        }
    }
}
