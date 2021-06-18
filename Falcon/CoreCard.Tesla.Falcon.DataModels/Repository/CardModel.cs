using System;

namespace CoreCard.Tesla.Falcon.DataModels.Repository
{
    public class CardModel
    {
        public string CardId { get; set; }
        public string AccountId { get; set; }
        public string CustomerId { get; set; }
        public string CardTokenId { get; set; }
        public string CardType { get; set; }
        public string CardExpirationDate { get; set; }
        public string CardBin { get; set; }
        public string CardLast4 { get; set; }
        public string ProductId { get; set; }
        public string InstitutionId { get; set; }
        public string ProgramManagerId { get; set; }
        public string CardActivatedDate { get; set; }
        public bool IsActivated { get; set; }
        public decimal DailyLimitAmount { get; set; }
        public int DailyLimitCount { get; set; }
        public string CardStatus { get; set; }
        public decimal DailyUsageAmount { get; set; }
        public DateTime LastUsageDate { get; set; }
    }
}