using AutoMapper;
using Microsoft.Extensions.Logging;
using AutoFixture;
using DataUploader.Domain.Models;
using DataUploader.Audit.Mapper;
using DataUploader.RabbitMQ.Models;
using DataUploader.Domain.Interfaces;
using Moq;

namespace DataUploader.Audit.Tests.Mapper
{
    public class UserActionHistoryAuditProfileTests
    {
        private IMapper _mapper;
        private Mock<IUserRepository> _repository;
        private MapperConfiguration _configuration;

        public UserActionHistoryAuditProfileTests()
        {
            _configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new UserActionHistoryAuditProfile());
            },
            new LoggerFactory());
            _mapper = new AutoMapper.Mapper(_configuration);
            _repository = new Mock<IUserRepository>();
        }

        [Fact]
        public void Configuration_Must_Be_Valid()
        {
            _configuration.AssertConfigurationIsValid();
        }

        [Fact]
        public void UserActionMessageToUserActionHistory_Must_Map_Correctly()
        {
            var row = new Fixture().Create<UserActionMessage>();
            var userAction = _mapper.Map<UserActionHistory>(row);

            Assert.NotNull(userAction);
            Assert.NotNull(userAction.Id);
            Assert.NotEqual(userAction.Id, default);
            Assert.Equal(userAction.FileName, row.FileName);
            Assert.Equal(userAction.FileId, row.FileId);
            Assert.Equal(userAction.EventName, row.EventName);
            Assert.Equal(userAction.EventId, row.EventId);
            Assert.Equal(userAction.ActionDateTime, row.EventDate);
            Assert.Null(userAction.SavedDateTime);
            Assert.Null(userAction.Note);
            Assert.Equal(userAction.UserId, default);
        }

        [Fact]
        public void UserActionMessageToUserActionHistory_Must_MapUserId_Correctly()
        {
            var row = new Fixture().Create<UserActionMessage>();
            var userInfo = new Fixture().Create<UserInfo>();
            _repository.Setup(s => s.FindByFullName(It.IsAny<string>())).Returns(userInfo);
            var userAction = _mapper.Map<UserActionHistory>(row, opt => opt.Items["UserRepository"] = _repository.Object);
            Assert.Equal(userAction.UserId, userInfo.Id);
        }
    }
}