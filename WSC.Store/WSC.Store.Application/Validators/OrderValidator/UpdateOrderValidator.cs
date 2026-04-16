using FluentValidation;
using WSC.Store.Application.Dtos;

namespace WSC.Store.Application.Validators.OrderValidator
{
    public class UpdateOrderValidator : AbstractValidator<UpdateOrderDto>
    {
        public UpdateOrderValidator()
        {
            RuleFor(x => x.OrderId)
                .GreaterThan(0).WithMessage("OrderId must be greater than 0.");

            RuleFor(x => x.CustomerId)
                .GreaterThan(0).WithMessage("CustomerId must be greater than 0.");

            RuleFor(x => x.TotalAmount)
                .GreaterThan(0).WithMessage("TotalAmount must be greater than 0.");

            RuleFor(x => x.Status)
                .IsInEnum().WithMessage("Status must be a valid enum value.");
        }
    }
}
