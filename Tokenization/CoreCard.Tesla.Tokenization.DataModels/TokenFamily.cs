namespace CoreCard.Tesla.Tokenization.DataModels
{
    public enum TokenFormat
    {
        Default,
        Last4Only,
        First6AndLast4Only
    }
    public class Token
    {
        public string Family { get; set; }
        public string Value { get; set; }
        public TokenFormat Format { get; set; }
    }
}