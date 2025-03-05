using ServiceContracts;
using ServiceContracts.DTO;
using Sevices;
using Entities;
using ServiceContracts.Enums;
using System.Diagnostics;
using Xunit.Abstractions;
using Microsoft.EntityFrameworkCore;
using EntityFrameworkCoreMock;
using AutoFixture;
using System.Reflection;
using System;
using FluentAssertions;
using RepositoryContracts;
using Moq;
using System.Linq.Expressions;
using FluentAssertions.Execution;
using Serilog.Extensions.Hosting;
using Serilog;
using Microsoft.Extensions.Logging;

namespace CRUDTests
{
    public class PersonsServicesTest
    {
        private readonly IPersonsService _personsService;
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly Mock<IPersonsRepository> _personRepositoryMock;
        private readonly IFixture _fixture;
        private readonly IPersonsRepository _personsRepository;
        //constructor
        public PersonsServicesTest(ITestOutputHelper testOutputHelper)
        {
            _fixture = new Fixture();

            _personRepositoryMock = new Mock<IPersonsRepository>();

            _personsRepository = _personRepositoryMock.Object;

            var diagnosticContextMock = new Mock<IDiagnosticContext>();
            var loggerMock = new Mock<ILogger<PersonsService>>();

            #region Mock DbContext
            //List<Country> countries = new List<Country>() { };
            //List<Person> persons = new List<Person>() { };

            //DbContextMock<ApplicationDbContext> dbContextMock = new DbContextMock<ApplicationDbContext>(
            //new DbContextOptionsBuilder<ApplicationDbContext>().Options);

            //ApplicationDbContext dbContext = dbContextMock.Object;

            //dbContextMock.CreateDbSetMock(temp => temp.Countries, countries);
            //dbContextMock.CreateDbSetMock(temp => temp.Persons, persons);
            #endregion

            _personsService = new PersonsService(_personsRepository, loggerMock.Object, diagnosticContextMock.Object);

            _testOutputHelper = testOutputHelper;
        }
        private string GenerateUniqueEmail(string? value = "")
        {
            return $"user{value}{Guid.NewGuid().ToString("N").Substring(0, 8)}@gmail.com";
        }
        private DateTime GenerateUniqueDateOfBirth()
        {
            var startDate = new DateTime(1980, 1, 1);
            var range = (DateTime.Today - startDate).Days;
            return startDate.AddDays(_fixture.Create<int>() % range);
        }

        #region AddPerson
        [Fact]
        public async Task AddPersons_NullPersons_ToBeArgumentNullException()
        {
            // Arrange
            PersonAddRequest? request = null;
            // Assert
            Func<Task> action = (async () =>
            {
                // Acts
                await _personsService.AddPerson(request);
            });
            await action.Should().ThrowAsync<ArgumentNullException>();
            //await Assert.ThrowsAsync<ArgumentNullException>
        }

        [Fact]
        public async Task AddPersons_PersonNameIsNull_ToBeArgumentException()
        {
            // Arrange
            PersonAddRequest? request = _fixture.Build<PersonAddRequest>()
                .With(x => x.PersonName, null as string)
                .Create();
            Person person = request.ToPerson();
            _personRepositoryMock.Setup(temp => temp.AddPerson(It.IsAny<Person>()))
                .ReturnsAsync(person);

            Func<Task> action = async () =>
            {
                await _personsService.AddPerson(request);
            };
            await action.Should().ThrowAsync<ArgumentException>();
            //await Assert.ThrowsAsync<ArgumentException>();
        }

