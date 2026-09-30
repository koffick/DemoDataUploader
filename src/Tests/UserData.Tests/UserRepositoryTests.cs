using DataUploader.Domain.Interfaces;
using DataUploader.User.Data.Repositories;

namespace DataUploader.User.Data.Tests.Repositories
{
    public class UserRepositoryTests
    {
        private UserRepository _userRepository;

        public UserRepositoryTests()
        {
            _userRepository = new UserRepository();
        }

        [Fact]
        public void IsInstance_True()
        {
            Assert.IsAssignableFrom<IUserRepository>(_userRepository);
        }
    }
}
