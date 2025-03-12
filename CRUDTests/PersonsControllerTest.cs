using AutoFixture;
using Moq;
using ServiceContracts;
using System;
using FluentAssertions;
using CRUDExample.Controllers;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sevices;

namespace CRUDTests
{
    public class PersonsControllerTest
    {
        private readonly IPersonsService _personsService;
        private readonly ICountriesService _countriesService;
        private readonly Mock<ICountriesService> _countriesServiceMock;
        private readonly Mock<IPersonsService> _personsServiceMock;
        private readonly ILogger<PersonsController> _logger;
        private readonly Mock<ILogger<PersonsController>> _loggerMock;
        private readonly IFixture _fixture;
        public PersonsControllerTest()
        {
            _countriesServiceMock = new Mock<ICountriesService>();
            _personsServiceMock = new Mock<IPersonsService>();
            _loggerMock = new Mock<ILogger<PersonsController>>();

            _countriesService = _countriesServiceMock.Object;
            _personsService = _personsServiceMock.Object;
            _logger = _loggerMock.Object;

            _fixture = new Fixture();
        }

        #region Index

        [Fact]
        public async Task Index_ShouldReturnIndexViewPersonList()
        {
            List<PersonResponse> personResponses = _fixture.Create<List<PersonResponse>>();

            PersonsController personsController = new PersonsController(_personsService, _countriesService, _logger);

            _personsServiceMock.Setup(temp => temp.GetFilteredPersons(It.IsAny<String>(), It.IsAny<String>()))
                .ReturnsAsync(personResponses);

            _personsServiceMock
                .Setup(temp => temp.GetSortedPersons(It.IsAny<List<PersonResponse>>(),
                It.IsAny<string>(), It.IsAny<SortOrderOptions>()))
                .ReturnsAsync(personResponses);

            IActionResult result = await personsController.Index(_fixture.Create<String>(), _fixture.Create<String>(),
                _fixture.Create<String>(), _fixture.Create<SortOrderOptions>());

            ViewResult viewResult = Assert.IsType<ViewResult>(result);
            viewResult.ViewData.Model.Should().BeAssignableTo<IEnumerable<PersonResponse>>();
            viewResult.ViewData.Model.Should().Be(personResponses);
        }


        #endregion

        #region Create

        //[Fact]

        //public async Task Create_IfModelErrors_ToReturnCreateView()
        //{
        //    PersonAddRequest personAddRequest = _fixture.Create<PersonAddRequest>();

        //    PersonResponse personResponse = _fixture.Create<PersonResponse>();

        //    List<CountryResponse> countryResponses = _fixture.Create<List<CountryResponse>>();

        //    PersonsController personsController = new PersonsController(_personsService, _countriesService, _logger);

        //    _countriesServiceMock.Setup(temp => temp.GetAllCountries())
        //        .ReturnsAsync(countryResponses);

        //    _personsServiceMock.Setup(temp => temp.AddPerson(It.IsAny<PersonAddRequest>()))
        //        .ReturnsAsync(personResponse);

        //    personsController.ModelState.AddModelError("PersonName", "Person Name can't be blank");

        //    IActionResult result = await personsController.Create(personAddRequest);

        //    ViewResult viewResult = Assert.IsType<ViewResult>(result);
        //    viewResult.ViewData.Model.Should().BeAssignableTo<PersonAddRequest>();
        //    viewResult.ViewData.Model.Should().Be(personAddRequest);
        //}

        [Fact]
        public async Task Create_IfNoModelErrors_ToReturnRedirectToIndexView()
        {
            PersonAddRequest personAddRequest = _fixture.Create<PersonAddRequest>();

            PersonResponse personResponse = _fixture.Create<PersonResponse>();

            _personsServiceMock.Setup(temp => temp.AddPerson(It.IsAny<PersonAddRequest>()))
                .ReturnsAsync(personResponse);

            PersonsController personsController = new PersonsController(_personsService, _countriesService, _logger);

            IActionResult result = await personsController.Create(personAddRequest);

            RedirectToActionResult redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            redirectToActionResult.ActionName.Should().Be("Index");
            redirectToActionResult.ControllerName.Should().Be("Persons");
        }
        #endregion

        #region Edit

