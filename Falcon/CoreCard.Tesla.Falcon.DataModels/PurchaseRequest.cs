using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using CoreCard.Tesla.Falcon.DataModels.Common;
using CoreCard.Tesla.Falcon.DataModels.Repository;

namespace CoreCard.Tesla.Falcon.DataModels
{
    public class PurchaseRequest
    {
        public PurchaseRequest()
        {
        }
        public IsoRequest RequestDataElements { get; set; }
        public string TransactionType { get; set; }
        public string InternalTransactionType { get; set; }
    }
    public class PurchaseResponse
    {
        public PurchaseResponse()
        {
        }
        public IsoResponse ResponseDataElements { get; set; }
        public string ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
    }

    public class PurchaseRequestExt
    {
        public PurchaseRequestExt()
        {
            TransactionDetails = new TransactionModel();
        }
       // public PurchaseRequest Inner { get; set; }
        public TransactionModel TransactionDetails { get; set; }
    }

    public class PurchaseResponseExt
    {
        public PurchaseResponse Response { get; set; }
    }
}
