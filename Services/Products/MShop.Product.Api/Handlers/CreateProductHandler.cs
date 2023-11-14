using MassTransit;
using MShop.Infrastructure.Command.Product;
using MShop.Product.DataProvider.Services;

namespace MShop.Product.Api.Handlers
{
    public class CreateProductHandler : IConsumer<CreateProduct>
    {
        private readonly IProductService _productService;

        public CreateProductHandler(IProductService productService)
        {
            _productService = productService;   
        }

        public async Task Consume(ConsumeContext<CreateProduct> context)
        {
            await _productService.AddProduct(context.Message);
            await Task.CompletedTask;
        }
    }
}
