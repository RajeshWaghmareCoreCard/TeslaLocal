using System;

namespace CoreCard.Tesla.Falcon.DataModels.Repository
{
    public class AccountModel
    {
        public string AccountId { get; set; }
        public string AccountNumber { get; set; }
        public decimal CurrentBalance { get; set; }
        public decimal CreditLimit { get; set; }
        public string ProductId { get; set; }
        public string CustomerId { get; set; }
        public string AccountStatus { get; set; }
        public decimal DailyLimitAmount { get; set; }
        public int DailyLimitCount { get; set; }
        public DateTime DailyLimitDate { get; set; }
        public decimal CurrentDailyLimitAmount { get; set; }
        public int CurrentDailyLimitCount { get; set; }
        public decimal CashBalance { get; set; }
        public decimal CashCreditLimit { get; set; }
    }
}