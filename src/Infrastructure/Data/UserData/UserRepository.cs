using DataUploader.Domain.Interfaces;
using DataUploader.Domain.Models;

namespace UserData
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
