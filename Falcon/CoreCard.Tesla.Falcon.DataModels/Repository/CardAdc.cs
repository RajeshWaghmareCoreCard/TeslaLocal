namespace CoreCard.Tesla.Falcon.DataModels.Repository
{
    public class CardAdc
    {
        public string CardAdcId { get; set; }
        public string CardId { get; set; }
        public string AdcId { get; set; }
        public string ResponseCode { get; set; }
        public string InternalResponseCode { get; set; }
        public bool ContinueOnTimeout { get; set; }
        public bool Active { get; set; }
    }
}