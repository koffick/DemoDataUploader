using AutoMapper;
using Microsoft.Extensions.Logging;
using AutoFixture;
using DataUploader.Domain.Models;
using AuditData.Mapper;

namespace AuditData.Tests.Mapper
{
    public class FileActionAuditProfileTests
    {
        private IMapper _mapper;
        private MapperConfiguration _configuration;

        public FileActionAuditProfileTests()
        {
            _configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new FileActionAuditProfile());
            },
            new LoggerFactory());
            _mapper = new AutoMapper.Mapper(_configuration);
        }

        [Fact]
        public void Configuration_Must_Be_Valid()
        {
            _configuration.AssertConfigurationIsValid();
        }

        [Fact]
        public void FileActionHistoryToFileActionHistory_Must_Map_Correctly()
        {
            var row = new Fixture().Create<Models.FileActionHistory>();
            var fileAction = _mapper.Map<FileActionHistory>(row);

            Assert.NotNull(fileAction);
            Assert.Equal(fileAction.Id, row.Id);
            Assert.Equal(fileAction.FileId, row.FileId);
            Assert.Equal(fileAction.ActionType, row.ActionType);
            Assert.Equal(fileAction.ActionDateTime, row.ActionDateTime);
            Assert.Equal(fileAction.SavedDateTime, row.SavedDateTime);
            Assert.Equal(fileAction.Note, row.Note);
        }

        [Fact]
        public void FileActionHistoryToFileActionHistory_Revert_Must_Map_Correctly()
        {
            var row = new Fixture().Create<FileActionHistory>();
            var fileAction = _mapper.Map<Models.FileActionHistory>(row);

            Assert.NotNull(fileAction);
            Assert.Equal(fileAction.Id, row.Id);
            Assert.Equal(fileAction.FileId, row.FileId);
            Assert.Equal(fileAction.ActionType, row.ActionType);
            Assert.Equal(fileAction.ActionDateTime, row.ActionDateTime);
            Assert.Equal(fileAction.SavedDateTime, row.SavedDateTime);
            Assert.Equal(fileAction.Note, row.Note);
        }

        [Fact]
        public void FileActionHistoryToFileActionHistory_Revert_WithEmptyId()
        {
            var row = new Fixture().Create<FileActionHistory>();
            row.Id = null;
            var fileAction = _mapper.Map<Models.FileActionHistory>(row);

            Assert.NotEqual(fileAction.Id, row.Id);
            Assert.NotEqual(fileAction.Id, default);
        }
    }
}