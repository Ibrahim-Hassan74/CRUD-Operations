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
    public class PersonsUpdaterService : IPersonsUpdaterService
    {
        private readonly IPersonsRepository _personsRepository;
        private readonly ILogger<PersonsUpdaterService> _logger;
        private readonly IDiagnosticContext _diagnosticContext;
        //private readonly ICountriesService _countries;
        public PersonsUpdaterService(IPersonsRepository personsRepository, ILogger<PersonsUpdaterService> logger, IDiagnosticContext diagnosticContext)
        {
            //_countries = countriesService;
            _personsRepository = personsRepository;
            _logger = logger;
            _diagnosticContext = diagnosticContext;
        }
        public async Task<PersonResponse> UpdatePerson(PersonUpdateRequest? personUpdateRequest)
        {
            if (personUpdateRequest is null)
                throw new ArgumentNullException(nameof(personUpdateRequest));


            VaildationHelper.ModelValidation(personUpdateRequest);

            Person? person = await _personsRepository.GetPersonByID(personUpdateRequest.PersonID);
            if (person is null)
                throw new InvalidPersonIDException("Given Person ID doesn't exist");

            person.PersonName = personUpdateRequest.PersonName;
            person.Email = personUpdateRequest.Email;
            person.DateOfBirth = personUpdateRequest.DateOfBirth;
            person.Gender = personUpdateRequest.Gender.ToString();
            person.CountryID = personUpdateRequest.CountryID;
            person.Address = personUpdateRequest.Address;
            person.ReciveNewsLetters = personUpdateRequest.ReciveNewsLetters;
            await _personsRepository.UpdatePerson(person);

            //await _personsRepository.SaveChangesAsync();
            //_db.sp_UpdatePerson(personUpdateRequest.ToPerson());

            return person.ToPersonResponse();
        }
    }
}
