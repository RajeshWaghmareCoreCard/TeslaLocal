namespace CoreCard.Tesla.Falcon.DataModels.Common
{
    public class AuthDecisionControl
    {
        public string ADCId { get; set; }
        public int Priority { get; set; }
        public string ControlId { get; set; }
        public string Category { get; set; }
        public string RuleDescription { get; set; }
        public string InternalResponseCode { get; set; }
        public bool ContinueOnTimeout { get; set; }
        public string ResponseCode { get; set; }
        public bool IsActive { get; set; } 
        public string CategoryType { get; set; }
        public string InstitutionId { get; set; } 
        public string ProductId { get; set; }
        public string  ADCName { get; set; }
        public AdcType Type { get; set; }
    }
}