using FluentValidation;
using WSC.Delivery.Application.Dtos;

namespace WSC.Delivery.Application.Validators.DeliveryTrackingValidators
{
    public class CreateDeliveryTrackingValidator : AbstractValidator<CreateDeliveryTrackingDto>
    {
        public CreateDeliveryTrackingValidator()
        {
            RuleFor(x => x.DeliveryId)
                .GreaterThan(0).WithMessage("Valid delivery ID is required.");

            RuleFor(x => x.Status)
                .IsInEnum().WithMessage("Invalid delivery status.");

            RuleFor(x => x.Location)
                .NotEmpty().WithMessage("Location is required.")
                .MaximumLength(255).WithMessage("Location cannot exceed 255 characters.");

            RuleFor(x => x.Remarks)
                .MaximumLength(500).WithMessage("Remarks cannot exceed 500 characters.")
                .When(x => !string.IsNullOrWhiteSpace(x.Remarks));
        }
    }
}
