using MassTransit;
using Microsoft.AspNetCore.Mvc;
using MShop.Infrastructure.Command.Product;
using MShop.Infrastructure.Command.User;

namespace MShop.ApiGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IBusControl _bus;

        public UserController(IBusControl bus)
        {
            _bus = bus;
        }

        [HttpPost]
        public async Task<ActionResult> Add([FromForm] CreateUser user)
        {
            var uri = new Uri("rabbitmq://localhost/add_user");
            var endpoint = await _bus.GetSendEndpoint(uri);

            await endpoint.Send(user);

            return Accepted("User Created");
        }
    }
}
