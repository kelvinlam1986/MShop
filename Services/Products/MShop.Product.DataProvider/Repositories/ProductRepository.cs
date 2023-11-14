using MongoDB.Driver;
using MShop.Infrastructure.Command.Product;
using MShop.Infrastructure.Event.Product;

namespace MShop.Product.DataProvider.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IMongoDatabase _database;
        private readonly IMongoCollection<CreateProduct> _colllection;

        public ProductRepository(IMongoDatabase database)
        {
            _database = database;
            _colllection = database.GetCollection<CreateProduct>("products");
        }

        public async Task<ProductCreated> AddProduct(CreateProduct product)
        {
            await _colllection.InsertOneAsync(product);
            return new ProductCreated { ProductId = product.ProductId, ProductName = product.ProductName,  CreatedDate = DateTime.UtcNow };
        }

        public async Task<ProductCreated> GetProduct(string productId)
        {
            var product =  _colllection.AsQueryable().Where(x => x.ProductId == productId).FirstOrDefault();
            if (product == null)
            {
                throw new Exception("Product not found");
            }

            await Task.CompletedTask;
            return new ProductCreated { ProductId = product.ProductId, ProductName = product.ProductName,  CreatedDate = DateTime.UtcNow };
        }
    }
}
