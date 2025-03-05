using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    /// <summary>
    /// Person Domain model class
    /// </summary>
    public class Person
    {
        [Key]
        public Guid PersonID { get; set; }
        // nvarchar(max)
        [StringLength(50)] // nvarchar(50)
        public string? PersonName { get; set; }
        [StringLength(50)] // nvarchar(50)
        public string? Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        [StringLength(10)] // nvarchar(10)
        public string? Gender { get; set; }
        // uniqeidentifier in sql
        public Guid? CountryID { get; set; }
        [StringLength(200)] // nvarchar(200)
        public string? Address { get; set; }
        // bit
        public bool ReciveNewsLetters { get; set; }
        public string? TIN { get; set; }

        [ForeignKey(nameof(CountryID))]
        public virtual Country? Country { get; set; }
        public override string ToString()
        {
            return $"Person ID: {PersonID}, " +
                $"Person Name: {PersonName}, " +
                $"Email: {Email}, " +
                $"Date of birth: {DateOfBirth?.ToString("MM/dd/yyyy")}, " +
                $"Gender: {Gender}, " +
                $"Country ID: {CountryID}, Address: {Address}, " +
                $"Recieve news letters: {ReciveNewsLetters}, " +
                $"Country: {Country?.CountryName}";
        }

    }
}
