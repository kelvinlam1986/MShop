namespace MShop.Infrastructure.Authentication
{
    public interface IAuthenticationHandler
    {
        JwtAuthToken Create(string userId);
    }
}
