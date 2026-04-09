using FluentValidation;
using WSC.Delivery.Application.Dtos;

namespace WSC.Delivery.Application.Validators.DeliveryItemValidators
{
    public class UpdateDeliveryItemValidator : AbstractValidator<UpdateDeliveryItemDto>
    {
        public UpdateDeliveryItemValidator()
        {
            RuleFor(x => x.DeliveryItemId)
                .GreaterThan(0).WithMessage("Valid delivery item ID is required.");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than 0.");
        }
    }
}
