using CompanyStructure.Application.Features.Employees.Dtos;
using FluentValidation;

namespace CompanyStructure.Application.Features.Employees.Validators
{
    public class CreateEmployeeRequestValidator : AbstractValidator<CreateEmployeeRequest>
    {
        public CreateEmployeeRequestValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("FirstName is required.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("LastName is required.");

            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("Email must be a valid email address.")
                .When(x => !string.IsNullOrEmpty(x.Email));

            RuleFor(x => x.CompanyId)
                .NotNull().WithMessage("CompanyId is required.")
                .NotEmpty().WithMessage("CompanyId is required.");
        }
    }
}
