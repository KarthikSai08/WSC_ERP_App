namespace WSC.Gateway.Application.Dtos.AuthDtos
{
    public record LoginRequestDto
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
