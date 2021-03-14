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
    public class DataValidationExceptionHandler : IHttpExceptionHandler
    {
        private static readonly JsonSerializerSettings _settings = new JsonSerializerSettings().ApplyDefaultJsonSettings();

        public bool CanHandle(Exception exception)
        {
            return exception is DataValidationException;
        }

        public async Task Handle(HttpContext context, Exception exception)
        {
            string message = exception.Message;
            if (exception is DataValidationException ex)
            {
                message = string.Join(Environment.NewLine, ex.Errors);
            }

            ErrorResponse response = new ErrorResponse
            {
                Message = message
            };

            string json = JsonConvert.SerializeObject(response, _settings);
            context.Response.ContentType = MediaTypeNames.Application.Json;
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            await context.Response.WriteAsync(json);
        }
    }
}
