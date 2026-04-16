using System;
using System.Collections.Generic;
using System.Text;

namespace WSC.Shared.Contracts.Interfaces
{
    public interface IIdempotencyService
    {
        Task<string?> GetResponseAsync(string key, CancellationToken ct);
        Task SetResponseAsync(string key, string response, TimeSpan ttl, CancellationToken ct);
    }
}
