using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using WSC.CRM.Application.Dtos;

namespace WSC.CRM.Application.Validators.OpportunityValidators
{
    public class CreateOpportunityValidator : AbstractValidator<CreateOpportunityDto>
    {
        public CreateOpportunityValidator() 
        {
            RuleFor(x => x.OpportunityName)
                .NotEmpty().WithMessage("Opportunity name is required.")
                .MaximumLength(100).WithMessage("Opportunity name cannot exceed 100 characters.");

            RuleFor(x => x.Stage)
                .IsInEnum().WithMessage("Invalid opportunity stage.");
            
            RuleFor(x => x.Amount)
                .GreaterThanOrEqualTo(0).WithMessage("Amount must be a positive value.");

            RuleFor(x => x.CustomerId)
                .GreaterThan(0).WithMessage("Customer ID must be a positive integer.");
        }    
    }
}
