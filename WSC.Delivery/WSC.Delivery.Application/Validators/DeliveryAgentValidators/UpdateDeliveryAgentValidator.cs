using FluentValidation;
using WSC.Delivery.Application.Dtos;

namespace WSC.Delivery.Application.Validators.DeliveryAgentValidators
{
    public class UpdateDeliveryAgentValidator : AbstractValidator<UpdateDeliveryAgentDto>
    {
        public UpdateDeliveryAgentValidator()
        {
            RuleFor(x => x.AgentId)
                .GreaterThan(0).WithMessage("Invalid agent ID.");

            RuleFor(x => x.Name)
                .MaximumLength(100).WithMessage("Agent name cannot exceed 100 characters.")
                .When(x => !string.IsNullOrWhiteSpace(x.Name));

            RuleFor(x => x.Phone)
                .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Invalid phone number format.")
                .When(x => !string.IsNullOrWhiteSpace(x.Phone));

            RuleFor(x => x.VehicleNumber)
                .MaximumLength(50).WithMessage("Vehicle number cannot exceed 50 characters.")
                .When(x => !string.IsNullOrWhiteSpace(x.VehicleNumber));
        }
    }
}
