using MShop.Infrastructure.Command.User;
using MShop.Infrastructure.Event.User;

namespace MShop.User.Api.Services
{
    public interface IUserService
    {
        Task<UserCreated> AddUser(CreateUser user);

        Task<UserCreated> GetUser(CreateUser user);
    }
}
