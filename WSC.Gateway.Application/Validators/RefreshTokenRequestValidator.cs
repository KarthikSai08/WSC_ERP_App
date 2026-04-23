using FluentValidation;
using WSC.Gateway.Application.Dtos.AuthDtos;

namespace WSC.Gateway.Application.Validators
{
    public class RefreshTokenRequestValidator : AbstractValidator<RefreshTokenRequestDto>
    {
        public RefreshTokenRequestValidator()
        {
            RuleFor(x => x.RefreshToken)
                .NotEmpty().WithMessage("Refresh token is required.");
        }
    }
}
