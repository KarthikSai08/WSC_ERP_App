using FluentValidation;
using WSC.CRM.Application.Dtos;

namespace WSC.CRM.Application.Validators.OpportunityValidators
{
    public class UpdateOpportunityValidator : AbstractValidator<UpdateOpportunityDto>
    {
        public UpdateOpportunityValidator()
        {
            RuleFor(x => x.OpportunityId)
                .GreaterThan(0).WithMessage("Opportunity ID must be a positive integer.");

            RuleFor(x => x.OpportunityName)
                .MaximumLength(100).WithMessage("Opportunity name cannot exceed 100 characters.");

            RuleFor(x => x.Stage)
                .IsInEnum().WithMessage("Invalid opportunity stage.");

            RuleFor(x => x.Amount)
                .GreaterThanOrEqualTo(0).WithMessage("Amount must be a positive value.");

        }
    }
}
