using Entities;
using ServiceContracts.Enums;

namespace ServiceContracts.DTO
{
    /// <summary>
    /// Represents DTO class that is used as return type of most methods of Person Service
    /// </summary>
    public class PersonResponse
    {
        public Guid PersonID { get; set; }
        public string? PersonName { get; set; }
        public string? Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public Guid? CountryID { get; set; }
        public string? Country { get; set; }
        public string? Address { get; set; }
        public bool ReciveNewsLetters { get; set; }
        public double? Age { get; set; }

        /// <summary>
        /// Compares the current object data with the parameter object
        /// </summary>
        /// <param name="obj">the PersonResponse object to Compare</param>
        /// <returns>true or false, indicating whether all person details are matched with the specified parameter object</returns>
        public override bool Equals(object? obj)
        {
            if (obj is null)
                return false;
            if (obj.GetType() != typeof(PersonResponse))
                return false;
            PersonResponse personResponse = (PersonResponse)obj;
            return personResponse.PersonID == PersonID &&
                personResponse.PersonName == PersonName &&
                personResponse.Email == Email &&
                personResponse.DateOfBirth == DateOfBirth &&
                personResponse.Gender == Gender &&
                personResponse.Country == Country &&
                personResponse.Address == Address &&
                personResponse.ReciveNewsLetters == ReciveNewsLetters &&
                personResponse.Age == Age &&
                personResponse.CountryID == CountryID;
        }

        public override int GetHashCode()
        {
            int hash = 3 * PersonID.GetHashCode() +
                5 * PersonName.GetHashCode() +
                7 * Email.GetHashCode() +
                11 * DateOfBirth.GetHashCode() +
                13 * Gender.GetHashCode() + 17 * CountryID.GetHashCode() +
                23 * Address.GetHashCode() +
                27 * ReciveNewsLetters.GetHashCode() +
                31 * CountryID.GetHashCode() +
                33 * Country.GetHashCode() +
                37 * Age.GetHashCode();
            return hash;
        }
        public override string ToString()
        {
            return $"PersonID: {PersonID}, " +
               $"PersonName: {PersonName ?? "N/A"}, " +
               $"Email: {Email ?? "N/A"}, " +
               $"DateOfBirth: {DateOfBirth?.ToShortDateString() ?? "N/A"}, " +
               $"Gender: {Gender ?? "N/A"}, " +
               $"CountryID: {CountryID?.ToString() ?? "N/A"}, " +
               $"Country: {Country ?? "N/A"}, " +
               $"Address: {Address ?? "N/A"}, " +
               $"ReciveNewsLetters: {ReciveNewsLetters}, " +
               $"Age: {Age?.ToString() ?? "N/A"}";
        }

        public PersonUpdateRequest ToPersonUpdateRequest()
        {
            return new PersonUpdateRequest()
            {
                PersonID = PersonID,
                PersonName = PersonName,
                Email = Email,
                DateOfBirth = DateOfBirth,
                Gender = (GenderOptions)Enum.Parse(typeof(GenderOptions), Gender, true),
                Address = Address,
                CountryID = CountryID,
                ReciveNewsLetters = ReciveNewsLetters,
            };
        }
    }

    public static class PersonExtensions
    {
        /// <summary>
        /// An Extension method to convert an object of Person class into PersonResponse class 
        /// </summary>
        /// <param name="person">The Person object to convert</param>
        /// <returns>Return the converted PersonResonse object</returns>
        public static PersonResponse ToPersonResponse(this Person? person)
        {
            return new PersonResponse()
            {
                PersonID = person.PersonID,
                PersonName = person.PersonName,
                Email = person.Email,
                DateOfBirth = person.DateOfBirth,
                Gender = person.Gender,
                Address = person.Address,
                ReciveNewsLetters = person.ReciveNewsLetters,
                CountryID = person.CountryID,
                Age = (person.DateOfBirth is not null) ? Math.Round((DateTime.Now - person.DateOfBirth.Value).TotalDays / 365.25) : null,
                Country = person.Country?.CountryName,
            };
        }
        //private  PersonResponse ConvertPersonToPersonResponse(Person person)
        //{
        //    PersonResponse personResponse = person.ToPersonResponse();
        //    personResponse.Country = person.Country?.CountryName;
        //    return personResponse;
        //}
    }
}
