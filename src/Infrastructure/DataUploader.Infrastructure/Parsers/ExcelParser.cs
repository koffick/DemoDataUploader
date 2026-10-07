using ClosedXML.Excel;
using DataUploader.Domain.Interfaces;
using DataUploader.Domain.Models;

namespace DataUploader.Infrastructure.Parsers
{
    /// <inheritdoc cref="IExcelParser"/>
    public class ExcelParser : IExcelParser
    {
        /// <inheritdoc cref="IExcelParser.IsValidate(Stream, out IEnumerable{string}, OperationConfiguration?)"/>
        public bool IsValidate(Stream data, out IEnumerable<string> errors, OperationConfiguration? configuration = null)
        {
            var errorsReturn = new List<string>();
            errors = errorsReturn;
            try
            {
                configuration = configuration ?? new OperationConfiguration();
                var workbook = new XLWorkbook(data);
                var worksheet = workbook.Worksheets.Worksheet(configuration.PageNumber);
                if (configuration.ColumnKeysRowNumber.HasValue)
                {
                    var columnKeysRow = worksheet.Row(configuration.ColumnKeysRowNumber.Value);
                    if (columnKeysRow.Cells().All(a => a.DataType == XLDataType.Blank ||
                        string.IsNullOrEmpty(a.Value.GetText())))
                    {
                        errorsReturn.Add($"Отсутсвует сопоставление колонок со сруктурой данных в строке {configuration.ColumnKeysRowNumber.Value}");
                        return false;
                    }
                }
                var dataRows = worksheet.Rows(configuration.DataRowNumber, worksheet.RowsUsed(XLCellsUsedOptions.All).Count());
                if (dataRows.Count() == 0)
                {
                    errorsReturn.Add($"Файл не содержит данные для загрузки начиная со строки {configuration.DataRowNumber}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                if (ex.Message == "File contains corrupted data.")
                {
                    errorsReturn.Add("Файл не является Excel, либо его структура нарушена.");
                }
                else if (ex.Message == "There isn't a worksheet associated with that position.")
                {
                    errorsReturn.Add($"Отсутствует страница №{configuration?.PageNumber}");
                }
                else if (ex.Message == "Specified cast is not valid.")
                {
                    errorsReturn.Add($"Указанное преобразование типов недопустимо.");
                }
                else
                {
                    errorsReturn.Add(ex.Message);
                }
                return false;
            }

            return true;
        }
    }
}
