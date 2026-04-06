using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using WSC.CRM.Application.Dtos;

namespace WSC.CRM.Application.Validators.CustomerValidators
{
    public class UpdateCustomerValidator : AbstractValidator<UpdateCustomerDto>
    {
        public UpdateCustomerValidator()
        {
            RuleFor(x => x.CxId)
                .NotEmpty().WithMessage("Customer ID is required.");

            RuleFor(x => x.CxName)
                .NotEmpty().WithMessage("Customer name is required.")
                .MaximumLength(50).WithMessage("Customer name cannot exceed 50 characters.");

            RuleFor(x => x.CxEmail)
                .NotEmpty().WithMessage("Customer email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.CxPhone)
                .MaximumLength(20).WithMessage("Customer phone cannot exceed 20 characters.");
        }
    }
}
