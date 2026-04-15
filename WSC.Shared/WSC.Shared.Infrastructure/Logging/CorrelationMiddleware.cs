using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Logging;
using Serilog.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace WSC.Shared.Infrastructure.Logging
{
	public class CorrelationMiddleware
	{
		private readonly RequestDelegate _next;
		private const string Header = "X-Correlation-ID";

		public CorrelationMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			var correlationId = context.Request.Headers[Header].FirstOrDefault() 
									?? Guid.NewGuid().ToString();  

			context.TraceIdentifier = correlationId;
			context.Response.Headers[Header] = correlationId;

			using(LogContext.PushProperty("correlationId", correlationId))
			{
				await _next(context);
			}
        }
	}
}
