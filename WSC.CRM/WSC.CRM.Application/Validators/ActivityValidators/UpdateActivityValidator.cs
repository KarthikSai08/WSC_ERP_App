using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using WSC.CRM.Application.Dtos;

namespace WSC.CRM.Application.Validators.ActivityValidators
{
    public  class UpdateActivityValidator : AbstractValidator<UpdateActivityDto>
    {
        public UpdateActivityValidator() 
        {
            RuleFor(x => x.ActivityId)
                .GreaterThan(0).WithMessage("ActivityId must be greater than 0.");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(100).WithMessage("Title cannot exceed 100 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");

            RuleFor(x => x.Type)
                .IsInEnum().WithMessage("Invalid activity type.");
        }
    }
}
