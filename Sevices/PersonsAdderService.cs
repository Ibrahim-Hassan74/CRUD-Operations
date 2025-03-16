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
    public class PersonsAdderService : IPersonsAdderService
    {
        private readonly IPersonsRepository _personsRepository;
        private readonly ILogger<PersonsAdderService> _logger;
        private readonly IDiagnosticContext _diagnosticContext;
        //private readonly ICountriesService _countries;
        public PersonsAdderService(IPersonsRepository personsRepository, ILogger<PersonsAdderService> logger, IDiagnosticContext diagnosticContext)
        {
            //_countries = countriesService;
            _personsRepository = personsRepository;
            _logger = logger;
            _diagnosticContext = diagnosticContext;
        }

        public async Task<PersonResponse> AddPerson(PersonAddRequest? personAddRequest)
        {
            if (personAddRequest is null)
                throw new ArgumentNullException(nameof(personAddRequest));

            #region validtion with if statament
            //if (personAddRequest.PersonName is null ||
            //    personAddRequest.Email is null ||
            //    personAddRequest.Address is null ||
            //    personAddRequest.DateOfBirth is null ||
            //    personAddRequest.Gender is null ||
            //    personAddRequest.CountryID is null)
            //    throw new ArgumentException(nameof(personAddRequest.PersonName));
            //string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            //if (!Regex.IsMatch(personAddRequest?.Email, pattern))
            //    throw new FormatException(nameof(personAddRequest.Email));
            #endregion

            VaildationHelper.ModelValidation(personAddRequest);

            if (personAddRequest.DateOfBirth.HasValue && personAddRequest.DateOfBirth.Value.Year > 2010)
                throw new InvalidOperationException(nameof(personAddRequest.DateOfBirth));

            Person person = personAddRequest.ToPerson();
            person.PersonID = Guid.NewGuid();

            //await _personsRepository.Persons.AddAsync(person);
            //await _personsRepository.SaveChangesAsync();
            await _personsRepository.AddPerson(person);
            //_db.sp_InsertPerson(person);

            return person.ToPersonResponse();
        }

    }
}
