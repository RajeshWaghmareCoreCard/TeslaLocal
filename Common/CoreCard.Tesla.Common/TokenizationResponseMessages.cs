namespace CoreCard.Tesla.Common
{
    public class TokenizationResponseMessages
    {
        //NetworkInterfaceSeries - ModuleId 01
        //Falcon - ModuleId 02
        //Tokenization - ModuleId 03
        //Notification - ModuleId 04
        //Statementing - ModuleId 05
        public static string Success = "Success";


        //Falcon - ModuleId 02
        public static string FalconDbInsert = "02001";


        //Tokenization - ModuleId 03
        public static string DatabaseConnection = "Database connection is broken";
        public static string SessionNotFound = "Module Session Not Found";
        public static string TokenNotFound = "Card Token Not Found";
        public static string AesGenerationFailed = "Aes Generation failed";
        public static string SystemMalfunction = "Tokenization Malfunction";
        public static string InvalidModule = "Invalid Module";
        public static string ModulePermissionIssue = "Invalid Module Permission";

        

    }
}