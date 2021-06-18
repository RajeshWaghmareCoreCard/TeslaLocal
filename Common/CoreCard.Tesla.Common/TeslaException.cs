using System;

namespace CoreCard.Tesla.Common
{
    public class TeslaException : Exception
    {
        public TeslaException(string responseCode, string responseMessage)
        {
            ResponseCode = responseCode;
            ResponseMessage = responseMessage;
        }

        public string ResponseCode { get; }
        public string ResponseMessage { get; }
    }
}