
namespace CoreCard.Tesla.Tokenization.DataModels.DtoTypes
{
    public class CardTokenModel
    {
        public string CardTokenId { get; set; }
        public string CardBin { get; set; }

        public string CardLast4 { get; set; }

        public string CardHash { get; set; }

        public string CardExpiration { get; set; } = "";
        public string InstitutionId { get; set; }
        //public string TokenIdx { get; set; }
        public int HsmActiveKeyId { get; set; }
        public string EncryptedCardNumber { get; set; }
        public string NetworkName { get; set; }
        public string Active { get; set; }
    }
}