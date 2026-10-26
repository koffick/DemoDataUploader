using AutoMapper;
using DataUploader.Domain.Interfaces;
using DataUploader.Domain.Models;
using UserData.Models;

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

        public UserInfo FindByFullName(string name)
        {
            var users = _dataContext.Users.Where(f => name.ToLower().Contains(f.FirstName.ToLower()) && name.ToLower().Contains(f.LastName.ToLower())).ToArray();
            var userList = _mapper.Map<IEnumerable<UserInfo>>(users);
            var user = userList.FirstOrDefault(f => f.FullName.ToLower() == name.ToLower());
            return _mapper.Map<UserInfo>(user);
        }
    }
}
