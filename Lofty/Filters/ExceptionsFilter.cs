using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using ViewModels;

namespace WebApis.Filters
{
    /// <summary>
    /// For filter all exceptions
    /// </summary>
    public class ExceptionFilter : ExceptionFilterAttribute
    {
        /// <summary>
        /// Filtering exceptio function
        /// </summary>
        /// <param name="context"></param>
        public override void OnException(ExceptionContext context)
        {
            string exceptionMessage;
            if (context.Exception.InnerException != null)
            {
                exceptionMessage = context.Exception.InnerException.Message;
            }
            else
            {
                exceptionMessage = context.Exception.Message;

            }
            context.Result = new ObjectResult(new APIResult<string>()
            {
                Data = "",
                IsSucceed = false,
                Message = exceptionMessage,
                StatusCode = 500
            })
            {
                StatusCode = StatusCodes.Status500InternalServerError,
            };
            base.OnException(context);
        }
    }
}
