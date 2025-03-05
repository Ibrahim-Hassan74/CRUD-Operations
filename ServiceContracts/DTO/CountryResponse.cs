using Entities;

namespace ServiceContracts.DTO
{
    /// <summary>
    /// DTO class that is used as return type for most of CountriesServices methods
    /// </summary>
    public class CountryResponse
    {
        public Guid CountryID { get; set; }
        public string? CountryName { get; set; }
        public override bool Equals(object? obj)
        {
            if (obj is null) return false;
            if(obj.GetType() != typeof(CountryResponse)) return false;
            CountryResponse response = (CountryResponse) obj;
            return response.CountryID == CountryID && response.CountryName == CountryName;
        }
        public override int GetHashCode()
        {
            int hash = CountryID.GetHashCode() * 13 + CountryName!.GetHashCode() * 17;
            return hash;
        }
    }
    public static class ContryExtensions
    {
        public static CountryResponse ToCountryResponse(this Country country)
        {
            return new CountryResponse()
            {
                CountryID = country.CountryID,
                CountryName = country.CountryName,
            };
        }
    }
}
