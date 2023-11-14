using MassTransit;
using MShop.Infrastructure.Command.User;
using MShop.Infrastructure.Event.User;
using MShop.User.DataProvider.Services;
using MShop.User.DataProvider.Extensions;
using MShop.Infrastructure.Security;

namespace MShop.User.Query.Api.Handlers
{
    public class LoginUserHandler : IConsumer<LoginUser>
    {
        private readonly IUserService _userService;
        private readonly IEncrypter _encrypter;

        public LoginUserHandler(
            IUserService userService,
            IEncrypter encrypter)
        {
            _userService = userService;
            _encrypter = encrypter;
        }

        public async Task Consume(ConsumeContext<LoginUser> context)
        {
            var user = await _userService.GetUserByUsername(context.Message.Username);
            if (user == null)
            {
                var emptyUserCreated = new UserCreated();
                await context.RespondAsync<UserCreated>(emptyUserCreated);
            }

            var isAllowed = user.ValidatePassword(context.Message.Password, _encrypter);
            if (isAllowed == false)
            {
                var emptyUserCreated = new UserCreated();
                await context.RespondAsync<UserCreated>(emptyUserCreated);
            }

            await context.RespondAsync<UserCreated>(user);
        }
    }
}
