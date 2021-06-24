using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace CoreCard.Tesla.Falcon.DataModels.Entity
{
    [Table("cblog")]
    public class CBLog : BaseEntity
    {
        [Key]
        public Guid cblogid { get; set; }
        public Guid accountid { get; set; }
        public Guid tranid { get; set; }
        public decimal tranamount { get; set; }
        public decimal currentbal { get; set; }
        public DateTime posttime { get; set; }
        

    }
}
