using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUDExample.Filters.ActionFilters
{
    public class ResponseHeaderFilterFactoryAttribute : Attribute, IFilterFactory
    {
        private readonly string? _key;
        private readonly string? _value;
        private readonly int _order;

        public ResponseHeaderFilterFactoryAttribute(string? key, string? value, int order)
        {
            _key = key;
            _value = value;
            _order = order;
        }

        public bool IsReusable => false;

        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            // return filter object
            var filter = serviceProvider.GetRequiredService<ResponseHeaderActionFilter>();
            filter.Key = _key;
            filter.Value = _value;
            filter.Order = _order;
            return filter;
        }
    }

    public class ResponseHeaderActionFilter : IAsyncActionFilter, IOrderedFilter
    {
        private readonly ILogger<ResponseHeaderActionFilter> _logger;

        //private readonly ILogger<ResponseHeaderActionFilter> _logger;
        public string? Key { get; set; }
        public string? Value { get; set; }
        public int Order { get; set; }

        //public ResponseHeaderActionFilter(string key, string value, int order)
        //{
        //    //_logger = logger;
        //    Key = key;
        //    Value = value;
        //    Order = order;
        //}
        public ResponseHeaderActionFilter(ILogger<ResponseHeaderActionFilter> logger)
        {
            _logger = logger;
        }
        //public void OnActionExecuting(ActionExecutingContext context)
        //{


        //}

        //public void OnActionExecuted(ActionExecutedContext context)
        //{

        //}


        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // TO DO: Before logic here
            _logger.LogInformation("{FilterName}.{MethodName} method - before", nameof(ResponseHeaderActionFilter), nameof(OnActionExecutionAsync));

            await next();

            _logger.LogInformation("{FilterName}.{MethodName} method - after", nameof(ResponseHeaderActionFilter), nameof(OnActionExecutionAsync));
            context.HttpContext.Response.Headers[Key ?? ""] = Value;

            // TO DO: After logic here
        }
    }
}
