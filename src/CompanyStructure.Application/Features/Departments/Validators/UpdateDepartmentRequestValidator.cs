using CompanyStructure.Application.Features.Departments.Dtos;
using FluentValidation;

namespace CompanyStructure.Application.Features.Departments.Validators
{
    public class UpdateDepartmentRequestValidator : AbstractValidator<UpdateDepartmentRequest>
    {
        public UpdateDepartmentRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.");

            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("Code is required.");
        }
    }
}
