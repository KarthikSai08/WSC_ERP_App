using FluentValidation;
using WSC.Shared.Contracts.Dtos.DeliveryLayer;

namespace WSC.Delivery.Application.Validators.OrderDeliveryValidators
{
    public class CreateOrderDeliveryValidator : AbstractValidator<CreateOrderDeliveryDto>
    {
        public CreateOrderDeliveryValidator()
        {
            RuleFor(x => x.OrderId)
                .GreaterThan(0).WithMessage("Valid order ID is required.");

            RuleFor(x => x.CustomerId)
                .GreaterThan(0).WithMessage("Valid customer ID is required.");

            RuleFor(x => x.TrackingNumber)
                .NotEmpty().WithMessage("Tracking number is required.")
                .MaximumLength(100).WithMessage("Tracking number cannot exceed 100 characters.");

            RuleFor(x => x.ScheduledDate)
                .GreaterThan(DateTime.UtcNow).WithMessage("Scheduled date must be in the future.");

            RuleFor(x => x.DeliveryAddress)
                .NotEmpty().WithMessage("Delivery address is required.")
                .MaximumLength(500).WithMessage("Delivery address cannot exceed 500 characters.");
        }
    }
}
