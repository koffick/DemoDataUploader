using AutoMapper;
using Microsoft.Extensions.Logging;
using AutoFixture;
using DataUploader.Domain.Models;
using UserData.Mapper;

namespace UserData.Tests.Mapper
{
    public class UserProfileTests
    {
        private IMapper _mapper;
        private MapperConfiguration _configuration;

        public UserProfileTests()
        {
            _configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new UserProfile());
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
        public void UserToUserInfo_Must_Map_Correctly()
        {
            var row = new Fixture().Create<Models.User>();
            var userInfo = _mapper.Map<UserInfo>(row);

            Assert.NotNull(userInfo);
            Assert.Equal(userInfo.Id, row.Id);
            Assert.Equal(userInfo.LoginName, row.LoginName);
            Assert.Equal(userInfo.FirstName, row.FirstName);
            Assert.Equal(userInfo.LastName, row.LastName);
        }
    }
}