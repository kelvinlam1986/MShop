using System.Security.Cryptography;

namespace MShop.Infrastructure.Security
{
    public class Encrypter : IEncrypter
    {
        public string GetHash(string value, string salt)
        {
            var derivedByte = new Rfc2898DeriveBytes(value, GetBytes(salt), 1000);
            return Convert.ToBase64String(derivedByte.GetBytes(50));
        }

        public string GetSalt()
        {
            var salt = "939KYc_1v8qi5v?j#_+6D39#Ydrz3Wx(F4O-xFj74CpttM/CvT";
            return salt;
            //var saltBytes = new Byte[50];
            //var range = RandomNumberGenerator.Create();
            //range.GetBytes(saltBytes);

            //return Convert.ToBase64String(saltBytes);
        }

        private static byte[] GetBytes(string value)
        {
            var bytes = new Byte[value.Length];
            Buffer.BlockCopy(value.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }
    }
}
