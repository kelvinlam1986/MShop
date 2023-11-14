using MShop.Infrastructure.Command.User;
using MShop.Infrastructure.Event.User;
using MShop.Infrastructure.Security;
using MShop.User.DataProvider.Extensions;
using MShop.User.DataProvider.Repositories;

namespace MShop.User.DataProvider.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IEncrypter _encrypter;

        public UserService(
            IUserRepository userRepository,
            IEncrypter encrypter)
        {
            _userRepository = userRepository;
            _encrypter = encrypter;
        }

        public async Task<UserCreated> AddUser(CreateUser user)
        {
            var addedUser = await _userRepository.GetUser(user);
            if (addedUser == null || string.IsNullOrEmpty(addedUser.Username))
            {
                user.SetPassword(_encrypter);
            }
            else
            {
                throw new Exception($"User with username {user.Username} already added");
            }

            var result = await _userRepository.AddUser(user);
            return result;
        }

        public async Task<UserCreated> GetUser(CreateUser user)
        {
            var result = await _userRepository.GetUser(user);
            return result;
        }

        public async Task<UserCreated> GetUserByUsername(string username)
        {
            var result = await _userRepository.GetUserByUsername(username);
            return result;
        }
    }
}
