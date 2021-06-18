using System;

namespace CoreCard.Tesla.NetworkInterface.DataModels
{
    public class PurchaseRequest
    {
        public string CardNumber { get; set; }     
        public string ProcessingCode { get; set; }
    }
}
