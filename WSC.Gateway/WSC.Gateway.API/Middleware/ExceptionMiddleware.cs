using Microsoft.AspNetCore.Mvc.ApiExplorer;
using System.Net;
using System.Text.Json;
using WSC.Shared.Contracts.Common;
using WSC.Shared.Contracts.Exceptions;

namespace WSC.Gateway.API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        public ExceptionMiddleware(RequestDelegate next) => _next = next;

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleException(context, ex);
            }
            
        }
        private async Task HandleException(HttpContext context, Exception ex)
        {
            var res = context.Response;
            res.ContentType = "application/json";

            var result = ex switch 
            { 
              // Existing exceptions from your shared contracts
                NotFoundException nf => CreateResponse(HttpStatusCode.NotFound, nf.Message),
                DuplicateException d         => CreateResponse(HttpStatusCode.Conflict, d.Message),
                InSufficientException i      => CreateResponse(HttpStatusCode.BadRequest, i.Message),
                InActiveException ia         => CreateResponse(HttpStatusCode.BadRequest, ia.Message),
                InvalidInputIdException ii   => CreateResponse(HttpStatusCode.BadRequest, ii.Message),

                // New auth exceptions
                InvalidCredentialsException => CreateResponse(HttpStatusCode.Unauthorized, ex.Message),
                UnauthorizedException ue     => CreateResponse(HttpStatusCode.Unauthorized, ue.Message),
                TokenExpiredException => CreateResponse(HttpStatusCode.Unauthorized, ex.Message),

                // Validation
                ArgumentException ae         => CreateResponse(HttpStatusCode.BadRequest, ae.Message),

                _ => CreateResponse(HttpStatusCode.InternalServerError,
                 "Something went wrong. Please try again later.")
            };
            
            res.StatusCode = (int)result.code;

            var response = JsonSerializer.Serialize(result.Body, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await res.WriteAsync(response);
        }
        private (HttpStatusCode code, ApiResponse<string> Body) CreateResponse(HttpStatusCode code, string message)
            => (code, ApiResponse<string>.Failed(message));
    }
}
