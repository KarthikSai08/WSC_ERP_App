namespace WSC.Shared.Contracts.Interfaces.JwtInterfaces
{
    public interface IJwtBlocklistService
    {
        Task BlockTokenAsync(string jti, TimeSpan remainingLifeTime);
        Task<bool> IsBlockedAsync(string jti);
    }
}
