using MShop.Infrastructure.Command.Product;
using MShop.Infrastructure.Event.Product;

namespace MShop.Product.DataProvider.Repositories
{
    public interface IProductRepository
    {
        Task<ProductCreated> GetProduct(string productId);

        Task<ProductCreated> AddProduct(CreateProduct product);
    }
}
