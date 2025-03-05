using Entities;

namespace RepositoryContracts
{
    /// <summary>
    /// Repository data access logic for managing Person Entity
    /// </summary>
    public interface ICountriesRepository
    {
        /// <summary>
        /// Adds a new Country object to the data store
        /// </summary>
        /// <param name="person">Country object to add</param>
        /// <returns>Returns the country object after adding it to data store</returns>
        Task<Country> AddCountry(Country country);
        /// <summary>
        /// Get all countries in the data store
        /// </summary>
        /// <returns>All countries from the table</returns>
        Task<List<Country>> GetAllCountries();
        /// <summary>
        /// Return Country object base on given country id otherwise, it return null
        /// </summary>
        /// <param name="countryID">Country ID to search</param>
        /// <returns>Matching country or null</returns>
        Task<Country?> GetCountryByCountryID(Guid countryID);

        /// <summary>
        /// Return country object base on given country name, otherwise, it return null
        /// </summary>
        /// <param name="countryName">country name to search</param>
        /// <returns>Matching country or null</returns>
        Task<Country?> GetCountryByCountryName(string countryName);

    }
}
