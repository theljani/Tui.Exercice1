using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using TUI.Flights.Common.Exceptions;

namespace TUI.Flights.Web.Middlewares
{
    public class CustomCodeExceptionsMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomCodeExceptionsMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {

            try
            {
                await _next(httpContext);
            }
            catch (CustomCodeException ex)
            {
                if (httpContext.Response.HasStarted)
                {
                    throw;
                }

                httpContext.Response.Clear();
                httpContext.Response.StatusCode = ex.StatusCode;
                httpContext.Response.ContentType = ex.ContentType;

                await httpContext.Response.WriteAsync(ex.Message);

                return;
            }
            catch (Exception e)
            {
                await httpContext.Response.WriteAsync(e.Message);

                return;
            }
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class CustomCodeExceptionsMiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomCodeExceptionsMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CustomCodeExceptionsMiddleware>();
        }
    }
}
