using ServiceContracts.DTO;
using ServiceContracts.Enums;
using System.Runtime.InteropServices;

namespace ServiceContracts
{
    /// <summary>
    /// Represents business logic for manipulating Person entity
    /// </summary>
    public interface IPersonsSorterService
    {
        /// <summary>
        /// Return all Persons sorted by the given field and order
        /// </summary>
        /// <param name="allPersons">Represent the list of persons to sort </param>
        /// <param name="sortBy">Name of the property (key), based on which the persons should be sorted </param>
        /// <param name="sortOrder">ASC or DESC</param>
        /// <returns>Return sorted persons as PersonResponse list</returns>
        Task<List<PersonResponse>> GetSortedPersons(List<PersonResponse> allPersons, string sortBy, SortOrderOptions sortOrder);
    }
}
