using AutoMapper;
using DataUploader.Domain.Interfaces;
using DataUploader.Domain.Models;

namespace UserData
{
    /// <inheritdoc cref="IUserRepository"/>
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public UserRepository(DataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext ?? throw new ArgumentNullException(nameof(dataContext));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        
        /// <inheritdoc cref="IUserRepository.Find(string)"/>
        public UserInfo Find(string login)
        {
            var user = _dataContext.Users.FirstOrDefault(f => f.LoginName.ToLower() == login.ToLower());
            return _mapper.Map<UserInfo>(user);
        }
    }
}
