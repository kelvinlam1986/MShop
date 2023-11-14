using MShop.Infrastructure.Command.User;
using MShop.Infrastructure.Event.User;
using MShop.Infrastructure.Security;

namespace MShop.User.DataProvider.Extensions
{
    public static class Extension
    {
        public static CreateUser SetPassword(this CreateUser user, IEncrypter encrypter)
        {
            var salt = encrypter.GetSalt();
            user.Password = encrypter.GetHash(user.Password, salt);
            return user;
        }

        public static bool ValidatePassword(this UserCreated user, UserCreated savedUser, IEncrypter encrypter)
        {
            var salt = encrypter.GetSalt();
            return savedUser.Password.Equals(encrypter.GetHash(user.Password, salt));
        }

        public static bool ValidatePassword(this UserCreated user, string password, IEncrypter encrypter)
        {
            var salt = encrypter.GetSalt();
            return user.Password.Equals(encrypter.GetHash(password, salt));
        }
    }
}
