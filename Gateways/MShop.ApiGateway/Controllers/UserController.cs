using MassTransit;
using Microsoft.AspNetCore.Mvc;
using MShop.Infrastructure.Authentication;
using MShop.Infrastructure.Command.Product;
using MShop.Infrastructure.Command.User;
using MShop.Infrastructure.Event.User;

namespace MShop.ApiGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IBusControl _bus;
        private readonly IRequestClient<LoginUser> _loginRequestClient;

        public UserController(IBusControl bus, IRequestClient<LoginUser> loginRequestClient)
        {
            _bus = bus;
            _loginRequestClient = loginRequestClient;
        }

        [HttpPost]
        public async Task<ActionResult> Add([FromForm] CreateUser user)
        {
            var uri = new Uri("rabbitmq://localhost/add_user");
            var endpoint = await _bus.GetSendEndpoint(uri);

            await endpoint.Send(user);

            return Accepted("User Created");
        }

        [HttpPost]
        [Route("[Action]")]
        public async Task<ActionResult> Login([FromForm] LoginUser loginUser)
        {
            var userResponse = await _loginRequestClient.GetResponse<JwtAuthToken>(loginUser);
            return Accepted(userResponse.Message);

        }
    }
}
