using ServiceContracts.DTO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ServiceContracts
{
    /// <summary>
    /// Represents business logic for manipulating country entity
    /// </summary>
    public interface ICountriesAdderService
    {
        /// <summary>
        /// Adds a country object to the list of countries 
        /// </summary>
        /// <param name="countryAddRequest">Country object to add</param>
        /// <returns>Return the country object after adding it (including newly generated country id)</returns>
        Task<CountryResponse> AddCounty(CountryAddRequest? countryAddRequest);
    }
}
