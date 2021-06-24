using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreCard.Tesla.Falcon.DataModels.Entity
{
    [Table("loyaltyplan")]

    public class LoyaltyPlan : BaseEntity
    {
        //        Primary Key – LoyaltyPlanId – long int
        //Join Key – AccountId(Account) – long int
        //Field Names – LoyaltyPlanType - int, RewardBal-currency, col1 to col25(varchar)
        [Key]
        public Guid loyaltyplanid { get; set; }

        public Guid accountid { get; set; }

        public int loyaltyplantype { get; set; }

        public decimal rewardbal { get; set; }

        public virtual string column1 { get; set; }
        public virtual string column2 { get; set; }
        public virtual string column3 { get; set; }
        public virtual string column4 { get; set; }
        public virtual string column5 { get; set; }
        public virtual string column6 { get; set; }
        public virtual string column7 { get; set; }
        public virtual string column8 { get; set; }
        public virtual string column9 { get; set; }
        public virtual string column10 { get; set; }
        public virtual string column11 { get; set; }
        public virtual string column12 { get; set; }
        public virtual string column13 { get; set; }
        public virtual string column14 { get; set; }
        public virtual string column15 { get; set; }
        public virtual string column16 { get; set; }
        public virtual string column17 { get; set; }
        public virtual string column18 { get; set; }
        public virtual string column19 { get; set; }
        public virtual string column20 { get; set; }
        public virtual string column21 { get; set; }
        public virtual string column22 { get; set; }
        public virtual string column23 { get; set; }
        public virtual string column24 { get; set; }
        public virtual string column25 { get; set; }
        

    }
}