        [Fact]
        public async Task AddPersons_PersonEmailIsNull()
        {
            // Arrange
            PersonAddRequest? request = _fixture.Build<PersonAddRequest>()
                .With(x => x.Email, null as string)
                .Create();
            var person = request.ToPerson();
            _personRepositoryMock.Setup(temp => temp.AddPerson(It.IsAny<Person>()))
                .ReturnsAsync(person);

            // Assert
            Func<Task> action = async () =>
            {
                await _personsService.AddPerson(request);
            };
            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task AddPersons_PersonEmailFormat_ToBeArgumentException()
        {
            // Arrange
            PersonAddRequest? request = _fixture.Create<PersonAddRequest>();
            Person person = request.ToPerson();
            _personRepositoryMock.Setup(temp => temp.AddPerson(It.IsAny<Person>()))
                .ReturnsAsync(person);
            // Assert
            Func<Task> action = async () =>
            {
                await _personsService.AddPerson(request);
            };
            await action.Should().ThrowAsync<ArgumentException>();
        }

        //[Fact]
        //public void AddPersons_PersonDateOfBirthIsNull()
        //{
        //    // Arrange
        //    PersonAddRequest? request = new PersonAddRequest()
        //    {
        //        DateOfBirth = null
        //    };
        //    // Assert
        //    Assert.Throws<ArgumentException>(() =>
        //    {
        //        // Acts
        //        _personsService.AddPerson(request);
        //    });
        //}

        [Fact]
        public async Task AddPersons_PersonDateOfBirthIsNotValid_ToBeArgumentException()
        {
            // Arrange
            PersonAddRequest? request = _fixture.Build<PersonAddRequest>()
                                       .With(x => x.Email, GenerateUniqueEmail())
                                       .Create();
            Person person = request.ToPerson();
            _personRepositoryMock.Setup(temp => temp.AddPerson(It.IsAny<Person>()))
                .ReturnsAsync(person);
            // Assert
            Func<Task> action = async () =>
            {
                await _personsService.AddPerson(request);
            };
            await action.Should().ThrowAsync<InvalidOperationException>();
        }

        //[Fact]
        //public void AddPersons_PersonAddressIsNull()
        //{
        //    // Arrange
        //    PersonAddRequest? request = new PersonAddRequest()
        //    {
        //        Address = null
        //    };
        //    // Assert
        //    Assert.Throws<ArgumentException>(() =>
        //    {
        //        // Acts
        //        _personsService.AddPerson(request);
        //    });
        //}

        [Fact]
        public async Task AddPersons_PersonGenderIsNull_ToBeArgumentException()
        {
            // Arrange
            PersonAddRequest? request = _fixture.Build<PersonAddRequest>()
                .With(x => x.Gender, (GenderOptions?)null)
                .Create();
            Person person = request.ToPerson();
            _personRepositoryMock.Setup(temp => temp.AddPerson(It.IsAny<Person>()))
                .ReturnsAsync(person);
            // Assert
            Func<Task> action = async () =>
            {
                // Acts
                await _personsService.AddPerson(request);
            };

            await action.Should().ThrowAsync<ArgumentException>();

        }

        [Fact]
        public async Task AddPersons_FullPersonDetails_ToBeSuccessful()
        {
            // Arrange
            //PersonAddRequest? request = _fixture.Create<PersonAddRequest>();
            PersonAddRequest? personAddRequest = _fixture.Build<PersonAddRequest>()
                .With(x => x.Email, GenerateUniqueEmail())
                .With(x => x.DateOfBirth, Convert.ToDateTime("2000-09-11"))
                .Create();

            Person person = personAddRequest.ToPerson();
            PersonResponse personResponseExpected = person.ToPersonResponse();

            _personRepositoryMock
                .Setup(temp => temp.AddPerson(It.IsAny<Person>()))
                .ReturnsAsync(person);

            // Acts
            PersonResponse personResponseFromAdd = await _personsService.AddPerson(personAddRequest);
            personResponseExpected.PersonID = personResponseFromAdd.PersonID;
            // Assert

            //Assert.True(personResponseFromAdd.PersonID != Guid.Empty);
            personResponseFromAdd.PersonID.Should().NotBe(Guid.Empty);
            personResponseFromAdd.Should().Be(personResponseExpected);
            //Assert.Contains(personResponseFromAdd, personResponsesList);

        }

        #endregion

        #region GetPersonByPersonID
        [Fact]

        public async Task GetPersonByPersonID_NullPersonID_ToBeNull()
        {
            Guid? PersonID = null;
            PersonResponse? personResponse = await _personsService.GetPersonByPersonID(PersonID);
            //Assert.Null(personResponse);
            personResponse.Should().BeNull();
        }
        [Fact]
        public async Task GetPersonByPersonID_ValidPersonID_ToBeSuccessful()
        {
            // Arrange
            // Act
            Person person = _fixture.Build<Person>()
                .With(x => x.Email, GenerateUniqueEmail())
                .With(x => x.DateOfBirth, GenerateUniqueDateOfBirth())
                .With(temp => temp.Country, null as Country)
                .Create();

            PersonResponse personResponseExpected = person.ToPersonResponse();
            _personRepositoryMock.Setup(temp => temp.GetPersonByID(It.IsAny<Guid>()))
                .ReturnsAsync(person);

            PersonResponse? personResponseFromGet = await _personsService.GetPersonByPersonID(person.PersonID);

            // Assert
            //Assert.Equal(personResponseFromGet, personResponseFromAdd); 
            personResponseFromGet.Should().Be(personResponseExpected);
        }


        #endregion

        #region GetAllPersons

        [Fact]
        public async Task GetAllPersons_ToBeEmptyList()
        {
            /// Arrange
            var persons = new List<Person>();
            _personRepositoryMock.Setup(temp => temp.GetAllPersons())
                .ReturnsAsync(persons);

            List<PersonResponse> personResponses = await _personsService.GetAllPersons();
            //Assert.Empty(personResponses);
            personResponses.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAllPersons_WithFewPersons_ToBeSuccessful()
        {

            List<Person> persons = new List<Person>
            {
                _fixture.Build<Person>()
                .With(temp => temp.Email, GenerateUniqueEmail())
                .With(temp => temp.DateOfBirth, GenerateUniqueDateOfBirth())
                .With(temp => temp.Country, null as Country)
                .Create(),
                _fixture.Build<Person>()
                .With(temp => temp.Email, GenerateUniqueEmail())
                .With(temp => temp.DateOfBirth, GenerateUniqueDateOfBirth())
                .With(temp => temp.Country, null as Country)
                .Create(),
                _fixture.Build<Person>()
                .With(temp => temp.Email, GenerateUniqueEmail())
                .With(temp => temp.DateOfBirth, GenerateUniqueDateOfBirth())
                .With(temp => temp.Country, null as Country)
                .Create()
            };

            List<PersonResponse> personResponsesExpected =
                persons.Select(x => x.ToPersonResponse()).ToList();

            // print personResponseFromAdd 
            _testOutputHelper.WriteLine("Expected: ");
            foreach (var person in personResponsesExpected)
            {
                _testOutputHelper.WriteLine(person.ToString());
            }

            _personRepositoryMock.Setup(temp => temp.GetAllPersons())
                .ReturnsAsync(persons);

            List<PersonResponse> personResponsesFromGet = await _personsService.GetAllPersons();

            _testOutputHelper.WriteLine("Actual: ");
            foreach (var person in personResponsesFromGet)
            {
                _testOutputHelper.WriteLine(person.ToString());
            }

            //foreach (var personResponse in personResponsesExpected)
            //{
            //    Assert.Contains(personResponse, personResponsesFromGet);
            //}

            personResponsesFromGet.Should().BeEquivalentTo(personResponsesExpected);

        }

        #endregion

        #region GetFilteredPersons

        [Fact]
        public async Task GetFilteredPersons_EmptySearchText_ToBeSuccessful()
        {
            List<Person> persons = new List<Person>
            {
                _fixture.Build<Person>()
                .With(temp => temp.Email, GenerateUniqueEmail())
                .With(temp => temp.DateOfBirth, GenerateUniqueDateOfBirth())
                .With(temp => temp.Country, null as Country)
                .Create(),
                _fixture.Build<Person>()
                .With(temp => temp.Email, GenerateUniqueEmail())
                .With(temp => temp.DateOfBirth, GenerateUniqueDateOfBirth())
                .With(temp => temp.Country, null as Country)
                .Create(),
                _fixture.Build<Person>()
                .With(temp => temp.Email, GenerateUniqueEmail())
                .With(temp => temp.DateOfBirth, GenerateUniqueDateOfBirth())
                .With(temp => temp.Country, null as Country)
                .Create()
            };

            List<PersonResponse> personResponsesExpected = persons.Select(x => x.ToPersonResponse()).ToList();


            // print personResponseFromAdd 
            _testOutputHelper.WriteLine("Expected: ");
            foreach (var person in personResponsesExpected)
            {
                _testOutputHelper.WriteLine(person.ToString());
            }

            _personRepositoryMock
                .Setup(temp => temp.GetFilteredPersons(It.IsAny<Expression<Func<Person, bool>>>()))
                .ReturnsAsync(persons);

            List<PersonResponse> personResponsesFromSearch =
               await _personsService.GetFilteredPersons(nameof(Person.PersonName), "");

            _testOutputHelper.WriteLine("Actual: ");
            foreach (var person in personResponsesFromSearch)
            {
                _testOutputHelper.WriteLine(person.ToString());
            }

            //foreach (var personResponse in personResponsesFromAdd)
            //{
            //    Assert.Contains(personResponse, personResponsesFromSearch);
            //}

            personResponsesFromSearch.Should().BeEquivalentTo(personResponsesExpected);

        }

        [Fact]
        public async Task GetFilteredPersons_SearchByPersonName_ToBeSuccessful()
        {
            List<Person> persons = new List<Person>
            {
                _fixture.Build<Person>()
                .With(temp => temp.Email, GenerateUniqueEmail())
                .With(temp => temp.DateOfBirth, GenerateUniqueDateOfBirth())
                .With(temp => temp.Country, null as Country)
                .Create(),
                _fixture.Build<Person>()
                .With(temp => temp.Email, GenerateUniqueEmail())
                .With(temp => temp.DateOfBirth, GenerateUniqueDateOfBirth())
                .With(temp => temp.Country, null as Country)
                .Create(),
                _fixture.Build<Person>()
                .With(temp => temp.Email, GenerateUniqueEmail())
                .With(temp => temp.DateOfBirth, GenerateUniqueDateOfBirth())
                .With(temp => temp.Country, null as Country)
                .Create()
            };

            List<PersonResponse> personResponsesExpected = persons.Select(x => x.ToPersonResponse()).ToList();


            // print personResponseFromAdd 
            _testOutputHelper.WriteLine("Expected: ");
            foreach (var person in personResponsesExpected)
            {
                _testOutputHelper.WriteLine(person.ToString());
            }

            _personRepositoryMock
                .Setup(temp => temp.GetFilteredPersons(It.IsAny<Expression<Func<Person, bool>>>()))
                .ReturnsAsync(persons);

            List<PersonResponse> personResponsesFromSearch =
               await _personsService.GetFilteredPersons(nameof(Person.PersonName), "sa");

            _testOutputHelper.WriteLine("Actual: ");
            foreach (var person in personResponsesFromSearch)
            {
                _testOutputHelper.WriteLine(person.ToString());
            }

            //foreach (var personResponse in personResponsesFromAdd)
            //{
            //    Assert.Contains(personResponse, personResponsesFromSearch);
            //}

            personResponsesFromSearch.Should().BeEquivalentTo(personResponsesExpected);

        }

        [Fact]
        public async Task GetFilteredPersons_SearchByEmail_ToBeSuccessful()
        {
            List<Person> persons = new List<Person>
            {
                _fixture.Build<Person>()
                .With(temp => temp.Email, GenerateUniqueEmail("i"))
                .With(temp => temp.DateOfBirth, GenerateUniqueDateOfBirth())
                .With(temp => temp.Country, null as Country)
                .Create(),
                _fixture.Build<Person>()
                .With(temp => temp.Email, GenerateUniqueEmail("i"))
                .With(temp => temp.DateOfBirth, GenerateUniqueDateOfBirth())
                .With(temp => temp.Country, null as Country)
                .Create(),
                _fixture.Build<Person>()
                .With(temp => temp.Email, GenerateUniqueEmail("i"))
                .With(temp => temp.DateOfBirth, GenerateUniqueDateOfBirth())
                .With(temp => temp.Country, null as Country)
                .Create()
            };

            List<PersonResponse> personResponsesExpected = persons.Select(x => x.ToPersonResponse()).ToList();
            // print personResponseFromAdd 
            _testOutputHelper.WriteLine("Expected: ");
            foreach (var person in personResponsesExpected)
            {
                _testOutputHelper.WriteLine(person.ToString());
            }

            _personRepositoryMock.Setup(temp => temp.GetFilteredPersons(It.IsAny<Expression<Func<Person, bool>>>()))
                .ReturnsAsync(persons);

            List<PersonResponse> personResponsesFromSearch =
               await _personsService.GetFilteredPersons(nameof(Person.Email), "i");

            _testOutputHelper.WriteLine("Actual: ");
            foreach (var person in personResponsesFromSearch)
            {
                _testOutputHelper.WriteLine(person.ToString());
            }
            personResponsesFromSearch.Should().BeEquivalentTo(personResponsesExpected);
        }

        [Fact]
        public async Task GetFilteredPersons_SearchByAddress_ToBeSuccessful()
        {
            List<Person> persons = new List<Person>
            {
                _fixture.Build<Person>()
                .With(temp => temp.Email, GenerateUniqueEmail("i"))
                .With(temp => temp.DateOfBirth, GenerateUniqueDateOfBirth())
                .With(temp => temp.Country, null as Country)
                .Create(),
                _fixture.Build<Person>()
                .With(temp => temp.Email, GenerateUniqueEmail("i"))
                .With(temp => temp.DateOfBirth, GenerateUniqueDateOfBirth())
                .With(temp => temp.Country, null as Country)
                .Create(),
                _fixture.Build<Person>()
                .With(temp => temp.Email, GenerateUniqueEmail("i"))
                .With(temp => temp.DateOfBirth, GenerateUniqueDateOfBirth())
                .With(temp => temp.Country, null as Country)
                .Create()
            };

            List<PersonResponse> personResponsesExpected = persons.Select(x => x.ToPersonResponse()).ToList();
            // print personResponseFromAdd 
            _testOutputHelper.WriteLine("Expected: ");
            foreach (var person in personResponsesExpected)
            {
                _testOutputHelper.WriteLine(person.ToString());
            }

            _personRepositoryMock.Setup(temp => temp.GetFilteredPersons(It.IsAny<Expression<Func<Person, bool>>>()))
                .ReturnsAsync(persons);

            List<PersonResponse> personResponsesFromSearch =
               await _personsService.GetFilteredPersons(nameof(Person.Address), "N");

            _testOutputHelper.WriteLine("Actual: ");
            foreach (var person in personResponsesFromSearch)
            {
                _testOutputHelper.WriteLine(person.ToString());
            }
            personResponsesFromSearch.Should().BeEquivalentTo(personResponsesExpected);

        }

        #endregion


        #region GetSortededPersons

        [Fact]
        public async Task GetSortedPersons_OrderbyPersonName_ToBeSuccessful()
        {
            List<Person> persons = new List<Person>
            {
                _fixture.Build<Person>()
                .With(temp => temp.Email, GenerateUniqueEmail())
                .With(temp => temp.DateOfBirth, GenerateUniqueDateOfBirth())
                .With(temp => temp.Country, null as Country)
                .Create(),
                _fixture.Build<Person>()
                .With(temp => temp.Email, GenerateUniqueEmail())
                .With(temp => temp.DateOfBirth, GenerateUniqueDateOfBirth())
                .With(temp => temp.Country, null as Country)
                .Create(),
                _fixture.Build<Person>()
                .With(temp => temp.Email, GenerateUniqueEmail())
                .With(temp => temp.DateOfBirth, GenerateUniqueDateOfBirth())
                .With(temp => temp.Country, null as Country)
                .Create()
            };

            List<PersonResponse> personResponsesExpected = persons.Select(x => x.ToPersonResponse()).ToList();

            // print personResponseFromAdd 
            _testOutputHelper.WriteLine("Expected: ");
            foreach (var person in personResponsesExpected)
            {
                _testOutputHelper.WriteLine(person.ToString());
            }

            _personRepositoryMock.Setup(temp => temp.GetAllPersons())
                .ReturnsAsync(persons);

            List<PersonResponse> allPersons = await _personsService.GetAllPersons();

            List<PersonResponse> personResponsesFromSort =
                await _personsService.GetSortedPersons(allPersons, nameof(Person.PersonName), SortOrderOptions.ASC);

            _testOutputHelper.WriteLine("Actual: ");
            foreach (var person in personResponsesFromSort)
            {
                _testOutputHelper.WriteLine(person.ToString());
            }

            personResponsesFromSort.Should()
                .BeInAscendingOrder(x => x.PersonName);


        }

        [Fact]
        public async Task GetSortedPersons_OrderbyEmail_ToBeSuccessful()
        {
            List<Person> persons = new List<Person>
            {
                _fixture.Build<Person>()
                .With(temp => temp.Email, GenerateUniqueEmail())
                .With(temp => temp.DateOfBirth, GenerateUniqueDateOfBirth())
                .With(temp => temp.Country, null as Country)
                .Create(),
                _fixture.Build<Person>()
                .With(temp => temp.Email, GenerateUniqueEmail())
                .With(temp => temp.DateOfBirth, GenerateUniqueDateOfBirth())
                .With(temp => temp.Country, null as Country)
                .Create(),
                _fixture.Build<Person>()
                .With(temp => temp.Email, GenerateUniqueEmail())
                .With(temp => temp.DateOfBirth, GenerateUniqueDateOfBirth())
                .With(temp => temp.Country, null as Country)
                .Create()
            };

            List<PersonResponse> personResponsesExpected = persons.Select(x => x.ToPersonResponse()).ToList();

            // print personResponseFromAdd 
            _testOutputHelper.WriteLine("Expected: ");
            foreach (var person in personResponsesExpected)
            {
                _testOutputHelper.WriteLine(person.ToString());
            }

            _personRepositoryMock.Setup(temp => temp.GetAllPersons())
                .ReturnsAsync(persons);

            List<PersonResponse> allPersons = await _personsService.GetAllPersons();

            List<PersonResponse> personResponsesFromSort =
                await _personsService.GetSortedPersons(allPersons, nameof(Person.Email), SortOrderOptions.DESC);

            _testOutputHelper.WriteLine("Actual: ");
            foreach (var person in personResponsesFromSort)
            {
                _testOutputHelper.WriteLine(person.ToString());
            }

            personResponsesFromSort.Should()
                .BeInDescendingOrder(x => x.Email);
        }

        [Fact]
        public async Task GetSortedPersons_OrderbyDateOfBirth_ToBeSuccessful()
        {
            List<Person> persons = new List<Person>
            {
                _fixture.Build<Person>()
                .With(temp => temp.Email, GenerateUniqueEmail())
                .With(temp => temp.DateOfBirth, GenerateUniqueDateOfBirth())
                .With(temp => temp.Country, null as Country)
                .Create(),
                _fixture.Build<Person>()
                .With(temp => temp.Email, GenerateUniqueEmail())
                .With(temp => temp.DateOfBirth, GenerateUniqueDateOfBirth())
                .With(temp => temp.Country, null as Country)
                .Create(),
                _fixture.Build<Person>()
                .With(temp => temp.Email, GenerateUniqueEmail())
                .With(temp => temp.DateOfBirth, GenerateUniqueDateOfBirth())
                .With(temp => temp.Country, null as Country)
                .Create()
            };

            List<PersonResponse> personResponsesExpected = persons.Select(x => x.ToPersonResponse()).ToList();

            // print personResponseFromAdd 
            _testOutputHelper.WriteLine("Expected: ");
            foreach (var person in personResponsesExpected)
            {
                _testOutputHelper.WriteLine(person.ToString());
            }

            _personRepositoryMock.Setup(temp => temp.GetAllPersons())
                .ReturnsAsync(persons);

            List<PersonResponse> allPersons = await _personsService.GetAllPersons();

            List<PersonResponse> personResponsesFromSort =
                await _personsService.GetSortedPersons(allPersons, nameof(Person.DateOfBirth), SortOrderOptions.ASC);

            _testOutputHelper.WriteLine("Actual: ");
            foreach (var person in personResponsesFromSort)
            {
                _testOutputHelper.WriteLine(person.ToString());
            }

            personResponsesFromSort.Should()
                .BeInAscendingOrder(x => x.DateOfBirth);
        }

        [Fact]
        public async Task GetSortedPersons_OrderbyAge_ToSuccessful()
        {
            List<Person> persons = new List<Person>
            {
                _fixture.Build<Person>()
                .With(temp => temp.Email, GenerateUniqueEmail())
                .With(temp => temp.DateOfBirth, GenerateUniqueDateOfBirth())
                .With(temp => temp.Country, null as Country)
                .Create(),
                _fixture.Build<Person>()
                .With(temp => temp.Email, GenerateUniqueEmail())
                .With(temp => temp.DateOfBirth, GenerateUniqueDateOfBirth())
                .With(temp => temp.Country, null as Country)
                .Create(),
                _fixture.Build<Person>()
                .With(temp => temp.Email, GenerateUniqueEmail())
                .With(temp => temp.DateOfBirth, GenerateUniqueDateOfBirth())
                .With(temp => temp.Country, null as Country)
                .Create()
            };

            List<PersonResponse> personResponsesExpected = persons.Select(x => x.ToPersonResponse()).ToList();

            // print personResponseFromAdd 
            _testOutputHelper.WriteLine("Expected: ");
            foreach (var person in personResponsesExpected)
            {
                _testOutputHelper.WriteLine(person.ToString());
            }

            _personRepositoryMock.Setup(temp => temp.GetAllPersons())
                .ReturnsAsync(persons);

            List<PersonResponse> allPersons = await _personsService.GetAllPersons();

            List<PersonResponse> personResponsesFromSort =
                await _personsService.GetSortedPersons(allPersons, nameof(PersonResponse.Age), SortOrderOptions.ASC);

            _testOutputHelper.WriteLine("Actual: ");
            foreach (var person in personResponsesFromSort)
            {
                _testOutputHelper.WriteLine(person.ToString());
            }

            personResponsesFromSort.Should()
                .BeInAscendingOrder(x => x.Age);
        }
        [Fact]
        public async Task GetSortedPersons_OrderbyAddress_ToBeSuccessful()
        {
            List<Person> persons = new List<Person>
            {
                _fixture.Build<Person>()
                .With(temp => temp.Email, GenerateUniqueEmail())
                .With(temp => temp.DateOfBirth, GenerateUniqueDateOfBirth())
                .With(temp => temp.Country, null as Country)
                .Create(),
                _fixture.Build<Person>()
                .With(temp => temp.Email, GenerateUniqueEmail())
                .With(temp => temp.DateOfBirth, GenerateUniqueDateOfBirth())
                .With(temp => temp.Country, null as Country)
                .Create(),
                _fixture.Build<Person>()
                .With(temp => temp.Email, GenerateUniqueEmail())
                .With(temp => temp.DateOfBirth, GenerateUniqueDateOfBirth())
                .With(temp => temp.Country, null as Country)
                .Create()
            };

            List<PersonResponse> personResponsesExpected = persons.Select(x => x.ToPersonResponse()).ToList();

            // print personResponseFromAdd 
            _testOutputHelper.WriteLine("Expected: ");
            foreach (var person in personResponsesExpected)
            {
                _testOutputHelper.WriteLine(person.ToString());
            }

            _personRepositoryMock.Setup(temp => temp.GetAllPersons())
                .ReturnsAsync(persons);

            List<PersonResponse> allPersons = await _personsService.GetAllPersons();

            List<PersonResponse> personResponsesFromSort =
                await _personsService.GetSortedPersons(allPersons, nameof(Person.Address), SortOrderOptions.ASC);

            _testOutputHelper.WriteLine("Actual: ");
            foreach (var person in personResponsesFromSort)
            {
                _testOutputHelper.WriteLine(person.ToString());
            }

            personResponsesFromSort.Should()
                .BeInAscendingOrder(x => x.Address);

        }

        #endregion


        #region UpdatePerson

        [Fact]
        public async Task UpdatePerson_NullPerson_ToBeArgumentNullException()
        {
            // Arrange
            PersonUpdateRequest? personUpdateRequest = null;

            // Assert
            Func<Task> action = async () =>
            {
                // Acts
                await _personsService.UpdatePerson(personUpdateRequest);
            };
            await action.Should()
                .ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task UpdatePerson_InvalidPersonID_ToBeArgumentException()
        {
            // Arrange
            PersonUpdateRequest? personUpdateRequest = _fixture.Build<PersonUpdateRequest>()
                                                        .Create();

            // Assert
            Func<Task> action = async () =>
            {
                // Acts
                await _personsService.UpdatePerson(personUpdateRequest);
            };
            await action.Should()
                .ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task UpdatePerson_PersonNameIsNull_ToBeArgumentException()
        {
            Person person = _fixture.Build<Person>()
                .With(temp => temp.Email, GenerateUniqueEmail())
                .With(temp => temp.DateOfBirth, GenerateUniqueDateOfBirth())
                .With(temp => temp.Country, null as Country)
                .With(temp => temp.Gender, "Male")
                .Create();
            PersonResponse personResponse = person.ToPersonResponse();
            // Arrange
            PersonUpdateRequest? personUpdateRequest = personResponse.ToPersonUpdateRequest();
            personUpdateRequest.PersonName = null;

            // Assert
            Func<Task> action = async () =>
            {
                // Acts
                await _personsService.UpdatePerson(personUpdateRequest);
            };
            await action.Should()
                .ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task UpdatePerson_PersonFullDetailsUpdation_ToBeSuccessful()
        {
            Person person = _fixture.Build<Person>()
                 .With(temp => temp.Email, GenerateUniqueEmail())
                 .With(temp => temp.Country, null as Country)
                 .With(temp => temp.DateOfBirth, GenerateUniqueDateOfBirth())
                 .With(temp => temp.Gender, "Male")
                 .Create();
            PersonResponse personResponseExpected = person.ToPersonResponse();
            // Arrange
            PersonUpdateRequest? personUpdateRequest = personResponseExpected.ToPersonUpdateRequest();

            _personRepositoryMock.Setup(temp => temp.UpdatePerson(It.IsAny<Person>()))
                .ReturnsAsync(person);

            _personRepositoryMock.Setup(temp => temp.GetPersonByID(It.IsAny<Guid>()))
                .ReturnsAsync(person);

            PersonResponse personResponseFromUpdated = await _personsService.UpdatePerson(personUpdateRequest);

            // Assert
            //Assert.Equal(personResponseFromUpdated, personResponseFromGet);
            personResponseFromUpdated.Should()
                .Be(personResponseExpected);

        }

        #endregion


        #region DeletePerson

        [Fact]
        public async Task DeletePerson_NullPersonID_ToBeArgumentNullException()
        {
            Guid? personID = null;
            // Assert
            var action = async () =>
            {
                // Acts
                await _personsService.DeletePerson(personID);
            };

            await action.Should().ThrowAsync<ArgumentNullException>();

        }

        [Fact]
        public async Task DeletePerson_NotFouldPersonID_ToBeFalse()
        {
            Person person = _fixture.Build<Person>()
                 .With(temp => temp.Email, GenerateUniqueEmail())
                 .With(temp => temp.Country, null as Country)
                 .With(temp => temp.DateOfBirth, GenerateUniqueDateOfBirth())
                 .With(temp => temp.Gender, "Male")
                 .Create();
            Guid? personID = Guid.NewGuid();

            _personRepositoryMock.Setup(temp => temp.GetPersonByID(It.IsAny<Guid>()))
                .ReturnsAsync(person);
            // Assert
            bool isDeleted = await _personsService.DeletePerson(personID);
            isDeleted.Should().BeFalse();
        }

        [Fact]
        public async Task DeletePerson_FouldPersonID_ToBeSuccessful()
        {
            Person person = _fixture.Build<Person>()
                 .With(temp => temp.Email, GenerateUniqueEmail())
                 .With(temp => temp.Country, null as Country)
                 .With(temp => temp.DateOfBirth, GenerateUniqueDateOfBirth())
                 .With(temp => temp.Gender, "Male")
                 .Create();

            PersonResponse personResponse = person.ToPersonResponse();

            Guid? personID = person.PersonID;

            _personRepositoryMock.Setup(temp => temp.GetPersonByID(It.IsAny<Guid>()))
                .ReturnsAsync(person);
            _personRepositoryMock.Setup(temp => temp.DeletePersonByPersonID(It.IsAny<Guid>()))
                .ReturnsAsync(true);
            // Assert
            bool isDeleted = await _personsService.DeletePerson(personID);
            isDeleted.Should().BeTrue();
        }

        #endregion

    }
}
