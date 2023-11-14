using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MShop.Infrastructure.Command.Product;
using MShop.Infrastructure.Event.Product;
using MShop.Infrastructure.Query;

namespace MShop.ApiGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IBusControl _bus;
        private readonly IRequestClient<GetProductById> _requestClient;

        public ProductController(IBusControl bus, IRequestClient<GetProductById> requestClient)
        {
            _bus = bus;
            _requestClient = requestClient;
        }

        [HttpGet]
        public async Task<ActionResult> Get(string productId)
        {
            var query = new GetProductById { ProductId = productId };
            var product = await _requestClient.GetResponse<ProductCreated>(query);
            return Accepted(product);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Add([FromBody] CreateProduct product)
        {
            var uri = new Uri("rabbitmq://localhost/create_product");
            var endpoint = await _bus.GetSendEndpoint(uri);

            await endpoint.Send(product);

            return Accepted("Product Created");
        }
    }
}