        [Fact]
        public async Task Edit_IfPersonNotFound_ToReturnRedirectToIndexView()
        {
            PersonUpdateRequest? personUpdateRequest = null;

            _personsServiceMock.Setup(temp => temp.GetPersonByPersonID(It.IsAny<Guid?>()))
                .ReturnsAsync(null as PersonResponse);

            PersonsController personsController = new PersonsController(_personsService, _countriesService, _logger);

            IActionResult result = await personsController.Edit(personUpdateRequest);

            RedirectToActionResult redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);

            redirectToActionResult.ActionName.Should().Be("Index");
            redirectToActionResult.ControllerName.Should().Be("Persons");
        }

        //[Fact]
        //public async Task Edit_IfModelErrors_ToReturnEditView()
        //{
        //    PersonUpdateRequest personUpdateRequest = _fixture.Create<PersonUpdateRequest>();

        //    List<CountryResponse> countryResponses = _fixture.Create<List<CountryResponse>>();

        //    PersonResponse personResponse = _fixture.Build<PersonResponse>()
        //                                        .With(temp => temp.Gender, "Male")
        //                                        .Create();

        //    _personsServiceMock.Setup(temp => temp.GetPersonByPersonID(It.IsAny<Guid>()))
        //        .ReturnsAsync(personResponse);

        //    _countriesServiceMock.Setup(temp => temp.GetAllCountries())
        //        .ReturnsAsync(countryResponses);

        //    PersonsController personsController = new PersonsController(_personsService, _countriesService, _logger);

        //    personsController.ModelState.AddModelError("Email", "Invalid Email Format");

        //    IActionResult result = await personsController.Edit(personUpdateRequest);

        //    ViewResult viewResult = Assert.IsType<ViewResult>(result);

        //    viewResult.ViewData.Model.Should().BeAssignableTo<PersonUpdateRequest>();
        //    viewResult.ViewData.Model.Should().Be(personResponse.ToPersonUpdateRequest());

        //}

        [Fact]
        public async Task Edit_IfNoModelErrors_ToRedirectToIndexView()
        {
            PersonUpdateRequest personUpdateRequest = _fixture.Create<PersonUpdateRequest>();

            List<CountryResponse> countryResponses = _fixture.Create<List<CountryResponse>>();

            PersonResponse personResponse = _fixture.Build<PersonResponse>()
                                                .With(temp => temp.Gender, "Male")
                                                .Create();

            _personsServiceMock.Setup(temp => temp.GetPersonByPersonID(It.IsAny<Guid>()))
                .ReturnsAsync(personResponse);

            _personsServiceMock.Setup(temp => temp.UpdatePerson(It.IsAny<PersonUpdateRequest>()))
                .ReturnsAsync(personResponse);

            PersonsController personsController = new PersonsController(_personsService, _countriesService, _logger);


            IActionResult result = await personsController.Edit(personUpdateRequest);

            RedirectToActionResult redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);

            redirectToActionResult.ActionName.Should().Be("Index");
            redirectToActionResult.ControllerName.Should().Be("Persons");

        }

        #endregion

        #region Delete

        [Fact] 
        public async Task Delete_IfPersonNotFound_ReturnRedirectToIndexView()
        {
            PersonResponse personResponse = _fixture.Create<PersonResponse>();

            _personsServiceMock.Setup(temp => temp.GetPersonByPersonID(It.IsAny<Guid>()))
                .ReturnsAsync(null as PersonResponse);

            PersonsController personsController = new PersonsController(_personsService, _countriesService, _logger);

            IActionResult result = await personsController.Delete(personResponse);

            RedirectToActionResult redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);

            redirectToActionResult.ActionName.Should().Be("Index");
            redirectToActionResult.ControllerName.Should().Be("Persons");
        }

        [Fact]
        public async Task Delete_IfNoModelErrors_ReturnRedirectToIndexView()
        {
            PersonResponse personResponse = _fixture.Create<PersonResponse>();

            _personsServiceMock.Setup(temp => temp.GetPersonByPersonID(It.IsAny<Guid>()))
                .ReturnsAsync(personResponse);

            _personsServiceMock.Setup(temp => temp.DeletePerson(It.IsAny<Guid>()))
                .ReturnsAsync(true);

            PersonsController personsController = new PersonsController(_personsService, _countriesService, _logger);

            IActionResult result = await personsController.Delete(personResponse);

            RedirectToActionResult redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);

            redirectToActionResult.ActionName.Should().Be("Index");
            redirectToActionResult.ControllerName.Should().Be("Persons");
        }

        #endregion

    }
}
