using DataUploader.Domain.Interfaces;
using DataUploader.Domain.Models;
using DataUploader.Infrastructure.Parsers;
using System.Text;

namespace DataUploader.Infrastructure.Tests.Parsers
{
    public class ExcelParserTests
    {
        private ExcelParser _parser;
        private string _filePath;

        public ExcelParserTests()
        {
            var baseDirectoryPath = string.Join('/', AppContext.BaseDirectory, "TestData");
            _filePath = Path.Combine(baseDirectoryPath, "correct_file.xlsx");
            _parser = new ExcelParser();
        }

        [Fact]
        public void IsInstance_True()
        {
            Assert.IsAssignableFrom<IExcelParser>(_parser);
        }

        [Fact]
        public void ShouldReturnTrueCorrect()
        {
            var streamData = File.OpenRead(_filePath);

            var result = _parser.IsValidate(streamData, out var errors);

            Assert.True(result);
            Assert.Empty(errors);
        }

        [Fact]
        public void ShouldReturnFalseCorrect()
        {
            var byreOfStr = Encoding.UTF8.GetBytes(Guid.NewGuid().ToString());
            var streamData = new MemoryStream(byreOfStr);

            var result = _parser.IsValidate(streamData, out var errors);

            Assert.False(result);
            Assert.NotEmpty(errors);
            Assert.Contains(errors, a => a == "Файл не является Excel, либо его структура нарушена.");
        }

        [Fact]
        public void ShouldReturnFalseIfPageNotExists()
        {
            var streamData = File.OpenRead(_filePath);
            var configuration = new OperationConfiguration()
            {
                PageNumber = 2
            };

            var result = _parser.IsValidate(streamData, out var errors, configuration);

            Assert.False(result);
            Assert.NotEmpty(errors);
            Assert.Contains(errors, a => a == $"Отсутствует страница №{configuration.PageNumber}");
        }

        [Fact]
        public void ShouldReturnFalseIfRowMappingEmpty()
        {
            var streamData = File.OpenRead(_filePath);
            var configuration = new OperationConfiguration()
            {
                ColumnKeysRowNumber = 10000
            };

            var result = _parser.IsValidate(streamData, out var errors, configuration);

            Assert.False(result);
            Assert.NotEmpty(errors);
            Assert.Contains(errors, a => a == $"Отсутсвует сопоставление колонок со сруктурой данных в строке {configuration.ColumnKeysRowNumber.Value}");
        }

        [Fact]
        public void ShouldReturnFalseIfNotData()
        {
            var streamData = File.OpenRead(_filePath);
            var configuration = new OperationConfiguration()
            {
                DataRowNumber = 10000
            };

            var result = _parser.IsValidate(streamData, out var errors, configuration);

            Assert.False(result);
            Assert.NotEmpty(errors);
            Assert.Contains(errors, a => a == $"Файл не содержит данные для загрузки начиная со строки {configuration.DataRowNumber}");
        }
    }
}
