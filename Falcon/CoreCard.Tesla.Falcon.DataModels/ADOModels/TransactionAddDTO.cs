using CoreCard.Tesla.Falcon.DataModels.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreCard.Tesla.Falcon.DataModels.Model
{
    public class TransactionAddDTO
    {
        public string trantype { get; set; }
        public DateTime trantime { get; set; }
        public decimal amount { get; set; }
        public string cardnumber { get; set; }
        public string ccregion { get; set; }

        public static Transaction MapToTransaction(TransactionAddDTO transactionAddDTO)
        {
            Transaction transact = new Transaction();
            transact.trantype = transactionAddDTO.trantype;
            transact.amount = transactionAddDTO.amount;
            transact.cardnumber = transactionAddDTO.cardnumber;
            transact.trantime = transactionAddDTO.trantime;
            transact.ccregion = transactionAddDTO.ccregion;
            return transact;
        }
    }
}
