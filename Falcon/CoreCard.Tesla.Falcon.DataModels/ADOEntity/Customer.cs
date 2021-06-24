using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoreCard.Tesla.Falcon.DataModels.Entity
{
    [Table("customer")]
    public class Customer :BaseEntity
    {
    

        [Key]
        public Guid customerid { get; set; }
        public string ssn { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }

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
        public virtual string column26 { get; set; }
        public virtual string column27 { get; set; }
        public virtual string column28 { get; set; }
        public virtual string column29 { get; set; }
        public virtual string column30 { get; set; }
        public virtual string column31 { get; set; }
        public virtual string column32 { get; set; }
        public virtual string column33 { get; set; }
        public virtual string column34 { get; set; }
        public virtual string column35 { get; set; }
        public virtual string column36 { get; set; }
        public virtual string column37 { get; set; }
        public virtual string column38 { get; set; }
        public virtual string column39 { get; set; }
        public virtual string column40 { get; set; }
        public virtual string column41 { get; set; }
        public virtual string column42 { get; set; }
        public virtual string column43 { get; set; }
        public virtual string column44 { get; set; }
        public virtual string column45 { get; set; }
        public virtual string column46 { get; set; }
        public virtual string column47 { get; set; }
        public virtual string column48 { get; set; }
        public virtual string column49 { get; set; }
        public virtual string column50 { get; set; }
    }
}
