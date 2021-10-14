using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreCard.Tesla.Falcon.DataModels.Entity;
namespace CoreCard.Tesla.Falcon.DataModels.Model
{
    public class PaymentAddDTO
    {
        public DateTime trantime { get; set; }
        public string trancode { get; set; }
        public string trantype { get; set; }
        public decimal amount { get; set; }
        public UInt64 accountnumber { get; set; }
        public string ccregion { get; set; }
    }
}
