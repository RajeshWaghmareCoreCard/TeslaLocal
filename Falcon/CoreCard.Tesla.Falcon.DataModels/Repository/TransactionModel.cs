namespace CoreCard.Tesla.Falcon.DataModels.Repository
{
    public class TransactionModel
    {
        public long TransactionId { get; set; }

        public string CardToken { get; set; }

        public string ExpirationDate { get; set; }

        public Merchant MerchantDetails { get; set; }

        public long TransactionAmount { get; set; }
    }

    public class Merchant
    {
        public string Name { get; set; }
        public string Address { get; set; }
    }
}