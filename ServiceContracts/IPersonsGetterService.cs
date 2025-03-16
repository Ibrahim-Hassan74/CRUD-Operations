using ServiceContracts.DTO;
using ServiceContracts.Enums;
using System.Runtime.InteropServices;

namespace ServiceContracts
{
    /// <summary>
    /// Represents business logic for manipulating Person entity
    /// </summary>
    public interface IPersonsGetterService
    {
        /// <summary>
        /// Return all Persons 
        /// </summary>
        /// <returns>Return list of objects of PersonResponse type</returns>
        Task<List<PersonResponse>> GetAllPersons();

        /// <summary>
        /// Returns the person object based on the given person id
        /// </summary>
        /// <param name="personID">Person Id to search</param>
        /// <returns>Return mathching Person object</returns>
        Task<PersonResponse?> GetPersonByPersonID(Guid? personID);
        /// <summary>
        /// Returns all person objects that matches with given search field and search string
        /// </summary>
        /// <param name="searchBy">Search field to search</param>
        /// <param name="searchString">Search string to search</param>
        /// <returns>Retruns all matching persons based on the given search field and search string</returns>
        Task<List<PersonResponse>> GetFilteredPersons(string searchBy, string? searchString);
        /// <summary>
        /// Returns all persons in CSV format
        /// </summary>
        /// <returns>Return the memory stream with csv data of Persons</returns>
        Task<MemoryStream> GetPersonsCSV();

        /// <summary>
        /// Returns Persons as Excel 
        /// </summary>
        /// <returns>Return the memory stream with Execl data of Persons</returns>
        Task<MemoryStream> GetPersonsExcel();
    }
}
