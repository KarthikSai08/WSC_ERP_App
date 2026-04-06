using FluentValidation;
using WSC.CRM.Application.Dtos;

namespace WSC.CRM.Application.Validators.ActivityValidators
{
    public class CreateActivityValidator : AbstractValidator<CreateActivityDto>
    {
        public CreateActivityValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Activity name is required.")
                .MaximumLength(100).WithMessage("Activity name cannot exceed 100 characters.");

            RuleFor(x => x.LeadId)
                .GreaterThan(0)
                .WithMessage("Valid Lead ID is required.");

            RuleFor(x => x.Type)
                .IsInEnum()
                .WithMessage("Invalid activity type.");
        }
    }
}
