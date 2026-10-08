using AutoFixture;
using AutoMapper;
using DataUploader.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using UserData.Mapper;

namespace UserData.Tests
{
    public class UserRepositoryTests
    {
        private UserRepository _userRepository;
        private DataContext _dataContext;

        public UserRepositoryTests()
        {
            var contextOptions = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase("UserRepositoryTest")
                .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;

            _dataContext = new DataContext(contextOptions);

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new UserProfile());
            },
            new LoggerFactory());
            var mapper = new AutoMapper.Mapper(configuration);

            _userRepository = new UserRepository(_dataContext, mapper);
        }

        [Fact]
        public void IsInstance_True()
        {
            Assert.IsAssignableFrom<IUserRepository>(_userRepository);
        }

        [Fact]
        public void Find_Must_Be_Return_User()
        {
            var user = new Fixture().Create<Models.User>();
            _dataContext.Add(user);
            _dataContext.SaveChanges();

            var userInfo = _userRepository.Find(user.LoginName);
            Assert.NotNull(userInfo);
            Assert.Equal(userInfo.Id, user.Id);
        }
    }
}
