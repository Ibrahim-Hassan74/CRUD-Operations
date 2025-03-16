using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using ServiceContracts;
using ServiceContracts.DTO;
using RepositoryContracts;

namespace Sevices
{
    public class CountriesAdderService : ICountriesAdderService
    {
        //private readonly List<Country> _countries;
        private readonly ICountriesRepository _countriesRepository;

        public CountriesAdderService(ICountriesRepository countriesRepository)
        {
            _countriesRepository = countriesRepository;
        }
        public async Task<CountryResponse> AddCounty(CountryAddRequest? countryAddRequest)
        {
            if (countryAddRequest is null)
                throw new ArgumentNullException(nameof(countryAddRequest));

            if (countryAddRequest.CountryName is null)
                throw new ArgumentException(nameof(countryAddRequest.CountryName));

            if (await _countriesRepository.GetCountryByCountryName(countryAddRequest.CountryName) is not null)
                throw new ArgumentException("Given Country name already exists");

            Country? country = countryAddRequest.ToCountry();

            country.CountryID = Guid.NewGuid();
            //await _countriesRepository.Countries.AddAsync(country);
            //await _countriesRepository.SaveChangesAsync();
            await _countriesRepository.AddCountry(country);
            return country.ToCountryResponse();

        }
    }
}