using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using WSC.CRM.Application.Dtos;

namespace WSC.CRM.Application.Validators.LeadValidators
{
    public  class CreateLeadValidator : AbstractValidator<CreateLeadDto>
    {
        public CreateLeadValidator() 
        {
            RuleFor(x => x.LeadName)
                .NotEmpty().WithMessage("Lead name is required.")
                .MaximumLength(100).WithMessage("Lead name cannot exceed 100 characters.");

            RuleFor(x => x.LeadEmail)
                .NotEmpty().WithMessage("Lead email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.LeadPhone)
                .NotEmpty().WithMessage("Lead phone number is required.")
                .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Invalid phone number format.");

            RuleFor(x => x.CustomerId)
                .GreaterThan(0).WithMessage("Customer ID must be greater than zero.");
        }
    }
}
