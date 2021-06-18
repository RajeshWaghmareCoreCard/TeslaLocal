using System;
using System.Security.Cryptography;
using System.Xml;
using CoreCard.Tesla.Common;

namespace Tesla.TokenizationProvider
{
    public static class RSAExtensions
    {
        public static RSA CreateRSAFromPrivateKey(this string privateKey)
        {
            try
            {
                var rsa = RSA.Create();
                rsa.ImportFromPem(privateKey);
                return rsa;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }


}