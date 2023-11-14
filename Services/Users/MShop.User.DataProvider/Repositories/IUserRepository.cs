using MShop.Infrastructure.Command.User;
using MShop.Infrastructure.Event.User;

namespace MShop.User.DataProvider.Repositories
{
    public interface IUserRepository
    {
        Task<UserCreated> AddUser(CreateUser user);
        Task<UserCreated> GetUser(CreateUser user);
        Task<UserCreated> GetUserByUsername(string username);

    }
}
