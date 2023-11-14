using MassTransit;
using MShop.Infrastructure.Command.User;
using MShop.User.Api.Services;

namespace MShop.User.Api.Handlers
{
    public class CreateUserHandler : IConsumer<CreateUser>
    {
        private readonly IUserService _userService;

        public CreateUserHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task Consume(ConsumeContext<CreateUser> context)
        {
            var createdUser = await _userService.AddUser(context.Message);

        }
    }
}
