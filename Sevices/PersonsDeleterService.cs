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
    public class PersonsDeleterService : IPersonsDeleterService
    {
        private readonly IPersonsRepository _personsRepository;
        private readonly ILogger<PersonsDeleterService> _logger;
        private readonly IDiagnosticContext _diagnosticContext;
        //private readonly ICountriesService _countries;
        public PersonsDeleterService(IPersonsRepository personsRepository, ILogger<PersonsDeleterService> logger, IDiagnosticContext diagnosticContext)
        {
            //_countries = countriesService;
            _personsRepository = personsRepository;
            _logger = logger;
            _diagnosticContext = diagnosticContext;
        }
        public async Task<bool> DeletePerson(Guid? personID)
        {
            if (personID is null)
                throw new ArgumentNullException(nameof(personID));
            Person? person = await _personsRepository.GetPersonByID(personID.Value);
            if (person is null)
                return false;
            //await _personsRepository.SaveChangesAsync();

            return await _personsRepository.DeletePersonByPersonID(personID.Value);
        }
    }
}
