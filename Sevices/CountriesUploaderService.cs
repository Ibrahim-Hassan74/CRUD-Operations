using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using ServiceContracts;
using ServiceContracts.DTO;
using RepositoryContracts;

namespace Sevices
{
    public class CountriesUploaderService : ICountriesUploaderService
    {
        //private readonly List<Country> _countries;
        private readonly ICountriesRepository _countriesRepository;

        public CountriesUploaderService(ICountriesRepository countriesRepository)
        {
            _countriesRepository = countriesRepository;
        }
        public async Task<int> UploadCountriesFromExcelFile(IFormFile formFile)
        {
            MemoryStream memoryStream = new MemoryStream();
            await formFile.CopyToAsync(memoryStream);
            int countriesInserted = 0;
            using (ExcelPackage excelPackage = new ExcelPackage(memoryStream))
            {
                ExcelWorksheet excelWorksheet = excelPackage.Workbook.Worksheets["Countries"];
                int rows = excelWorksheet.Dimension.Rows;
                for (int i = 2; i <= rows; i++)
                {
                    string? cellValue = Convert.ToString(excelWorksheet.Cells[i, 1].Value);
                    if (!string.IsNullOrEmpty(cellValue))
                    {
                        string countryName = cellValue;

                        if (await _countriesRepository.GetCountryByCountryName(countryName) is null)
                        {
                            Country country = new Country() { CountryName = countryName };
                            //_countriesRepository.Countries.Add(country);
                            //await _countriesRepository.SaveChangesAsync();
                            await _countriesRepository.AddCountry(country);
                            countriesInserted++;
                        }

                    }
                }

            }

            return countriesInserted;
        }
    }
}