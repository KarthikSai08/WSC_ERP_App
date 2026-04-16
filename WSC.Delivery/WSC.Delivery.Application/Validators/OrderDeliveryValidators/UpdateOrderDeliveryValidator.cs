using FluentValidation;
using WSC.Delivery.Application.Dtos;

namespace WSC.Delivery.Application.Validators.OrderDeliveryValidators
{
    public class UpdateOrderDeliveryValidator : AbstractValidator<UpdateOrderDeliveryDto>
    {
        public UpdateOrderDeliveryValidator()
        {
            RuleFor(x => x.DeliveryId)
                .GreaterThan(0).WithMessage("Valid delivery ID is required.");

            RuleFor(x => x.TrackingNumber)
                .MaximumLength(100).WithMessage("Tracking number cannot exceed 100 characters.")
                .When(x => !string.IsNullOrWhiteSpace(x.TrackingNumber));

            RuleFor(x => x.ScheduledDate)
                .GreaterThan(DateTime.UtcNow).WithMessage("Scheduled date must be in the future.")
                .When(x => x.ScheduledDate.HasValue);
        }
    }
}
