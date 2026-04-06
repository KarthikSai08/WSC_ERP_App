using FluentValidation;
using WSC.Store.Application.Dtos;

namespace WSC.Store.Application.Validators.ProductValidator
{
    internal class CreateProductValidator : AbstractValidator<CreateProductDto>
    {
        public CreateProductValidator()
        {
            RuleFor(p => p.ProductName)
                .NotEmpty().WithMessage("Product name is required.")
                .MaximumLength(100).WithMessage("Product name cannot exceed 100 characters.");

            RuleFor(p => p.SKU)
                .NotEmpty().WithMessage("SKU is required.")
                .MaximumLength(50).WithMessage("SKU cannot exceed 50 characters.");

            RuleFor(p => p.Price)
                .GreaterThan(0).WithMessage("Price must be greater than zero.");
        }
    }
}
