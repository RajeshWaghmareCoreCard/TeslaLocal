using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreCard.Tesla.Falcon.DataModels.Entity
{
    [Table("plansegment")]
   public  class PlanSegment : BaseEntity
    {
        [Key]
        public Guid planid { get; set; }
        public Guid accountid { get; set; }
        public int plantype { get; set; }
        public DateTime creationtime { get; set; }
        public decimal? currentbal { get; set; }
        public decimal? principal { get; set; }
        public decimal? interest { get; set; }
        public decimal? fees { get; set; }
        public decimal? purchaseamount { get; set; }
        
        public int? purchasecount { get; set; }
        public decimal? paymentamount { get; set; }

        public virtual string? column1 { get; set; }
        public virtual string? column2 { get; set; }
        public virtual string? column3 { get; set; }
        public virtual string? column4 { get; set; }
        public virtual string? column5 { get; set; }
        public virtual string? column6 { get; set; }
        public virtual string? column7 { get; set; }
        public virtual string? column8 { get; set; }
        public virtual string? column9 { get; set; }
        public virtual string? column10 { get; set; }
        public virtual string? column11 { get; set; }
        public virtual string? column12 { get; set; }
        public virtual string? column13 { get; set; }
        public virtual string? column14 { get; set; }
        public virtual string? column15 { get; set; }
        public virtual string? column16 { get; set; }
        public virtual string? column17 { get; set; }
        public virtual string? column18 { get; set; }
        public virtual string? column19 { get; set; }
        public virtual string? column20 { get; set; }
        public virtual string? column21 { get; set; }
        public virtual string? column22 { get; set; }
        public virtual string? column23 { get; set; }
        public virtual string? column24 { get; set; }
        public virtual string? column25 { get; set; }
        public virtual string? column26 { get; set; }
        public virtual string? column27 { get; set; }
        public virtual string? column28 { get; set; }
        public virtual string? column29 { get; set; }
        public virtual string? column30 { get; set; }
        public virtual string? column31 { get; set; }
        public virtual string? column32 { get; set; }
        public virtual string? column33 { get; set; }
        public virtual string? column34 { get; set; }
        public virtual string? column35 { get; set; }
        public virtual string? column36 { get; set; }
        public virtual string? column37 { get; set; }
        public virtual string? column38 { get; set; }
        public virtual string? column39 { get; set; }
        public virtual string? column40 { get; set; }
        public virtual string? column41 { get; set; }
        public virtual string? column42 { get; set; }
        public virtual string? column43 { get; set; }
        public virtual string? column44 { get; set; }
        public virtual string? column45 { get; set; }
        public virtual string? column46 { get; set; }
        public virtual string? column47 { get; set; }
        public virtual string? column48 { get; set; }
        public virtual string? column49 { get; set; }
        public virtual string? column50 { get; set; }

        public static implicit operator PlanSegment(DataRow dr)
        {
            PlanSegment o = new PlanSegment();
            if (dr.IsNull("planid") == false)
            {
                o.planid = dr.Field<Guid>("planid");
            }
            if (dr.IsNull("accountid") == false)
            {
                o.accountid = dr.Field<Guid>("accountid");
            }
            if (dr.IsNull("plantype") == false)
            {
                o.plantype = Convert.ToInt32( dr.Field<long>("plantype"));
            }
            if (dr.IsNull("creationtime") == false)
            {
                o.creationtime = dr.Field<DateTime>("creationtime");
            }
            if (dr.IsNull("currentbal") == false)
            {
                o.currentbal = dr.Field<decimal>("currentbal");
            }
            if (dr.IsNull("principal") == false)
            {
                o.principal = dr.Field<decimal>("principal");
            }
            if (dr.IsNull("interest") == false)
            {
                o.interest = dr.Field<decimal>("interest");
            }
            if (dr.IsNull("fees") == false)
            {
                o.fees = dr.Field<decimal>("fees");
            }
            if (dr.IsNull("purchaseamount") == false)
            {
                o.purchaseamount = dr.Field<decimal>("purchaseamount");
            }
            if (dr.IsNull("purchasecount") == false)
            {
                o.purchasecount = Convert.ToInt32(dr.Field<long>("purchasecount"));
            }
            if (dr.IsNull("paymentamount") == false)
            {
                o.paymentamount = dr.Field<decimal>("paymentamount");
            }
            if (dr.IsNull("column1") == false)
            {
                o.column1 = dr.Field<string>("column1");
            }
            if (dr.IsNull("column2") == false)
            {
                o.column2 = dr.Field<string>("column2");
            }
            if (dr.IsNull("column3") == false)
            {
                o.column3 = dr.Field<string>("column3");
            }
            if (dr.IsNull("column4") == false)
            {
                o.column4 = dr.Field<string>("column4");
            }
            if (dr.IsNull("column5") == false)
            {
                o.column5 = dr.Field<string>("column5");
            }
            if (dr.IsNull("column6") == false)
            {
                o.column6 = dr.Field<string>("column6");
            }
            if (dr.IsNull("column7") == false)
            {
                o.column7 = dr.Field<string>("column7");
            }
            if (dr.IsNull("column8") == false)
            {
                o.column8 = dr.Field<string>("column8");
            }
            if (dr.IsNull("column9") == false)
            {
                o.column9 = dr.Field<string>("column9");
            }
            if (dr.IsNull("column10") == false)
            {
                o.column10 = dr.Field<string>("column10");
            }
            if (dr.IsNull("column11") == false)
            {
                o.column11 = dr.Field<string>("column11");
            }
            if (dr.IsNull("column12") == false)
            {
                o.column12 = dr.Field<string>("column12");
            }
            if (dr.IsNull("column13") == false)
            {
                o.column13 = dr.Field<string>("column13");
            }
            if (dr.IsNull("column14") == false)
            {
                o.column14 = dr.Field<string>("column14");
            }
            if (dr.IsNull("column15") == false)
            {
                o.column15 = dr.Field<string>("column15");
            }
            if (dr.IsNull("column16") == false)
            {
                o.column16 = dr.Field<string>("column16");
            }
            if (dr.IsNull("column17") == false)
            {
                o.column17 = dr.Field<string>("column17");
            }
            if (dr.IsNull("column18") == false)
            {
                o.column18 = dr.Field<string>("column18");
            }
            if (dr.IsNull("column19") == false)
            {
                o.column19 = dr.Field<string>("column19");
            }
            if (dr.IsNull("column20") == false)
            {
                o.column20 = dr.Field<string>("column20");
            }
            if (dr.IsNull("column21") == false)
            {
                o.column21 = dr.Field<string>("column21");
            }
            if (dr.IsNull("column22") == false)
            {
                o.column22 = dr.Field<string>("column22");
            }
            if (dr.IsNull("column23") == false)
            {
                o.column23 = dr.Field<string>("column23");
            }
            if (dr.IsNull("column24") == false)
            {
                o.column24 = dr.Field<string>("column24");
            }
            if (dr.IsNull("column25") == false)
            {
                o.column25 = dr.Field<string>("column25");
            }
            if (dr.IsNull("column26") == false)
            {
                o.column26 = dr.Field<string>("column26");
            }
            if (dr.IsNull("column27") == false)
            {
                o.column27 = dr.Field<string>("column27");
            }
            if (dr.IsNull("column28") == false)
            {
                o.column28 = dr.Field<string>("column28");
            }
            if (dr.IsNull("column29") == false)
            {
                o.column29 = dr.Field<string>("column29");
            }
            if (dr.IsNull("column30") == false)
            {
                o.column30 = dr.Field<string>("column30");
            }
            if (dr.IsNull("column31") == false)
            {
                o.column31 = dr.Field<string>("column31");
            }
            if (dr.IsNull("column32") == false)
            {
                o.column32 = dr.Field<string>("column32");
            }
            if (dr.IsNull("column33") == false)
            {
                o.column33 = dr.Field<string>("column33");
            }
            if (dr.IsNull("column34") == false)
            {
                o.column34 = dr.Field<string>("column34");
            }
            if (dr.IsNull("column35") == false)
            {
                o.column35 = dr.Field<string>("column35");
            }
            if (dr.IsNull("column36") == false)
            {
                o.column36 = dr.Field<string>("column36");
            }
            if (dr.IsNull("column37") == false)
            {
                o.column37 = dr.Field<string>("column37");
            }
            if (dr.IsNull("column38") == false)
            {
                o.column38 = dr.Field<string>("column38");
            }
            if (dr.IsNull("column39") == false)
            {
                o.column39 = dr.Field<string>("column39");
            }
            if (dr.IsNull("column40") == false)
            {
                o.column40 = dr.Field<string>("column40");
            }
            if (dr.IsNull("column41") == false)
            {
                o.column41 = dr.Field<string>("column41");
            }
            if (dr.IsNull("column42") == false)
            {
                o.column42 = dr.Field<string>("column42");
            }
            if (dr.IsNull("column43") == false)
            {
                o.column43 = dr.Field<string>("column43");
            }
            if (dr.IsNull("column44") == false)
            {
                o.column44 = dr.Field<string>("column44");
            }
            if (dr.IsNull("column45") == false)
            {
                o.column45 = dr.Field<string>("column45");
            }
            if (dr.IsNull("column46") == false)
            {
                o.column46 = dr.Field<string>("column46");
            }
            if (dr.IsNull("column47") == false)
            {
                o.column47 = dr.Field<string>("column47");
            }
            if (dr.IsNull("column48") == false)
            {
                o.column48 = dr.Field<string>("column48");
            }
            if (dr.IsNull("column49") == false)
            {
                o.column49 = dr.Field<string>("column49");
            }
            if (dr.IsNull("column50") == false)
            {
                o.column50 = dr.Field<string>("column50");
            }
            
            return o;
        }
    }
}
