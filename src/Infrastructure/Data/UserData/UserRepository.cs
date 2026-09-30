using DataUploader.Domain.Interfaces;
using DataUploader.Domain.Models;

namespace DataUploader.User.Data.Repositories
{
    /// <inheritdoc cref="IUserRepository"/>
    public class UserRepository : IUserRepository
    {
        /// <inheritdoc cref="IUserRepository.Find(string)"/>
        public UserInfo Find(string login)
        {
            throw new NotImplementedException();
        }
    }
}
