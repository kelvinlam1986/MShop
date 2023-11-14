using MShop.Infrastructure.Command.Product;
using MShop.Infrastructure.Event.Product;

namespace MShop.Product.DataProvider.Services
{ 
    public interface IProductService
    {
        Task<ProductCreated> GetProduct(string productId);
        Task<ProductCreated> AddProduct(CreateProduct product);
    }
}
