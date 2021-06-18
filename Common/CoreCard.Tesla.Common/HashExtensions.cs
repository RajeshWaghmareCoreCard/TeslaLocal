using System.Security.Cryptography;
using System.Text;

namespace CoreCard.Tesla.Common
{
    public static class HashExtensions
    {
        public static string HMACSHA512Hash(this string data, string salt)
        {
            string hash = string.Empty;
            byte[] tokenBytes = Encoding.UTF8.GetBytes(data);
            byte[] saltBytes = Encoding.UTF8.GetBytes(salt);
            using (HMACSHA512 hMACSHA512 = new HMACSHA512(saltBytes))
            {
                var hashBytes = hMACSHA512.ComputeHash(tokenBytes);
                hash = hashBytes.TryToBase64String();
            }
            return hash;
        }
    }
}