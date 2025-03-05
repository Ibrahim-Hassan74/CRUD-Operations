using ServiceContracts.DTO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ServiceContracts
{
    /// <summary>
    /// Represents business logic for manipulating country entity
    /// </summary>
    public interface ICountriesService
    {
        /// <summary>
        /// Adds a country object to the list of countries 
        /// </summary>
        /// <param name="countryAddRequest">Country object to add</param>
        /// <returns>Return the country object after adding it (including newly generated country id)</returns>
        Task<CountryResponse> AddCounty(CountryAddRequest? countryAddRequest);

        /// <summary>
        /// Returns all countries from the list 
        /// </summary>
        /// <returns>Returns all countries from the list as List of CountryResponse </returns>
        Task<List<CountryResponse>> GetAllCountries();

        /// <summary>
        /// Return a country object based on the given country id
        /// </summary>
        /// <param name="countryID">countryID (guid) to search</param>
        /// <returns>Mathching country object</returns>
        Task<CountryResponse?> GetCountryByCountryID(Guid? countryID);

        /// <summary>
        /// Uploads countries from excel file into database
        /// </summary>
        /// <param name="formFile">Excel file with list of countries</param>
        /// <returns>Returns number of countries added</returns>
        Task<int> UploadCountriesFromExcelFile(IFormFile formFile);
    }
}
