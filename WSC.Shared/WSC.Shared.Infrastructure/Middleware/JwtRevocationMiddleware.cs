using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using WSC.Shared.Contracts.Interfaces.JwtInterfaces;

namespace WSC.Shared.Infrastructure.Middleware
{
    public class JwtRevocationMiddleware
    {
        private readonly RequestDelegate _next;
        public JwtRevocationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IJwtBlocklistService blocklistService)
        {

            if (context.User.Identity?.IsAuthenticated == true)
            {
                var jti = context.User.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;
                if (!string.IsNullOrEmpty(jti) && await blocklistService.IsBlockedAsync(jti))
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync("{\"success\": false, \"message\": \"Token has been revoked. Please Login again\"}");
                    return;
                }
            }
            await _next(context);
        }
    }
}
