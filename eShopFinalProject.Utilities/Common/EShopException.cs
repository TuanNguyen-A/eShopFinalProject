using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using ExceptionFilterAttribute = Microsoft.AspNetCore.Mvc.Filters.ExceptionFilterAttribute;

namespace eShopFinalProject.Utilities.Common
{
    public class EShopException : Exception
    {
        public HttpStatusCode StatusCode { get; } = HttpStatusCode.InternalServerError;
        public object Dto { get; set; }

        public EShopException(string message, HttpStatusCode statusCode = HttpStatusCode.InternalServerError, Exception innerException = null)
            : base(message, innerException)
        {
            StatusCode = statusCode;
        }

        public static EShopException BadRequest(string message = null, Exception innerException = null, string resourceName = null)
            => string.IsNullOrEmpty(resourceName) ? new EShopException(message ?? "Invalid Request Parameter", HttpStatusCode.BadRequest, innerException: innerException)
            : new EShopException(string.Format(message ?? "Invalid {0} Request Parameter", resourceName), HttpStatusCode.BadRequest, innerException: innerException);

        public static EShopException NotFound(string message = null, Exception innerException = null, string resourceName = null)
            => string.IsNullOrEmpty(resourceName) ? new EShopException(message ?? "Resource Is Not Found", HttpStatusCode.NotFound, innerException: innerException)
            : new EShopException(string.Format(message ?? "{0} Is Not Found", resourceName), HttpStatusCode.NotFound, innerException: innerException);

    }
    
    public class EShopExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public void OnException(HttpActionExecutedContext context)
        {
            if(context.Exception is EShopException exception)
            {
                if (exception.Dto != null)
                    context.Response = context.Request.CreateResponse(exception.StatusCode, exception.Dto);
                else
                    context.Response = context.Request.CreateResponse(exception.StatusCode, "Working");
            }
        }
    }

}
