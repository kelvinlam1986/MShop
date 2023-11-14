using MShop.Infrastructure.Command.Product;
using MShop.Infrastructure.Event.Product;
using MShop.Product.DataProvider.Repositories;

namespace MShop.Product.DataProvider.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<ProductCreated> AddProduct(CreateProduct product)
        {
            var createdProduct = await _productRepository.AddProduct(product);
            return createdProduct;
        }

        public async Task<ProductCreated> GetProduct(string productId)
        {
            var product = await _productRepository.GetProduct(productId);
            return product;
        }
    }
}
