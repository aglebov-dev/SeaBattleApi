using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SeaBattle.Api.Abstract;
using SeaBattle.Api.Contracts;
using SeaBattle.Api.Extensions;
using SeaBattle.Domain.Exceptions;
using System;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;

namespace SeaBattle.Api.Infrastructure.ExceptionHandlers
{
    public class SimpleExceptionHandler : IHttpExceptionHandler
    {
        private static readonly JsonSerializerSettings _settings = new JsonSerializerSettings().ApplyDefaultJsonSettings();

        public bool CanHandle(Exception exception)
        {
            return 
                exception is InvalidGameOperationException ||
                exception is NotFoundException;
        }

        public async Task Handle(HttpContext context, Exception exception)
        {
            context.Response.ContentType = MediaTypeNames.Application.Json;
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            ErrorResponse response = new ErrorResponse
            {
                Message = exception.Message
            };

            string json = JsonConvert.SerializeObject(response, _settings);
            await context.Response.WriteAsync(json);
        }
    }
}
