namespace CoreCard.Tesla.Tokenization.DataModels.NCipher
{
    public class DecryptRequest
    {
        public string EncryptedData { get; set; }

        public string AesKey { get; set; }

        public string DataLength { get; set; }
    }

     public class DecryptResponse
    {
        public string EncryptedData { get; set; }

        public string AesKey { get; set; }

        public string DataLength { get; set; }
    }
}