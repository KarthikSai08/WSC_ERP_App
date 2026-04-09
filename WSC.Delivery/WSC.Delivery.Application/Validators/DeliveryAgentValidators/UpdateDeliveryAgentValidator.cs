using FluentValidation;
using WSC.Delivery.Application.Dtos;

namespace WSC.Delivery.Application.Validators.DeliveryAgentValidators
{
    public class UpdateDeliveryAgentValidator : AbstractValidator<UpdateDeliveryAgentDto>
    {
        public UpdateDeliveryAgentValidator()
        {
            RuleFor(x => x.DeliveryAgentId)
                .GreaterThan(0).WithMessage("Invalid agent ID.");

            RuleFor(x => x.AgentName)
                .MaximumLength(100).WithMessage("Agent name cannot exceed 100 characters.")
                .When(x => !string.IsNullOrWhiteSpace(x.AgentName));

            RuleFor(x => x.AgentPhone)
                .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Invalid phone number format.")
                .When(x => !string.IsNullOrWhiteSpace(x.AgentPhone));

            RuleFor(x => x.VehicleNumber)
                .MaximumLength(50).WithMessage("Vehicle number cannot exceed 50 characters.")
                .When(x => !string.IsNullOrWhiteSpace(x.VehicleNumber));
        }
    }
}
