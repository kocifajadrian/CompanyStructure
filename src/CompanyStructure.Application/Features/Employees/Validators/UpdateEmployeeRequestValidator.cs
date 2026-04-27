using CompanyStructure.Application.Features.Employees.Dtos;
using FluentValidation;

namespace CompanyStructure.Application.Features.Employees.Validators
{
    public class UpdateEmployeeRequestValidator : AbstractValidator<UpdateEmployeeRequest>
    {
        public UpdateEmployeeRequestValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("FirstName is required.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("LastName is required.");

            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("Email must be a valid email address.")
                .When(x => !string.IsNullOrEmpty(x.Email));
        }
    }
}
