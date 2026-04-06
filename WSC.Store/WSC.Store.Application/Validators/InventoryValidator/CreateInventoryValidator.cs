using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using WSC.Store.Application.Dtos;

namespace WSC.Store.Application.Validators.InventoryValidator
{
    public class CreateInventoryValidator : AbstractValidator<CreateInventoryRecordDto>
    {
        public CreateInventoryValidator() 
        {
         RuleFor(i => i.ProductId)
                .NotEmpty()
                .WithMessage("ProductId is required.")
                .GreaterThan(0)
                .WithMessage("Product id should be greater than 0 and cannot be negative");

        RuleFor(i => i.InStock)
                .GreaterThanOrEqualTo(0)
                .WithMessage("InStock cannot be negative.");

        RuleFor(i => i.MinStock)
                .GreaterThan(0)
                .WithMessage("MinStock cannot be negative nor Zero.");

        }
    }
}
