using MongoDB.Bson.Serialization.Attributes;

namespace MShop.Infrastructure.Command.Product
{
    public class CreateProduct
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string? ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }  
        public decimal ProductPrice { get; set; }
        public Guid CategoryId { get; set; }
    }
}
