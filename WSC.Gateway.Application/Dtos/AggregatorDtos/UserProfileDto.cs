using System;
using System.Collections.Generic;
using System.Text;

namespace WSC.Gateway.Application.Dtos
{
    public record UserProfileDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }

    }
}
