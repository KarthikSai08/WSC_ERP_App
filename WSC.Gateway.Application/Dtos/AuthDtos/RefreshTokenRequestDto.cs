namespace WSC.Gateway.Application.Dtos.AuthDtos
{
    public record RefreshTokenRequestDto
    {
        public string RefreshToken { get; set; } = null!;

    }
}
