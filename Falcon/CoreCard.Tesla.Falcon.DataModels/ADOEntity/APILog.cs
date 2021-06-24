using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoreCard.Tesla.Falcon.DataModels.Entity
{
    [Table("apilog")]
    public class APILog:BaseEntity
    {
        [Key]
        public Guid logid { get; set; }

        public string apiname { get; set; }

        public int response { get; set; }

        public DateTime logtime { get; set; }

        public virtual string column1 { get; set; }
        public virtual string column2 { get; set; }
        public virtual string column3 { get; set; }
        public virtual string column4 { get; set; }
        public virtual string column5 { get; set; }

    }
}
