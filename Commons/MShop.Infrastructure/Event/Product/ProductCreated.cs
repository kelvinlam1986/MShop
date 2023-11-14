namespace MShop.Infrastructure.Event.Product
{
    public class ProductCreated
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
