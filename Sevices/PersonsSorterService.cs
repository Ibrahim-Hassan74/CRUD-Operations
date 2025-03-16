using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Sevices.Helpers;
using System.Reflection;
using ServiceContracts.Enums;
using Microsoft.EntityFrameworkCore;
using CsvHelper;
using System.Globalization;
using CsvHelper.Configuration;
using OfficeOpenXml;
using RepositoryContracts;
using System.Linq.Expressions;
using Microsoft.Extensions.Logging;
using Serilog;
using SerilogTimings;
using Excetions;

namespace Sevices
{
    public class PersonsSorterService : IPersonsSorterService
    {
        private readonly IPersonsRepository _personsRepository;
        private readonly ILogger<PersonsSorterService> _logger;
        private readonly IDiagnosticContext _diagnosticContext;
        //private readonly ICountriesService _countries;
        public PersonsSorterService(IPersonsRepository personsRepository, ILogger<PersonsSorterService> logger, IDiagnosticContext diagnosticContext)
        {
            //_countries = countriesService;
            _personsRepository = personsRepository;
            _logger = logger;
            _diagnosticContext = diagnosticContext;
        }

        private List<PersonResponse> GetSortedPersons(List<PersonResponse> allPersons, PropertyInfo propertyInfo, SortOrderOptions sortOrder)
        {

            if (sortOrder == SortOrderOptions.ASC)
            {
                if (propertyInfo.PropertyType == typeof(string))
                    return allPersons
                        .OrderBy(x => (string?)propertyInfo.GetValue(x), StringComparer.OrdinalIgnoreCase)
                        .ToList();
                return allPersons
                    .OrderBy(x => propertyInfo.GetValue(x))
                    .ToList();
            }
            if (propertyInfo.PropertyType == typeof(string))
                return allPersons
                    .OrderByDescending(x => (string?)propertyInfo.GetValue(x), StringComparer.OrdinalIgnoreCase)
                    .ToList();
            return allPersons
                .OrderByDescending(x => propertyInfo.GetValue(x))
                .ToList();
        }

        public async Task<List<PersonResponse>> GetSortedPersons(List<PersonResponse> allPersons, string sortBy, SortOrderOptions sortOrder)
        {
            _logger.LogInformation("GetSortedPersons of PersonsService");
            if (string.IsNullOrEmpty(sortBy))
                return allPersons;
            PropertyInfo? propertyInfo = typeof(PersonResponse).GetProperty(sortBy,
                BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            if (propertyInfo is null)
                return allPersons;

            if (sortOrder == SortOrderOptions.ASC)
                return GetSortedPersons(allPersons, propertyInfo, sortOrder);

            return GetSortedPersons(allPersons, propertyInfo, sortOrder);
        }
    }
}
