using FluentValidation;
using WSC.CRM.Application.Dtos;

namespace WSC.CRM.Application.Validators.CustomerValidators
{
    public class CreateCustomerValidator : AbstractValidator<CreateCustomerDto>
    {
        public CreateCustomerValidator()
        {
            RuleFor(x => x.CxName)
                .NotEmpty().WithMessage("Customer name is required.")
                .MaximumLength(100).WithMessage("Customer name cannot exceed 100 characters.");

            RuleFor(x => x.CxEmail)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.CxPhone)
                .NotEmpty().WithMessage("Phone number is required.")
                .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Invalid phone number format.");
        }
    }
}
