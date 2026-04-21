namespace WSC.Gateway.Application.Dtos.AggregatorDtos
{
    public record UserProfileDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }

    }
}
