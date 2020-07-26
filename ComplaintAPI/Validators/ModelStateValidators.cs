using System;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ComplaintAPI.Validators
{
    public class ModelStateValidators:Attribute, IActionFilter
    {
        private IApiResponse _response;
        public ModelStateValidators(IApiResponse response)
        {
            _response = response;
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
           bool isModelStateValid = context.ModelState.IsValid;

           if (!isModelStateValid)
           {
               var errors = context.ModelState.Values.Select(x => x.Errors.Select(y => y.ErrorMessage));
               string errorMessage = "";
               foreach (var error in errors)
               {
                   errorMessage = errorMessage + ";" + error;
               }

               context.HttpContext.Response.ContentType = "application/json";
               context.HttpContext.Response.StatusCode = (int) HttpStatusCode.BadRequest;

               var response = _response.GetApiResponse("parameter validation failed",
                   context.HttpContext.Response.StatusCode.ToString(), errorMessage, null);

               context.HttpContext.Response.WriteAsync(response.ToString());
           }
           
        }
    }
}