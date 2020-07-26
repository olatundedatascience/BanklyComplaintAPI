using System;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BankyAuthServer
{
    public class HandleException:Attribute, IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            context.HttpContext.Response.ContentType = "application/json";
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            if (!context.ExceptionHandled && context.Exception is Exception ex)
            {
                context.HttpContext.Response.ContentType = "application/json";
                context.HttpContext.Response.WriteAsync(new
                {
                    StatusCode = context.HttpContext.Response.StatusCode,
                    message = "something went wrong",
                    description= ex.Message
                }.ToString());
            }

        }
    }
}