using AutoMapper;
using Microsoft.Extensions.Logging;
using AutoFixture;
using DataUploader.Domain.Models;
using DataUploader.Audit.Mapper;
using DataUploader.RabbitMQ.Models;
using DataUploader.Domain.Interfaces;
using Moq;
using DataUploader.Domain.Enums;

namespace DataUploader.Audit.Tests.Mapper
{
    public class FileActionHistoryAuditProfileTests
    {
        private IMapper _mapper;
        private MapperConfiguration _configuration;

        public FileActionHistoryAuditProfileTests()
        {
            _configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new FileActionHistoryAuditProfile());
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
        public void FileActionMessageToUserActionHistory_Must_Map_Correctly()
        {
            var row = new Fixture().Create<FileToProccessMessage>();
            var fileAction = _mapper.Map<FileActionHistory>(row, opt => opt.Items["ActionType"] = nameof(ActionTypeEnum.FileToProccess));

            Assert.NotNull(fileAction);
            Assert.NotNull(fileAction.Id);
            Assert.NotEqual(fileAction.Id, default);
            Assert.Equal(fileAction.FileId, row.FileId);
            Assert.Equal(fileAction.ActionDateTime, row.EventDate);
            Assert.Null(fileAction.SavedDateTime);
            Assert.Null(fileAction.Note);
            Assert.Equal(fileAction.ActionType, nameof(ActionTypeEnum.FileToProccess));
        }

        [Fact]
        public void FileActionMessageToUserActionHistory_MapWithoutActionType_Correctly()
        {
            var row = new Fixture().Create<FileToProccessMessage>();
            var fileAction = _mapper.Map<FileActionHistory>(row);

            Assert.NotNull(fileAction);
            Assert.NotNull(fileAction.Id);
            Assert.NotEqual(fileAction.Id, default);
            Assert.Equal(fileAction.FileId, row.FileId);
            Assert.Equal(fileAction.ActionDateTime, row.EventDate);
            Assert.Null(fileAction.SavedDateTime);
            Assert.Null(fileAction.Note);
            Assert.Null(fileAction.ActionType);
        }
    }
}