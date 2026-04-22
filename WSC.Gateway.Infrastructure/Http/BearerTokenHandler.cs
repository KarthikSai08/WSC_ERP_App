using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;

namespace WSC.Gateway.Infrastructure.Http
{
    public class BearerTokenHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _http;
        public BearerTokenHandler(IHttpContextAccessor http)
        {
            _http = http;
        }
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = _http.HttpContext?.Request.Headers["Authorization"].FirstOrDefault();

            if (!string.IsNullOrEmpty(token))
            {
                var rawToken = token.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase)
                    ? token["Bearer ".Length..]
                    : token;

                request.Headers.Authorization =
                    new AuthenticationHeaderValue("Bearer", rawToken);
            }
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
