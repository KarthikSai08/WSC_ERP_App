using FluentValidation;
using WSC.Shared.Contracts.Dtos.DeliveryLayer;

namespace WSC.Delivery.Application.Validators.DeliveryItemValidators
{
    public class CreateDeliveryItemValidator : AbstractValidator<CreateDeliveryItemDto>
    {
        public CreateDeliveryItemValidator()
        {
            RuleFor(x => x.DeliveryId)
                .GreaterThan(0).WithMessage("Valid delivery ID is required.");

            RuleFor(x => x.ProductId)
                .GreaterThan(0).WithMessage("Valid product ID is required.");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than 0.");
        }
    }
}
