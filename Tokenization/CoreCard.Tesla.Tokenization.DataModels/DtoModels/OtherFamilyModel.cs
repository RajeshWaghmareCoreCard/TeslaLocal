namespace CoreCard.Tesla.Tokenization.DataModels.DtoModels
{
    public class OtherFamilyModel
    {
        public string OtherTokenId { get; set; }
        public string InstitutionId { get; set; }
        public string TokenHash { get; set; }
        public string EncryptedData { get; set; }
        public bool Active { get; set; } = true;
        public string TokenFamilyId { get; set; }
        public int HsmActiveKeyId { get; set; }
    }
}