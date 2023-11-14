namespace MShop.Infrastructure.Mongo
{
    public interface IDatabaseInitializer
    {
        Task InitializeAsync();
    }
}
