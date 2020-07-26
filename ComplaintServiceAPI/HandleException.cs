using System;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ComplaintServiceAPI
{
    public class HandleException:Attribute, IExceptionFilter
    {
        private ILoggerManager _log;
        public HandleException(ILoggerManager log)
        {
            _log = log;
        }
        public void OnException(ExceptionContext context)
        {
            context.HttpContext.Response.ContentType = "application/json";
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            if (!context.ExceptionHandled && context.Exception is Exception ex)
            {
                context.HttpContext.Response.ContentType = "application/json";
                
                _log.LogError(ex.Message);
                context.HttpContext.Response.WriteAsync(new
                {
                    StatusCode = context.HttpContext.Response.StatusCode,
                    message = "something went wrong",
                    description= "Check File Console...."
                }.ToString());
            }

        }
    }
}