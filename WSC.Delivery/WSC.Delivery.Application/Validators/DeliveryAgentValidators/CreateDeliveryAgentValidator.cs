using FluentValidation;
using WSC.Delivery.Application.Dtos;

namespace WSC.Delivery.Application.Validators.DeliveryAgentValidators
{
    public class CreateDeliveryAgentValidator : AbstractValidator<CreateDeliveryAgentDto>
    {
        public CreateDeliveryAgentValidator()
        {
            RuleFor(x => x.AgentName)
                .NotEmpty().WithMessage("Agent name is required.")
                .MaximumLength(100).WithMessage("Agent name cannot exceed 100 characters.");

            RuleFor(x => x.AgentPhone)
                .NotEmpty().WithMessage("Agent phone is required.")
                .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Invalid phone number format.");

            RuleFor(x => x.VehicleNumber)
                .NotEmpty().WithMessage("Vehicle number is required.")
                .MaximumLength(50).WithMessage("Vehicle number cannot exceed 50 characters.");
        }
    }
}
