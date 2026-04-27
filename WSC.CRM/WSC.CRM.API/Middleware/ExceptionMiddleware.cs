using System.Net;
using System.Text.Json;
using WSC.Shared.Contracts.Common;
using WSC.Shared.Contracts.Exceptions;

namespace WSC.Store.API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred: {ExceptionMessage}", ex.Message);

            var res = context.Response;
            res.ContentType = "application/json";

            var result = ex switch
            {
                NotFoundException nf => CreateResponse(HttpStatusCode.NotFound, nf.Message),
                DuplicateException d => CreateResponse(HttpStatusCode.Conflict, d.Message),
                InSufficientException i => CreateResponse(HttpStatusCode.BadRequest, i.Message),
                InActiveException ia => CreateResponse(HttpStatusCode.BadRequest, ia.Message),
                InvalidInputIdException ii => CreateResponse(HttpStatusCode.BadRequest, ii.Message),

                _ => CreateResponse(HttpStatusCode.InternalServerError, "Something went wrong. Please try again later.")
            };

            res.StatusCode = (int)result.code;

            var response = JsonSerializer.Serialize(result.Body, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await res.WriteAsync(response);
        }

        private (HttpStatusCode code, ApiResponse<string> Body) CreateResponse(HttpStatusCode code, string message)
        {
            return (code, ApiResponse<string>.Failed(message));
        }
    }
}

