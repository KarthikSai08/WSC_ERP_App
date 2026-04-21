namespace WSC.Gateway.Application.Dtos.AuthDtos
{
    public record LoginResponseDto
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpiresAt { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public int UserId { get; set; }
    }
}
