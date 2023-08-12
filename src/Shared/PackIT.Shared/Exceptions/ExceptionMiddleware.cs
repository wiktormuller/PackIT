using Microsoft.AspNetCore.Http;
using PackIT.Shared.Abstractions.Exceptions;
using System.Text.Json;

namespace PackIT.Shared.Exceptions
{
    internal sealed class ExceptionMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
			try
			{
				await next(context);
			}
			catch (PackItException exception)
			{
				context.Response.StatusCode = 400;
				context.Response.Headers.Add("content-type", "application/json");

				var exceptionName = exception.GetType().Name.Replace("Exception", string.Empty);

                var errorCode = ToUnderscoreCase(exceptionName);
                var json = JsonSerializer.Serialize(
                    new
                    {
                        ErrorCode = errorCode,
                        ErrorMessage = exception.Message
                    }
                );

                await context.Response.WriteAsync(json);
            }
        }
        public static string ToUnderscoreCase(string value)
            => string.Concat((value ?? string.Empty).Select((x, i) => i > 0 && char.IsUpper(x) && !char.IsUpper(value[i - 1]) 
                ? $"_{x}" 
                : x.ToString())).ToLower();
    }
}
