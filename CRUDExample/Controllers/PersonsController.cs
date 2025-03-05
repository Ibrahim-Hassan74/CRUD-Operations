using AspNetCoreGeneratedDocument;
using CRUDExample.Filters.ActionFilters;
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
    [TypeFilter(typeof(ResponseHeaderActionFilter), Arguments = new object[] { "My-Key-From-Controller", "My-Value-From-Controller", 3 }, Order = 3)]
    public class PersonsController : Controller
    {
        private readonly IPersonsService _personsService;
        private readonly ICountriesService _countriesService;
        private readonly ILogger<PersonsController> _logger;
        public PersonsController(IPersonsService personsService, ICountriesService countriesService, ILogger<PersonsController> logger)
        {
            _personsService = personsService;
            _countriesService = countriesService;
            _logger = logger;
        }

        public IActionResult Dashboard()
        {
            return View();
        }

        [Route("[action]")]
        [Route("/")]
        [TypeFilter(typeof(PersonsListActionFilter), Order = 4)]
        [TypeFilter(typeof(ResponseHeaderActionFilter), Arguments = new object[] { "My-Key-From-Action", "My-Value-From-Action", 1 }, Order = 1)]
        public async Task<IActionResult> Index(string searchBy, string? searchString,
            string sortBy = nameof(PersonResponse.PersonName),
            SortOrderOptions sortOrder = SortOrderOptions.ASC)
        {
            _logger.LogInformation("Index action method of persons controller");
            _logger.LogDebug($"search by: {searchBy}, search string: {searchString}, sort by: {sortBy}, sort order: {sortOrder}");
            // Search
            var persons = await _personsService.GetFilteredPersons(searchBy, searchString);
            List<PersonResponse> personResponses = await
                _personsService.GetSortedPersons(persons, sortBy, sortOrder);
            return View(personResponses);
        }

        //[Route("create")]
        [Route("[action]")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var countries = await _countriesService.GetAllCountries();
            ViewBag.Countries = countries.Select(x =>
            {
                return new SelectListItem() { Text = x.CountryName, Value = x.CountryID.ToString() };
            });
            return View();
        }

        //[Route("create")]
        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> Create(PersonAddRequest? personAddRequest)
        {
            if (!ModelState.IsValid)
            {
                List<string> errors = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();
                ViewBag.Errors = errors;
                var countries = await _countriesService.GetAllCountries();
                ViewBag.Countries = countries.Select(x =>
                {
                    return new SelectListItem() { Text = x.CountryName, Value = x.CountryID.ToString() };
                });
                return View(personAddRequest);
            }
            var personResponse = await _personsService.AddPerson(personAddRequest);
            return RedirectToAction("Index", "Persons");
        }

        [Route("[action]/{personID}")]
        [HttpGet]
        public async Task<IActionResult> Edit(Guid? personID)
        {
            PersonResponse? personResponse = await _personsService.GetPersonByPersonID(personID);
            if (personResponse is null)
            {
                return RedirectToAction("Index", "Persons");
            }
            var countries = await _countriesService.GetAllCountries();
            ViewBag.Countries = countries.Select(x =>
            {
                return new SelectListItem() { Text = x.CountryName, Value = x.CountryID.ToString() };
            });

            return View(personResponse.ToPersonUpdateRequest());
        }

        [HttpPost]
        [Route("[action]/{personID}")]
        public async Task<IActionResult> Edit(PersonUpdateRequest? personUpdateRequest)
        {
            PersonResponse? personResponse = await _personsService.GetPersonByPersonID(personUpdateRequest?.PersonID);
            if (personResponse is null)
            {
                return RedirectToAction("Index", "Persons");
            }

            if (ModelState.IsValid)
            {
                await _personsService.UpdatePerson(personUpdateRequest);
                return RedirectToAction("Index", "Persons");
            }
            List<string> errors = ModelState.Values.SelectMany(x => x.Errors).Select(error => error.ErrorMessage).ToList();
            ViewBag.Errors = errors;
            var countries = await _countriesService.GetAllCountries();
            ViewBag.Countries = countries.Select(x =>
            {
                return new SelectListItem() { Text = x.CountryName, Value = x.CountryID.ToString() };
            });
            return View(personResponse.ToPersonUpdateRequest());
        }

        [HttpGet]
        [Route("[action]/{personID}")]
        public async Task<IActionResult> Delete(Guid? personID)
        {
            PersonResponse? personResponse = await _personsService.GetPersonByPersonID(personID);
            if (personResponse is null)
                return RedirectToAction("Index", "Persons");
            return View(personResponse);
        }

        [HttpPost]
        [Route("[action]/{personID}")]
        public async Task<IActionResult> Delete(PersonResponse? personResponse)
        {
            PersonResponse? person = await _personsService.GetPersonByPersonID(personResponse?.PersonID);
            if (person is null)
                return RedirectToAction("Index", "Persons");

            bool isDeleted = await _personsService.DeletePerson(personResponse?.PersonID);
            return RedirectToAction("Index", "Persons");
        }

        [Route("[action]")]
        public async Task<IActionResult> PersonsPDF()
        {
            List<PersonResponse> persons = await _personsService.GetAllPersons();
            return new ViewAsPdf("PersonsPDF", persons, ViewData)
            {
                PageMargins = new Rotativa.AspNetCore.Options.Margins() { Bottom = 20, Left = 20, Right = 20, Top = 20 },
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape
            };
        }

        [Route("[action]")]
        public async Task<IActionResult> PersonsCSV()
        {
            MemoryStream memoryStream = await _personsService.GetPersonsCSV();

            return File(memoryStream, "application/octet-stream", "persons.csv");

        }

        [Route("[action]")]
        public async Task<IActionResult> PersonsExcel()
        {
            MemoryStream memoryStream = await _personsService.GetPersonsExcel();
            return File(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "persons.xlsx");
        }
    }
}
