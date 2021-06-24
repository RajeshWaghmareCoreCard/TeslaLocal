using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace CoreCard.Tesla.Falcon.DataModels.Entity
{
    [Table("trans_in_acct")]
    public class Trans_in_Acct : BaseEntity
    {
        [Key]
        public Guid transinacctid { get; set; }
        public Guid accountid { get; set; }
        public Guid tranid { get; set; }
        public virtual string column1 { get; set; }
        public virtual string column2 { get; set; }
        public virtual string column3 { get; set; }

    }
}
