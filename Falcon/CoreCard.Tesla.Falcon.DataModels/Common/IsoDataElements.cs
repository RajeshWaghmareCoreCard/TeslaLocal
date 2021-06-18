namespace CoreCard.Tesla.Falcon.DataModels.Common
{
    public class IsoDataElements
    {
        //002
        public  string CardToken { get; set; }
        //003
        public  string ProcessingCode { get; set; }
        //004   
        public string ExpirationDate { get; set; }
        //
        public long TransactionAmount { get; set; }
    }
}