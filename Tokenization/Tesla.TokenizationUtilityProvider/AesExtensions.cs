using System.IO;
using System.Security.Cryptography;
using System.Text;
using CoreCard.Tesla.Common;

namespace Tesla.TokenizationProvider
{
    public static class AesExtensions
    {
        public static string Encrypt(this Aes aesSession, string data)
        {
            // Create an encryptor to perform the stream transform.
            ICryptoTransform encryptor = aesSession.CreateEncryptor(aesSession.Key, aesSession.IV);

            // Create the streams used for encryption.AesExtensions
            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        //Write all data to the stream.
                        swEncrypt.Write(data);

                    }
                    return msEncrypt.ToArray().TryToBase64String();
                }
            }
        }
    
        public static string Decrypt(this Aes aesSession, string data)
        {
            // Create a decryptor to perform the stream transform.
            ICryptoTransform decryptor = aesSession.CreateDecryptor(aesSession.Key, aesSession.IV);

            // Create the streams used for decryption.
            using (MemoryStream msDecrypt = new MemoryStream(data.TryFromBase64String()))
            {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {
                        // Read the decrypted bytes from the decrypting stream
                        // and place them in a string.
                        return srDecrypt.ReadToEnd();
                    }
                }
            }
        }
    }
}