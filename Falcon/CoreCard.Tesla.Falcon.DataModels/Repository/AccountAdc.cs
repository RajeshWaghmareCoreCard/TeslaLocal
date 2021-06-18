namespace CoreCard.Tesla.Falcon.DataModels.Repository
{
    public class AccountAdc
    {
        public string AccountAdcId { get; set; }
        public string AccountId { get; set; }
        public string AdcId { get; set; }
        public string ResponseCode { get; set; }
        public string InternalResponseCode { get; set; }
        public bool ContinueOnTimeout { get; set; }
        public bool Active { get; set; }
    }
}