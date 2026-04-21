using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace WSC.Shared.Contracts.Interfaces.JwtInterfaces
{
    public interface IJwtService
    {
        string GenerateAccessToken(string userId, string userName, string email, string role);
        string GenerateRefreshToken();  
        bool ValidateRefreshToken(string refreshToken);
         ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
    }
}
