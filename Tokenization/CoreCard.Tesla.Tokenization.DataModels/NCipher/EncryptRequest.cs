namespace CoreCard.Tesla.Tokenization.DataModels.NCipher
{
    public class EncryptRequest
    {
        public string AesKey { get; set; }

        public string Data { get; set; }
        public int DataLength { get; set; }
    }

    public class EncryptResponse
    {
        public string AesKey { get; set; }

        public string EncryptedData { get; set; }

        public int Length { get; set; }

        public string ResponseCode { get; set; }
        public string ResponseMessage { get; set; }

        
    }
}