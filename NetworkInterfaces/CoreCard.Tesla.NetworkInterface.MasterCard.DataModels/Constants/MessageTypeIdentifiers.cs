namespace CoreCard.Tesla.NetworkInterface.MasterCard.DataModels.Constants
{
    public static class MessageTypeIdentifiers
    {
        public const string AdministrativeAdviceRequest = "0600";
        public const string AdministrativeAdviceResponse = "0630";
        public const string AuthorizationRequest = "0100";
        public const string AuthorizationResponse = "0110";
        public const string FinancialRequest = "0200";
        public const string FinancialResponse = "0210";
        public const string FinancialTransactionAdvice = "0220";
        public const string FinancialTransactionAdviceResponse = "0230";
        public const string NetworkManagementRequest = "0800";
        public const string NetworkManagementResponse = "0810";

    }
}