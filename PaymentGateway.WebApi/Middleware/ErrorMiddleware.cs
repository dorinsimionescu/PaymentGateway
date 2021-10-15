using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace PaymentGateway.WebApi.Middleware
{
    public class ErrorMiddleware
    {
        private readonly RequestDelegate _next; // cors

        public ErrorMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }
    }
}

