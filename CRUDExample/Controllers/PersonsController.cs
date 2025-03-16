using AspNetCoreGeneratedDocument;
using CRUDExample.Filters;
using CRUDExample.Filters.ActionFilters;
using CRUDExample.Filters.Authorization;
using CRUDExample.Filters.ExceptionsFilter;
using CRUDExample.Filters.Resourse;
using CRUDExample.Filters.ResultFilters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using Rotativa.AspNetCore;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using System.IO;

namespace CRUDExample.Controllers
{
    [Route("[controller]")]
    //[TypeFilter(typeof(ResponseHeaderActionFilter), Arguments = new object[] { "My-Key-From-Controller", "My-Value-From-Controller", 3 }, Order = 3)]
    //[TypeFilter(typeof(HandleExceptionFilter))]
    [TypeFilter(typeof(PersonsAlwaysRunResultFilter))]
    [ResponseHeaderFilterFactory("My-Key-From-Controller", "My-Value-From-Controller", 3)]
    public class PersonsController : Controller
    {
        private readonly IPersonsGetterService _personsGetterService;
        private readonly IPersonsAdderService _personsAdderService;
        private readonly IPersonsSorterService _personsSorterService;
        private readonly IPersonsDeleterService _personsDeleterService;
        private readonly IPersonsUpdaterService _personsUpdaterService;
        private readonly ICountriesGetterService _countriesGetterService;
        private readonly ILogger<PersonsController> _logger;
        public PersonsController(IPersonsGetterService personsGetterService, IPersonsAdderService personsAdderService, 
            IPersonsSorterService personsSorterService, IPersonsDeleterService personsDeleterService, 
            IPersonsUpdaterService personsUpdaterService, ICountriesGetterService countriesGetterService, ILogger<PersonsController> logger)
        {
            _personsGetterService = personsGetterService;
            _personsAdderService = personsAdderService;
            _personsSorterService = personsSorterService;
            _personsDeleterService = personsDeleterService;
            _personsUpdaterService = personsUpdaterService;
            _countriesGetterService = countriesGetterService;
            _logger = logger;
        }

        public IActionResult Dashboard()
        {
            return View();
        }

        [Route("[action]")]
        [Route("/")]
        [ServiceFilter(typeof(PersonsListActionFilter), Order = 4)]
        //[TypeFilter(typeof(ResponseHeaderActionFilter), Arguments = new object[] { "My-Key-From-Action", "My-Value-From-Action", 1 }, Order = 1)]
        [ResponseHeaderFilterFactory("My-Key-From-Action", "My-Value-From-Action", 1)]
        [TypeFilter(typeof(PersonsListResultFilter))]
        [SkipFilter]
        public async Task<IActionResult> Index(string searchBy, string? searchString,
            string sortBy = nameof(PersonResponse.PersonName),
            SortOrderOptions sortOrder = SortOrderOptions.ASC)
        {
            _logger.LogInformation("Index action method of persons controller");
            _logger.LogDebug($"search by: {searchBy}, search string: {searchString}, sort by: {sortBy}, sort order: {sortOrder}");
            // Search
            var persons = await _personsGetterService.GetFilteredPersons(searchBy, searchString);
            List<PersonResponse> personResponses = await
                _personsSorterService.GetSortedPersons(persons, sortBy, sortOrder);
            return View(personResponses);
        }

        //[Route("create")]
        [Route("[action]")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var countries = await _countriesGetterService.GetAllCountries();
            ViewBag.Countries = countries.Select(x =>
            {
                return new SelectListItem() { Text = x.CountryName, Value = x.CountryID.ToString() };
            });
            return View();
        }

        //[Route("create")]
        [Route("[action]")]
        [HttpPost]
        [TypeFilter(typeof(PersonCreateAndEditPostActionFilter))]
        [TypeFilter(typeof(FeatureDisabledResourceFilter), Arguments = new object[] { false })]
        public async Task<IActionResult> Create(PersonAddRequest? personRequest)
        {
            var personResponse = await _personsAdderService.AddPerson(personRequest);
            return RedirectToAction("Index", "Persons");
        }

        [Route("[action]/{personID}")]
        [HttpGet]
        [TypeFilter(typeof(TokenResultFilter))]
        public async Task<IActionResult> Edit(Guid? personID)
        {
            PersonResponse? personResponse = await _personsGetterService.GetPersonByPersonID(personID);
            if (personResponse is null)
            {
                return RedirectToAction("Index", "Persons");
            }
            var countries = await _countriesGetterService.GetAllCountries();
            ViewBag.Countries = countries.Select(x =>
            {
                return new SelectListItem() { Text = x.CountryName, Value = x.CountryID.ToString() };
            });

            return View(personResponse.ToPersonUpdateRequest());
        }

        [HttpPost]
        [Route("[action]/{personID}")]
        [TypeFilter(typeof(PersonCreateAndEditPostActionFilter))]
        [TypeFilter(typeof(TokenAuthorizationFilter))]
        public async Task<IActionResult> Edit(PersonUpdateRequest? personRequest)
        {
            PersonResponse? personResponse = await _personsGetterService.GetPersonByPersonID(personRequest?.PersonID);
            if (personResponse is null)
            {
                return RedirectToAction("Index", "Persons");
            }
            await _personsUpdaterService.UpdatePerson(personRequest);
            return RedirectToAction("Index", "Persons");
        }

        [HttpGet]
        [Route("[action]/{personID}")]
        public async Task<IActionResult> Delete(Guid? personID)
        {
            PersonResponse? personResponse = await _personsGetterService.GetPersonByPersonID(personID);
            if (personResponse is null)
                return RedirectToAction("Index", "Persons");
            return View(personResponse);
        }

        [HttpPost]
        [Route("[action]/{personID}")]
        public async Task<IActionResult> Delete(PersonResponse? personResponse)
        {
            PersonResponse? person = await _personsGetterService.GetPersonByPersonID(personResponse?.PersonID);
            if (person is null)
                return RedirectToAction("Index", "Persons");

            bool isDeleted = await _personsDeleterService.DeletePerson(personResponse?.PersonID);
            return RedirectToAction("Index", "Persons");
        }

        [Route("[action]")]
        public async Task<IActionResult> PersonsPDF()
        {
            List<PersonResponse> persons = await _personsGetterService.GetAllPersons();
            return new ViewAsPdf("PersonsPDF", persons, ViewData)
            {
                PageMargins = new Rotativa.AspNetCore.Options.Margins() { Bottom = 20, Left = 20, Right = 20, Top = 20 },
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape
            };
        }

        [Route("[action]")]
        public async Task<IActionResult> PersonsCSV()
        {
            MemoryStream memoryStream = await _personsGetterService.GetPersonsCSV();

            return File(memoryStream, "application/octet-stream", "persons.csv");

        }

        [Route("[action]")]
        public async Task<IActionResult> PersonsExcel()
        {
            MemoryStream memoryStream = await _personsGetterService.GetPersonsExcel();
            return File(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "persons.xlsx");
        }
    }
}
