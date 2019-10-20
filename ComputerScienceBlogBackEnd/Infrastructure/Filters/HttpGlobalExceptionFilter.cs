using ComputerScienceBlogBackEnd.Infrastructure.ActionResults;
using ComputerScienceBlogBackEnd.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Net;

namespace ComputerScienceBlogBackEnd.Infrastructure.Filters
{
    public class HttpGlobalExceptionFilter : Attribute, IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is RequestedResourceHasConflictException)
            {
                var problemDetails = new ValidationProblemDetails()
                {
                    Instance = context.HttpContext.Request.Path,
                    Status = StatusCodes.Status409Conflict,
                };

                problemDetails.Errors.Add("DomainValidations", new string[] { context.Exception.Message.ToString() });
                context.Result = new ConflictObjectResult(problemDetails);
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Conflict;
            }
            else if (context.Exception is RequestedResourceNotFoundException)
            {
                var problemDetails = new ValidationProblemDetails()
                {
                    Instance = context.HttpContext.Request.Path,
                    Status = StatusCodes.Status404NotFound
                };

                problemDetails.Errors.Add("DomainValidations", new string[] { context.Exception.Message.ToString() });
                context.Result = new NotFoundObjectResult(problemDetails);
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
            }
            else
            {
                var json = new JsonErrorResponse
                {
                    Messages = new[] { "An error ocurred." }
                };

                context.Result = new InternalServerErrorObjectResult(json);
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
            context.ExceptionHandled = true;
        }

        private class JsonErrorResponse
        {
            public string[] Messages { get; set; }

            public object DeveloperMessage { get; set; }
        }
    }
}
