using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;


namespace DepCalcsCS
{
    public class ExceptionMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (System.Exception e)
            {
                switch (e.Message)
                {
                    case("INVALID_GAAP_METHOD"):
                        context.Response.StatusCode = (int)HttpStatusCode.UnprocessableEntity;
                        await context.Response.WriteAsync("Invalid GAAP Method Provided.  Only SL, DB150, DB200 and SYD allowed");
                        break;
                    case("INVALID_TAX_METHOD"):
                        context.Response.StatusCode = (int)HttpStatusCode.UnprocessableEntity;
                        await context.Response.WriteAsync("Invalid Tax Method Provided.  Only MACRSHY and MACRSMQ allowed.");
                        break;
                    case("INVALID_TAX_YEAR"):
                        context.Response.StatusCode = (int)HttpStatusCode.UnprocessableEntity;
                        await context.Response.WriteAsync("Invalid Tax Year.  Only the following allowed: 3, 5, 7, 10, 15 & 20");
                        break;
                    default:
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        await context.Response.WriteAsync("Unhandled exception occurred");
                        break;
                }
            }
        }
    }
}