using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreCard.Tesla.Falcon.DataModels.Model
{
    public class PlanSegmentAddDTO
    {
        public int PlanType { get; set; }
        public DateTime CreationTime { get; set; }
        public decimal CurrentBal { get; set; }
        public decimal Principal { get; set; }
        public decimal Interest { get; set; }
        public decimal Fees { get; set; }
        public decimal PurchaseAmount { get; set; }
        public int PurchaseCount { get; set; }
        public decimal PaymentAmount { get; set; }
    }
}
