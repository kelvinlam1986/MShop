using MassTransit;
using MShop.Infrastructure.Event.Product;
using MShop.Infrastructure.Query;
using MShop.Product.DataProvider.Services;

namespace MShop.Product.Query.Api.Handlers
{
    public class GetProductByIdHandler : IConsumer<GetProductById>
    {
        private readonly IProductService _productService;

        public GetProductByIdHandler(IProductService productService) 
        {
            _productService = productService;
        }

        public async Task Consume(ConsumeContext<GetProductById> context)
        {
            var product = await _productService.GetProduct(context.Message.ProductId);
            await context.RespondAsync<ProductCreated>(product);
        }
    }
}
