using ServiceContracts.DTO;
using ServiceContracts.Enums;
using System.Runtime.InteropServices;

namespace ServiceContracts
{
    /// <summary>
    /// Represents business logic for manipulating Person entity
    /// </summary>
    public interface IPersonsService
    {
        /// <summary>
        /// Add new Person into the list of Persons
        /// </summary>
        /// <param name="personAddRequest">Person to add</param>
        /// <returns>Return the person details along with newly generated PersonID</returns>
        Task<PersonResponse> AddPerson(PersonAddRequest? personAddRequest);
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
        /// Return all Persons sorted by the given field and order
        /// </summary>
        /// <param name="allPersons">Represent the list of persons to sort </param>
        /// <param name="sortBy">Name of the property (key), based on which the persons should be sorted </param>
        /// <param name="sortOrder">ASC or DESC</param>
        /// <returns>Return sorted persons as PersonResponse list</returns>
        Task<List<PersonResponse>> GetSortedPersons(List<PersonResponse> allPersons, string sortBy, SortOrderOptions sortOrder);

        /// <summary>
        /// Update the person details based on the given person id
        /// </summary>
        /// <param name="personUpdateRequest">Person details to update, including person id</param>
        /// <returns>Returns the Person resopnse object after updating</returns>
        Task<PersonResponse> UpdatePerson(PersonUpdateRequest? personUpdateRequest);
        /// <summary>
        /// Delete person based on the given person id
        /// </summary>
        /// <param name="personID">ID for person you want to delete</param>
        /// <returns>(true / false)</returns>
        Task<bool> DeletePerson(Guid? personID);

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
