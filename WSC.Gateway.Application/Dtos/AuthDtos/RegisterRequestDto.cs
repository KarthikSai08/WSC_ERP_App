using System;
using System.Collections.Generic;
using System.Text;

namespace WSC.Gateway.Application.Dtos.AuthDtos
{
    public record RegisterRequestDto
    {
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Role { get; set; } = "Viewer";
    }
}
