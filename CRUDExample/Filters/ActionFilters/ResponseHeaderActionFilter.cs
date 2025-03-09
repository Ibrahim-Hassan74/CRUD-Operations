using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUDExample.Filters.ActionFilters
{
    public class ResponseHeaderActionFilter : ActionFilterAttribute
    {
        //private readonly ILogger<ResponseHeaderActionFilter> _logger;
        private readonly string _key;
        private readonly string _value;

        public ResponseHeaderActionFilter(string key, string value, int order)
        {
            //_logger = logger;
            _key = key;
            _value = value;
            Order = order;
        }
        //public void OnActionExecuting(ActionExecutingContext context)
        //{


        //}

        //public void OnActionExecuted(ActionExecutedContext context)
        //{
            
        //}


        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // TO DO: Before logic here
            //_logger.LogInformation("{FilterName}.{MethodName} method - before", nameof(ResponseHeaderActionFilter), nameof(OnActionExecutionAsync));

            await next();

            //_logger.LogInformation("{FilterName}.{MethodName} method - after", nameof(ResponseHeaderActionFilter), nameof(OnActionExecutionAsync));
            context.HttpContext.Response.Headers[_key] = _value;

            // TO DO: After logic here
        }
    }
}
