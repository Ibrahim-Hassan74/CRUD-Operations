using ServiceContracts;
using ServiceContracts.DTO;
using Entities;
using Sevices;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using EntityFrameworkCoreMock;
using Moq;
using AutoFixture;
using RepositoryContracts;
using FluentAssertions;

namespace CRUDTests
{
    public class CountriesServiceTest
    {
        private readonly ICountriesService _countriesServices;
        private readonly Mock<ICountriesRepository> _countriesRepositoryMock;
        private readonly ICountriesRepository _countriesRepository;
        private readonly IFixture _fixture;

        //constructor
        public CountriesServiceTest()
        {
            _countriesRepositoryMock = new Mock<ICountriesRepository>();
            _countriesRepository = _countriesRepositoryMock.Object;
            _fixture = new Fixture();
            var countries = new List<Country>() { };

            DbContextMock<ApplicationDbContext> dbcontextMock = new DbContextMock<ApplicationDbContext>(
                    new DbContextOptionsBuilder<ApplicationDbContext>().Options
                );


            ApplicationDbContext dbContext = dbcontextMock.Object;
            dbcontextMock.CreateDbSetMock(x => x.Countries, countries);


            _countriesServices = new CountriesService(_countriesRepository);
        }

        #region Add Country
        // When CountryAddRequest is null, it should throw ArgumentNullException
        [Fact]
        public async Task AddCountry_NullCountry_ToBeArgumentNullException()
        {
            // Arrange
            CountryAddRequest? request = null;

            // Assert

            Func<Task> action = async () =>
            {
                // Act 
                await _countriesServices.AddCounty(request);
            };
            await action.Should().ThrowExactlyAsync<ArgumentNullException>();
        }

        // When CountryName is null, it should throw ArgumentException 
        [Fact]
        public async Task AddCountry_CountryNameIsNull_ToBeArgumentException()
        {
            // Arrange
            CountryAddRequest? request = new CountryAddRequest()
            {
                CountryName = null
            };
            // Assert
            Func<Task> action = async () =>
            {
                // Act 
                await _countriesServices.AddCounty(request);
            };
            await action.Should().ThrowExactlyAsync<ArgumentException>();
        }

        // When CountryName is duplicate, it should throw ArgumentException  

        [Fact]
        public async Task AddCountry_DuplicateCountryName_ToBeArgumentException()
        {
            // Arrange
            CountryAddRequest? countryAddRequest = _fixture.Create<CountryAddRequest>();


            // Assert
            _countriesRepositoryMock
                .Setup(temp => temp.GetCountryByCountryName(It.IsAny<String>()))
                .ReturnsAsync(countryAddRequest.ToCountry());

            Func<Task> action = async () =>
            {
                // Act 
                await _countriesServices.AddCounty(countryAddRequest);
            };
            await action.Should().ThrowAsync<ArgumentException>();
        }

        // When you supply prop country name it should insert into list of country

        [Fact]
        public async Task AddCountry_ProperCountryDetails_ToBeSuccessful()
        {
            // Arrange
            CountryAddRequest? countryAddRequest = _fixture.Create<CountryAddRequest>();
            Country country = countryAddRequest.ToCountry();
            CountryResponse countryResponseExpected = country.ToCountryResponse();

            _countriesRepositoryMock.Setup(temp => temp.AddCountry(It.IsAny<Country>()))
                .ReturnsAsync(country);

            _countriesRepositoryMock.Setup(temp => temp.GetCountryByCountryName(It.IsAny<String>()))
                .ReturnsAsync(null as Country);

            // Assert
            CountryResponse countryResponseFromGet = await _countriesServices.AddCounty(countryAddRequest);
            countryResponseExpected.CountryID = countryResponseFromGet.CountryID;
            //List<CountryResponse> countryResponse = await _countriesServices.GetAllCountries();

            //Assert 
            //Assert.True(response.CountryID != Guid.Empty);
            bool hasID = countryResponseFromGet.CountryID != Guid.Empty;
            hasID.Should().BeTrue();

            //Assert.Contains(response, countryResponse);
            countryResponseFromGet.Should().Be(countryResponseFromGet);

        }
        #endregion

        #region GetAllCountries

        [Fact]
        public async Task GetAllCountries_ToEmptyList()
        {
            _countriesRepositoryMock.Setup(temp => temp.GetAllCountries())
                .ReturnsAsync(new List<Country>());
            // Acts 
            List<CountryResponse> response = await _countriesServices.GetAllCountries();
            response.Should().BeEmpty();
        }
        [Fact]
        public async Task GetAllCountries_AddFewCountries_ToBeSuccessful()
        {
            List<Country> countries = new List<Country>()
            {
                new Country(){ CountryName = "USA" },
                new Country(){ CountryName = "Egypt" },
                new Country(){ CountryName = "UK" }
            };

            List<CountryResponse> countryResponseExpected = countries.Select(x => x.ToCountryResponse()).ToList();

            _countriesRepositoryMock.Setup(temp => temp.GetAllCountries())
                .ReturnsAsync(countries);

            List<CountryResponse> countryResponseFromGet = await _countriesServices.GetAllCountries();

            countryResponseFromGet.Should().BeEquivalentTo(countryResponseExpected);

        }

        #endregion

        #region GetCountryByCountryID

        [Fact]
        public async Task GetCountryByCountryID_WithNullCountryID()
        {
            // Arrange
            Guid? countryID = null;

            // Act
            CountryResponse? country_response_from_get_method = await _countriesServices.GetCountryByCountryID(countryID);

            // Assert
            //Assert.Null(country_response_from_get_method);
            country_response_from_get_method.Should().BeNull();

        }

        [Fact]
        public async Task GetCountryByCountryID_WithValidCountryID_Successful()
        {
            // Arrange
            Country country = _fixture.Build<Country>()
                                .With(temp => temp.Persons, null as ICollection<Person>)
                                .Create();

            CountryResponse countryResponseExpected = country.ToCountryResponse();
            Guid? countryID = country.CountryID;

            _countriesRepositoryMock.Setup(temp => temp.GetCountryByCountryID(It.IsAny<Guid>()))
                .ReturnsAsync(country);

            // Act
            CountryResponse? countryResponseFromGet = await _countriesServices.GetCountryByCountryID(countryID);

            // Assert
            //Assert.Equal(countryResponseExpected, countryResponseFromGet);
            countryResponseFromGet.Should().Be(countryResponseExpected);
        }

        #endregion

    }
}
