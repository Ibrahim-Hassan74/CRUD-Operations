using CRUDExample.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServiceContracts;
using ServiceContracts.DTO;

namespace CRUDExample.Filters.ActionFilters
{
    public class PersonCreateAndEditPostActionFilter : IAsyncActionFilter
    {
        private readonly ICountriesGetterService _countriesGetterService;
        private readonly ILogger<PersonCreateAndEditPostActionFilter> _logger;
        public PersonCreateAndEditPostActionFilter(ICountriesGetterService countriesGetterService, ILogger<PersonCreateAndEditPostActionFilter> logger)
        {
            _countriesGetterService = countriesGetterService;
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.Controller is PersonsController personsController)
            {
                if (!personsController.ModelState.IsValid)
                {
                    List<string> errors = personsController.ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();
                    personsController.ViewBag.Errors = errors;
                    var countries = await _countriesGetterService.GetAllCountries();
                    personsController.ViewBag.Countries = countries.Select(x =>
                    {
                        return new SelectListItem() { Text = x.CountryName, Value = x.CountryID.ToString() };
                    });
                    var personRequest = context.ActionArguments["personRequest"];
                    context.Result = personsController.View(personRequest); // short-circuits or skips the subsequent filters & action filter
                }
                else
                {
                    await next();
                }
            }
            else
            {
                await next();
            }
            _logger.LogInformation("{FilterName}.{MethodName} method - after", nameof(PersonCreateAndEditPostActionFilter), nameof(OnActionExecutionAsync));
        }
    }
}
