using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using RepositoryContracts;
using Serilog;
using ServiceContracts.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sevices
{
    public class PersonsGetterServiceChild : PersonsGetterService
    {
        private readonly IPersonsRepository _personsRepository;
        private readonly ILogger<PersonsGetterService> _logger;
        private readonly IDiagnosticContext _diagnosticContext;

        public PersonsGetterServiceChild(IPersonsRepository personsRepository, ILogger<PersonsGetterService> logger, IDiagnosticContext diagnosticContext) : base(personsRepository, logger, diagnosticContext)
        {
            _personsRepository = personsRepository;
            _logger = logger;
            _diagnosticContext = diagnosticContext;
        }

        public override async Task<MemoryStream> GetPersonsExcel()
        {
            MemoryStream memoryStream = new MemoryStream();

            using (ExcelPackage excelPackage = new ExcelPackage(memoryStream))
            {
                ExcelWorksheet excelWorksheet = excelPackage.Workbook.Worksheets.Add("PersonsSheet");

                excelWorksheet.Cells["A1"].Value = "Person Name";
                excelWorksheet.Cells["B1"].Value = "Age";
                excelWorksheet.Cells["C1"].Value = "Gender";

                using (ExcelRange headerCells = excelWorksheet.Cells["A1:C1"])
                {
                    headerCells.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    headerCells.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                    headerCells.Style.Font.Bold = true;
                }

                int row = 2;
                List<PersonResponse> persons = await GetAllPersons();

                foreach (var person in persons)
                {
                    excelWorksheet.Cells[row, 1].Value = person.PersonName;
                    excelWorksheet.Cells[row, 2].Value = person.Age;
                    excelWorksheet.Cells[row++, 3].Value = person.Gender;
                }

                excelWorksheet.Cells[$"A1:C{row}"].AutoFitColumns();
                await excelPackage.SaveAsync();

            }

            memoryStream.Position = 0;
            return memoryStream;
        }

    }
}
