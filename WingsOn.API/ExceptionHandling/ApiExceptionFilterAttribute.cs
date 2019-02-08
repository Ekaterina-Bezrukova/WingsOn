using System;
using System.Net;
using log4net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WingsOn.API.ExceptionHandling
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class ApiExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly ILog _log;

        public ApiExceptionFilterAttribute(ILog log)
        {
            _log = log;
        }

        public override void OnException(ExceptionContext context)
        {
            _log.Error(context.Exception.Message, context.Exception);
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Result = new JsonResult(context.Exception.Message);
            base.OnException(context);
        }
    }
}
