using System.Security.Claims;

namespace WSC.Shared.Contracts.Interfaces.JwtInterfaces
{
    public interface IJwtService
    {
        string GenerateAccessToken(int userId, string userName, string email, string role);
        string GenerateRefreshToken();
        bool ValidateRefreshToken(string refreshToken);
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
    }
}
