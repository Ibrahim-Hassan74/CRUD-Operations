using ServiceContracts.DTO;
using ServiceContracts.Enums;
using System.Runtime.InteropServices;

namespace ServiceContracts
{
    /// <summary>
    /// Represents business logic for manipulating Person entity
    /// </summary>
    public interface IPersonsDeleterService
    {
        /// <summary>
        /// Delete person based on the given person id
        /// </summary>
        /// <param name="personID">ID for person you want to delete</param>
        /// <returns>(true / false)</returns>
        Task<bool> DeletePerson(Guid? personID);
    }
}
