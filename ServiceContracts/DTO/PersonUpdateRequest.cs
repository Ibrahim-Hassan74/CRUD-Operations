using Entities;
using ServiceContracts.Enums;
using System.ComponentModel.DataAnnotations;

namespace ServiceContracts.DTO
{
    /// <summary>
    /// Acts as a DTO for Update a person  
    /// </summary>
    public class PersonUpdateRequest
    {
        [Required(ErrorMessage = "Person ID can't be blank")]
        public Guid PersonID { get; set; }
        [Required(ErrorMessage = "Person Name can't be blank")]
        public string? PersonName { get; set; }
        [Required(ErrorMessage = "Email can't be blank")]
        [EmailAddress(ErrorMessage = "Email value should be a valid")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "{0} can't be blank")]
        [Display(Name = "Date of birth")]
        public DateTime? DateOfBirth { get; set; }
        [Required(ErrorMessage = "{0} can't be blank")]
        public GenderOptions? Gender { get; set; }
        public Guid? CountryID { get; set; }
        public string? Address { get; set; }
        public bool ReciveNewsLetters { get; set; }

        /// <summary>
        /// Converts the current object of PersonAddRequest into a new object of Person type
        /// </summary>
        /// <returns>Returns Person object</returns>
        public Person ToPerson()
        {
            return new Person()
            {
                PersonID = PersonID,
                PersonName = PersonName,
                Email = Email,
                DateOfBirth = DateOfBirth,
                Gender = Gender.ToString(),
                CountryID = CountryID,
                Address = Address,
                ReciveNewsLetters = ReciveNewsLetters
            };
        }
        public override bool Equals(object? obj)
        {
            if (obj is null)
                return false;
            if (obj.GetType() != typeof(PersonUpdateRequest))
                return false;
            PersonUpdateRequest personUpdateRequest = (PersonUpdateRequest)obj;
            return personUpdateRequest.PersonID == PersonID &&
                personUpdateRequest.PersonName == PersonName &&
                personUpdateRequest.Email == Email &&
                personUpdateRequest.DateOfBirth == DateOfBirth &&
                personUpdateRequest.Gender == Gender &&
                personUpdateRequest.Address == Address &&
                personUpdateRequest.ReciveNewsLetters == ReciveNewsLetters &&
                personUpdateRequest.CountryID == CountryID;
        }

        public override int GetHashCode()
        {
            int hash = 3 * PersonID.GetHashCode() +
                5 * (PersonName is null ? 0 : PersonName.GetHashCode()) +
                7 * (Email is null ? 0 : Email.GetHashCode()) +
                11 * DateOfBirth.GetHashCode() +
                13 * Gender.GetHashCode() + 17 * CountryID.GetHashCode() +
                91 * Gender.GetHashCode() +
                23 * (Address is null ? 0 : Address.GetHashCode()) +
                27 * ReciveNewsLetters.GetHashCode() +
                31 * CountryID.GetHashCode();
            return hash;
        }

    }
}
