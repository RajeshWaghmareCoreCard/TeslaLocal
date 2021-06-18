namespace CoreCard.Tesla.Falcon.DataModels.Repository
{
    public class ProductAdc
    {
        public string ProductAdcId { get; set; }
        public string ProductId { get; set; }
        public string AdcId { get; set; }
        public string ResponseCode { get; set; }
        public string InternalResponseCode { get; set; }
        public bool ContinueOnTimeout { get; set; }
        public bool Active { get; set; }
    }
}