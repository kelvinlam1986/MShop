using MassTransit;
using MShop.Infrastructure.Command.User;
using MShop.Infrastructure.Event.User;
using MShop.User.DataProvider.Services;
using MShop.User.DataProvider.Extensions;
using MShop.Infrastructure.Security;
using MShop.Infrastructure.Authentication;

namespace MShop.User.Query.Api.Handlers
{
    public class LoginUserHandler : IConsumer<LoginUser>
    {
        private readonly IUserService _userService;
        private readonly IEncrypter _encrypter;
        private readonly IAuthenticationHandler _authenticationHandler;

        public LoginUserHandler(
            IUserService userService,
            IEncrypter encrypter,
            IAuthenticationHandler authenticationHandler)
        {
            _userService = userService;
            _encrypter = encrypter;
            _authenticationHandler = authenticationHandler;
        }

        public async Task Consume(ConsumeContext<LoginUser> context)
        {
            var token = new JwtAuthToken();

            var user = await _userService.GetUserByUsername(context.Message.Username);
            if (user == null)
            {
                await context.RespondAsync<JwtAuthToken>(token);
            }

            var isAllowed = user.ValidatePassword(context.Message.Password, _encrypter);
            if (isAllowed == false)
            {
                await context.RespondAsync<JwtAuthToken>(token);
            }

            token = _authenticationHandler.Create(user.UserId);
            await context.RespondAsync<JwtAuthToken>(token);
        }
    }
}
