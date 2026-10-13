using AutoMapper;
using Microsoft.Extensions.Logging;
using AutoFixture;
using DataUploader.Domain.Models;
using AuditData.Mapper;

namespace AuditData.Tests.Mapper
{
    public class UserActionAuditProfileTests
    {
        private IMapper _mapper;
        private MapperConfiguration _configuration;

        public UserActionAuditProfileTests()
        {
            _configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new UserActionAuditProfile());
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
        public void UserActionHistoryToUserActionHistory_Must_Map_Correctly()
        {
            var row = new Fixture().Create<Models.UserActionHistory>();
            var fileAction = _mapper.Map<UserActionHistory>(row);

            Assert.NotNull(fileAction);
            Assert.Equal(fileAction.Id, row.Id);
            Assert.Equal(fileAction.FileName, row.FileName);
            Assert.Equal(fileAction.FileId, row.FileId);
            Assert.Equal(fileAction.EventName, row.EventName);
            Assert.Equal(fileAction.EventId, row.EventId);
            Assert.Equal(fileAction.ActionDateTime, row.ActionDateTime);
            Assert.Equal(fileAction.SavedDateTime, row.SavedDateTime);
            Assert.Equal(fileAction.Note, row.Note);
            Assert.Equal(fileAction.UserId, row.UserId);
        }

        [Fact]
        public void UserActionHistoryToUserActionHistory_Revert_Must_Map_Correctly()
        {
            var row = new Fixture().Create<UserActionHistory>();
            var fileAction = _mapper.Map<Models.UserActionHistory>(row);

            Assert.NotNull(fileAction);
            Assert.Equal(fileAction.Id, row.Id);
            Assert.Equal(fileAction.FileName, row.FileName);
            Assert.Equal(fileAction.FileId, row.FileId);
            Assert.Equal(fileAction.EventName, row.EventName);
            Assert.Equal(fileAction.EventId, row.EventId);
            Assert.Equal(fileAction.ActionDateTime, row.ActionDateTime);
            Assert.Equal(fileAction.SavedDateTime, row.SavedDateTime);
            Assert.Equal(fileAction.Note, row.Note);
            Assert.Equal(fileAction.UserId, row.UserId);
        }

        [Fact]
        public void UserActionHistoryToUserActionHistory_Revert_WithEmptyId()
        {
            var row = new Fixture().Create<UserActionHistory>();
            row.Id = null;
            var userAction = _mapper.Map<Models.UserActionHistory>(row);

            Assert.NotEqual(userAction.Id, row.Id);
            Assert.NotEqual(userAction.Id, default);
        }
    }
}