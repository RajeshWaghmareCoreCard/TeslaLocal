using CoreCard.Tesla.Falcon.DataModels.Entity;
using CoreCard.Tesla.Falcon.DataModels.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreCard.Tesla.Falcon.NpgRepository.Interface
{
    public interface IPurchaseUnit
    {
        //Account Account { get; set; }
        //Transaction Transaction { get; set; }
        //PlanSegment PlanSegment { get; set; }
        //CBLog CBLog { get; set; }
        //LogArTxn LogArTxn { get; set; }
        //Trans_in_Acct trans_In_Acct { get; set; }
       // PaymentAddDTO paymentaddDTO { get; set; }
        Task<Transaction> MakePayment(PaymentAddDTO paymentAddDTO);
    }
}
