using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using WSC.CRM.Application.Dtos;

namespace WSC.CRM.Application.Validators.LeadValidators
{
    public class UpdateLeadValidator : AbstractValidator<UpdateLeadDto>
    {
        public UpdateLeadValidator()
        {
            RuleFor(x => x.LeadId)
                .GreaterThan(0).WithMessage("Lead ID must be greater than zero.");

            RuleFor(x => x.LeadName)
                .MaximumLength(100).WithMessage("Lead name cannot exceed 100 characters.");

            RuleFor(x => x.LeadEmail)
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.LeadPhone)
                .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Invalid phone number format.");
        }
    }
}
