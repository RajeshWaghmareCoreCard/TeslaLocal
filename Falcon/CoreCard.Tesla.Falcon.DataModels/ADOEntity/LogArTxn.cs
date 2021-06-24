using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace CoreCard.Tesla.Falcon.DataModels.Entity
{
    [Table("logartxn")]
    public    class LogArTxn:BaseEntity
    {
        [Key]
        public Guid logartxnid { get; set; }
        public Guid tranid { get; set; }
        public DateTime businessdate { get; set; }
        public int artype { get; set; }

        public string status { get; set; }
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
        
    }
}
