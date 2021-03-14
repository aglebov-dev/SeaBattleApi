using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SeaBattle.Api.Abstract;
using SeaBattle.Api.Contracts;
using SeaBattle.Api.Extensions;
using System;
using System.Net;
using System.Threading.Tasks;

namespace SeaBattle.Api.Infrastructure.ExceptionHandlers
{
    public class UnexpectedExceptionHandler : IHttpExceptionHandler
    {
        private static readonly JsonSerializerSettings _settings = new JsonSerializerSettings().ApplyDefaultJsonSettings();

        private readonly ILogger _logger;

        public UnexpectedExceptionHandler(ILogger<UnexpectedExceptionHandler> logger)
        {
            _logger = logger;
        }

        public bool CanHandle(Exception exception)
        {
            return true;
        }

        public async Task Handle(HttpContext context, Exception excption)
        {
            _logger.LogError(excption, excption.Message);

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            ErrorResponse response = new ErrorResponse
            {
                Message = "Something went wrong."
            };

            string json = JsonConvert.SerializeObject(response, _settings);
            await context.Response.WriteAsync(json);
        }
    }
}
