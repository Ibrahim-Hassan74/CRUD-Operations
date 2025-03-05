using CRUDExample.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using ServiceContracts.DTO;
using System.Numerics;
using System.Reflection;

namespace CRUDExample.Filters.ActionFilters
{
    public class PersonsListActionFilter : IActionFilter
    {
        private readonly ILogger<PersonsListActionFilter> _logger;

        public PersonsListActionFilter(ILogger<PersonsListActionFilter> logger)
        {
            _logger = logger;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // To do: add after logic here
            _logger.LogInformation("{FilterName}.{MethodName} method", nameof(PersonsListActionFilter), nameof(OnActionExecuted));

            PersonsController personsController = (PersonsController)context.Controller;
            IDictionary<string, object?>? parameters = (IDictionary<string, object?>?)context.HttpContext.Items["argument"];
            if (parameters is not null)
            {
                if (parameters.ContainsKey("searchBy"))
                    personsController.ViewData["SearchBy"] = parameters["searchBy"];

                if (parameters.ContainsKey("searchString"))
                    personsController.ViewData["SearchString"] = parameters["searchString"];

                if (parameters.ContainsKey("sortBy"))
                    personsController.ViewData["SortBy"] = parameters["sortBy"];
                else
                    personsController.ViewData["SortBy"] = nameof(PersonResponse.PersonName);

                if (parameters.ContainsKey("sortOrder"))
                    personsController.ViewData["SortOrder"] = parameters["sortOrder"]?.ToString();
                else
                    personsController.ViewData["SortOrder"] = "ASC";

            }
            personsController.ViewData["SearchFields"] = new Dictionary<string, string>()
            {
                {nameof(PersonResponse.PersonName), "Person Name"},
                {nameof(PersonResponse.Email), "Email"},
                {nameof(PersonResponse.CountryID) , "Country Name"},
                {nameof(PersonResponse.Address), "Address"},
                {nameof(PersonResponse.Gender), "Gender" },
                {nameof(PersonResponse.DateOfBirth), "Date of birth"},
                {nameof(PersonResponse.ReciveNewsLetters), "Receive News Letters"}
            };

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            // To do: add before logic here
            _logger.LogInformation("{FilterName}.{MethodName} method", nameof(PersonsListActionFilter), nameof(OnActionExecuting));

            context.HttpContext.Items["argument"] = context.ActionArguments;

            if (context.ActionArguments.ContainsKey("searchBy"))
            {
                string? searchBy = Convert.ToString(context.ActionArguments["searchBy"]);
                if (!string.IsNullOrEmpty(searchBy))
                {
                    PropertyInfo? propertyInfo = typeof(PersonResponse).GetProperty(searchBy);
                    if (propertyInfo is null)
                    {
                        _logger.LogInformation($"search by updated value is PersonName");
                        context.ActionArguments["searchBy"] = nameof(PersonResponse.PersonName);
                    }
                    else
                    {
                        _logger.LogInformation($"search by actual value is {searchBy}");
                    }
                }
            }
        }
    }
}
