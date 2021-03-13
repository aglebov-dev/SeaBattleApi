using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SeaBattle.Api.Abstract;
using SeaBattle.Api.Contracts;
using SeaBattle.Api.Extensions;
using System;
using System.Net;
using System.Threading.Tasks;

namespace SeaBattle.Api.Infrastructure.ExceptionHandlers
{
    public class ArgumentNullExceptionHandler : IHttpExceptionHandler
    {
        private static readonly JsonSerializerSettings _settings = new JsonSerializerSettings().ApplyDefaultJsonSettings();

        public bool CanHandle(Exception exception)
        {
            return exception is ArgumentNullException;
        }

        public async Task Handle(HttpContext context, Exception exception)
        {
            // TODO execute this handler in develop mode only
            context.Response.StatusCode = (int)HttpStatusCode.UnprocessableEntity;
            ErrorResponse response = new ErrorResponse
            {
                Message = exception.ToString()
            };

            string json = JsonConvert.SerializeObject(response, _settings);
            await context.Response.WriteAsync(json);
        }
    }
}
