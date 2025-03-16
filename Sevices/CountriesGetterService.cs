using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using ServiceContracts;
using ServiceContracts.DTO;
using RepositoryContracts;

namespace Sevices
{
    public class CountriesGetterService : ICountriesGetterService
    {
        //private readonly List<Country> _countries;
        private readonly ICountriesRepository _countriesRepository;

        public CountriesGetterService(ICountriesRepository countriesRepository)
        {
            _countriesRepository = countriesRepository;
        }
        public async Task<List<CountryResponse>> GetAllCountries()
        {
            //return await _countriesRepository.Countries.Select(x => x.ToCountryResponse()).ToListAsync();
            return (await _countriesRepository.GetAllCountries()).Select(x => x.ToCountryResponse()).ToList();
        }

        public async Task<CountryResponse?> GetCountryByCountryID(Guid? countryID)
        {
            if (countryID is null)
                return null;
            //Country? country = await _countriesRepository.Countries.FirstOrDefaultAsync(x => x.CountryID == countryID);
            Country? country = await _countriesRepository.GetCountryByCountryID(countryID.Value);
            if (country is null)
                return null;
            return country.ToCountryResponse();
        }
    }
}