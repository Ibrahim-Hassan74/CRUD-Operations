using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryContracts
{
    /// <summary>
    /// Repository data access logic for managing Person entity
    /// </summary>
    public interface IPersonsRepository
    {
        /// <summary>
        /// Adds a Person object to data store
        /// </summary>
        /// <param name="person">Person object to add</param>
        /// <returns>Returns the person object after adding</returns>
        Task<Person> AddPerson(Person person);
        /// <summary>
        /// Return all persons in the data store
        /// </summary>
        /// <returns>List of person object from table</returns>
        Task<List<Person>> GetAllPersons();
        /// <summary>
        /// Returns a person object based on the given person id
        /// </summary>
        /// <param name="personID">PersonID (Guid) to search</param>
        /// <returns>A person object or null</returns>
        Task<Person?> GetPersonByID(Guid personID);

        /// <summary>
        /// Returns all person object base on the given expression 
        /// </summary>
        /// <param name="predicate">LINQ expression</param>
        /// <returns>All matching persons with given condition</returns>
        Task<List<Person>> GetFilteredPersons(Expression<Func<Person, bool>> predicate);

        /// <summary>
        /// Deletes a persons object based on the person id
        /// </summary>
        /// <param name="personID">PersonID (Guid) to search</param>
        /// <returns>Returns true, if the deletion is successful otherwise false</returns>
        Task<bool> DeletePersonByPersonID(Guid personID);

        /// <summary>
        /// Updates a person object (person name - email - .....)
        /// </summary>
        /// <param name="person">Person object to update</param>
        /// <returns>Returns the update person object</returns>
        Task<Person> UpdatePerson(Person person);

    }
}
