using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace SeaBattle.Api.Abstract
{
    public interface IHttpExceptionHandler
    {
        bool CanHandle(Exception exception);
        Task Handle(HttpContext context, Exception exception);
    }
}
