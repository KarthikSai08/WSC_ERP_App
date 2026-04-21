using WSC.Shared.Infrastructure.Services;

namespace WSC.CRM.API.Middleware

{
    public class IdempotencyMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<IdempotencyMiddleware> _logger;

        public IdempotencyMiddleware(RequestDelegate next, ILogger<IdempotencyMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, IdempotencyService service)
        {

            if (!(context.Request.Method == HttpMethod.Post.Method ||
                 context.Request.Method == HttpMethod.Put.Method ||
                 context.Request.Method == HttpMethod.Delete.Method ||
                 context.Request.Method == HttpMethod.Patch.Method))
            {
                await _next(context);
                return;
            }

            var key = context.Request.Headers["Idempotency-Key"].FirstOrDefault();

            if (!string.IsNullOrEmpty(key))
            {
                await _next(context);
                return;
            }

            var cacheKey = $"idem:{key}";

            var cachedResponse = await service.GetResponseAsync(cacheKey, context.RequestAborted);

            if (cachedResponse != null)
            {
                _logger.LogInformation("Returning cached response for idempotency key: {Key}", key);

                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(cachedResponse, context.RequestAborted);
                return;
            }

            var originalBody = context.Response.Body;

            using var newBody = new MemoryStream();
            context.Response.Body = newBody;

            await _next(context);

            newBody.Seek(0, SeekOrigin.Begin);
            var responseBody = await new StreamReader(newBody).ReadToEndAsync(context.RequestAborted);

            newBody.Seek(0, SeekOrigin.Begin);
            await newBody.CopyToAsync(originalBody, context.RequestAborted);

            await service.SetResponseAsync(cacheKey, responseBody, TimeSpan.FromMinutes(5), context.RequestAborted);

            _logger.LogInformation("Idempotency stored for key: {Key}", key);
        }
    }
}
