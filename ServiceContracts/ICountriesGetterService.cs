using ServiceContracts.DTO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ServiceContracts
{
    /// <summary>
    /// Represents business logic for manipulating country entity
    /// </summary>
    public interface ICountriesGetterService
    {
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
    }
}
