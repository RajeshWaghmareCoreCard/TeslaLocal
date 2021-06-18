namespace CoreCard.Tesla.Common
{
    public class ResponseMessages
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
        public static string TokenizationDbInsert = "Failed to insert to database";
        public static string TokenizationSystemMalfunction = "Tokenization Malfunction";

    }
}