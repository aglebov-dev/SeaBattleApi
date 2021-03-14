using Microsoft.AspNetCore.Http;
using SeaBattle.Api.Abstract;
using SeaBattle.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeaBattle.Api.Infrastructure
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IList<IHttpExceptionHandler> _handlers;

        public ExceptionMiddleware(RequestDelegate next, IEnumerable<IHttpExceptionHandler> handlers)
        {
            _next = next.NotNull(nameof(next));
            _handlers = (handlers ?? Enumerable.Empty<IHttpExceptionHandler>()).ToList();
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                IHttpExceptionHandler handler = _handlers.Where(handler => handler.CanHandle(exception)).FirstOrDefault();
                if (handler is null || context.Response.HasStarted)
                {
                    throw;
                }
                else
                {
                    await handler.Handle(context, exception); 
                }
            }
        }
    }
}
