using MShop.Infrastructure.Command.User;
using MShop.Infrastructure.Event.User;
using MShop.User.Api.Repositories;

namespace MShop.User.Api.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserCreated> AddUser(CreateUser user)
        {
            var result = await _userRepository.AddUser(user);
            return result;
        }

        public async Task<UserCreated> GetUser(CreateUser user)
        {
            var result = await _userRepository.GetUser(user);
            return result;
        }
    }
}
