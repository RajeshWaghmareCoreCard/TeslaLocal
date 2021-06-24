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
    [Table("account")]

    public class Account : BaseEntity
    {
        [Key]
        public Guid accountid { get; set; }

        public Guid customerid { get; set; }

        public Int64 accountnumber { get; set; }

        public decimal creditlimit { get; set; }

        public decimal currentbal { get; set; }

        public decimal principal { get; set; }

        public decimal interest { get; set; }
        public decimal fees { get; set; }

        public decimal purchaseamount { get; set; }
        public int purchasecount { get; set; }
        public decimal paymentamount { get; set; }

        public int paymentcount { get; set; }

        public int status { get; set; }

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
        public virtual string column51 { get; set; }
        public virtual string column52 { get; set; }
        public virtual string column53 { get; set; }
        public virtual string column54 { get; set; }
        public virtual string column55 { get; set; }
        public virtual string column56 { get; set; }
        public virtual string column57 { get; set; }
        public virtual string column58 { get; set; }
        public virtual string column59 { get; set; }
        public virtual string column60 { get; set; }
        public virtual string column61 { get; set; }
        public virtual string column62 { get; set; }
        public virtual string column63 { get; set; }
        public virtual string column64 { get; set; }
        public virtual string column65 { get; set; }
        public virtual string column66 { get; set; }
        public virtual string column67 { get; set; }
        public virtual string column68 { get; set; }
        public virtual string column69 { get; set; }
        public virtual string column70 { get; set; }
        public virtual string column71 { get; set; }
        public virtual string column72 { get; set; }
        public virtual string column73 { get; set; }
        public virtual string column74 { get; set; }
        public virtual string column75 { get; set; }
        public virtual string column76 { get; set; }
        public virtual string column77 { get; set; }
        public virtual string column78 { get; set; }
        public virtual string column79 { get; set; }

        public virtual string column80 { get; set; }
        public virtual string column81 { get; set; }
        public virtual string column82 { get; set; }
        public virtual string column83 { get; set; }
        public virtual string column84 { get; set; }
        public virtual string column85 { get; set; }
        public virtual string column86 { get; set; }
        public virtual string column87 { get; set; }
        public virtual string column88 { get; set; }
        public virtual string column89 { get; set; }
        public virtual string column90 { get; set; }
        public virtual string column91 { get; set; }
        public virtual string column92 { get; set; }
        public virtual string column93 { get; set; }
        public virtual string column94 { get; set; }
        public virtual string column95 { get; set; }
        public virtual string column96 { get; set; }
        public virtual string column97 { get; set; }
        public virtual string column98 { get; set; }
        public virtual string column99 { get; set; }
        public virtual string column100 { get; set; }

        public static implicit operator Account(DataRow dr)
        {
            Account o = new Account();
            if (dr.IsNull("accountid") == false)
            {
                o.accountid = dr.Field<Guid>("accountid");
            }
            if (dr.IsNull("customerid") == false)
            {
                o.customerid = dr.Field<Guid>("customerid");
            }
            if (dr.IsNull("accountnumber") == false)
            {
                o.accountnumber = Convert.ToInt64( dr.Field<Int64>("accountnumber"));
            }
            if (dr.IsNull("creditlimit") == false)
            {
                o.creditlimit = dr.Field<decimal>("creditlimit");
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
                o.purchasecount = Convert.ToInt32( dr.Field<Int64>("purchasecount"));
            }
            if (dr.IsNull("paymentamount") == false)
            {
                o.paymentamount = dr.Field<decimal>("paymentamount");
            }
            if (dr.IsNull("paymentcount") == false)
            {
                o.paymentcount = Convert.ToInt32(dr.Field<Int64>("paymentcount"));
            }
            if (dr.IsNull("status") == false)
            {
                o.status = Convert.ToInt32(dr.Field<decimal>("status"));
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
            if (dr.IsNull("column51") == false)
            {
                o.column51 = dr.Field<string>("column51");
            }
            if (dr.IsNull("column52") == false)
            {
                o.column52 = dr.Field<string>("column52");
            }
            if (dr.IsNull("column53") == false)
            {
                o.column53 = dr.Field<string>("column53");
            }
            if (dr.IsNull("column54") == false)
            {
                o.column54 = dr.Field<string>("column54");
            }
            if (dr.IsNull("column55") == false)
            {
                o.column55 = dr.Field<string>("column55");
            }
            if (dr.IsNull("column56") == false)
            {
                o.column56 = dr.Field<string>("column56");
            }
            if (dr.IsNull("column57") == false)
            {
                o.column57 = dr.Field<string>("column57");
            }
            if (dr.IsNull("column58") == false)
            {
                o.column58 = dr.Field<string>("column58");
            }
            if (dr.IsNull("column59") == false)
            {
                o.column59 = dr.Field<string>("column59");
            }
            if (dr.IsNull("column60") == false)
            {
                o.column60 = dr.Field<string>("column60");
            }
            if (dr.IsNull("column61") == false)
            {
                o.column61 = dr.Field<string>("column61");
            }
            if (dr.IsNull("column62") == false)
            {
                o.column62 = dr.Field<string>("column62");
            }
            if (dr.IsNull("column63") == false)
            {
                o.column63 = dr.Field<string>("column63");
            }
            if (dr.IsNull("column64") == false)
            {
                o.column64 = dr.Field<string>("column64");
            }
            if (dr.IsNull("column65") == false)
            {
                o.column65 = dr.Field<string>("column65");
            }
            if (dr.IsNull("column66") == false)
            {
                o.column66 = dr.Field<string>("column66");
            }
            if (dr.IsNull("column67") == false)
            {
                o.column67 = dr.Field<string>("column67");
            }
            if (dr.IsNull("column68") == false)
            {
                o.column68 = dr.Field<string>("column68");
            }
            if (dr.IsNull("column69") == false)
            {
                o.column69 = dr.Field<string>("column69");
            }
            if (dr.IsNull("column70") == false)
            {
                o.column70 = dr.Field<string>("column70");
            }
            if (dr.IsNull("column71") == false)
            {
                o.column71 = dr.Field<string>("column71");
            }
            if (dr.IsNull("column72") == false)
            {
                o.column72 = dr.Field<string>("column72");
            }
            if (dr.IsNull("column73") == false)
            {
                o.column73 = dr.Field<string>("column73");
            }
            if (dr.IsNull("column74") == false)
            {
                o.column74 = dr.Field<string>("column74");
            }
            if (dr.IsNull("column75") == false)
            {
                o.column75 = dr.Field<string>("column75");
            }
            if (dr.IsNull("column76") == false)
            {
                o.column76 = dr.Field<string>("column76");
            }
            if (dr.IsNull("column77") == false)
            {
                o.column77 = dr.Field<string>("column77");
            }
            if (dr.IsNull("column78") == false)
            {
                o.column78 = dr.Field<string>("column78");
            }
            if (dr.IsNull("column79") == false)
            {
                o.column79 = dr.Field<string>("column79");
            }
            if (dr.IsNull("column80") == false)
            {
                o.column80 = dr.Field<string>("column80");
            }
            if (dr.IsNull("column81") == false)
            {
                o.column81 = dr.Field<string>("column81");
            }
            if (dr.IsNull("column82") == false)
            {
                o.column82 = dr.Field<string>("column82");
            }
            if (dr.IsNull("column83") == false)
            {
                o.column83 = dr.Field<string>("column83");
            }
            if (dr.IsNull("column84") == false)
            {
                o.column84 = dr.Field<string>("column84");
            }
            if (dr.IsNull("column85") == false)
            {
                o.column85 = dr.Field<string>("column85");
            }
            if (dr.IsNull("column86") == false)
            {
                o.column86 = dr.Field<string>("column86");
            }
            if (dr.IsNull("column87") == false)
            {
                o.column87 = dr.Field<string>("column87");
            }
            if (dr.IsNull("column88") == false)
            {
                o.column88 = dr.Field<string>("column88");
            }
            if (dr.IsNull("column89") == false)
            {
                o.column89 = dr.Field<string>("column89");
            }
            if (dr.IsNull("column90") == false)
            {
                o.column90 = dr.Field<string>("column90");
            }
            if (dr.IsNull("column91") == false)
            {
                o.column91 = dr.Field<string>("column91");
            }
            if (dr.IsNull("column92") == false)
            {
                o.column92 = dr.Field<string>("column92");
            }
            if (dr.IsNull("column93") == false)
            {
                o.column93 = dr.Field<string>("column93");
            }
            if (dr.IsNull("column94") == false)
            {
                o.column94 = dr.Field<string>("column94");
            }
            if (dr.IsNull("column95") == false)
            {
                o.column95 = dr.Field<string>("column95");
            }
            if (dr.IsNull("column96") == false)
            {
                o.column96 = dr.Field<string>("column96");
            }
            if (dr.IsNull("column97") == false)
            {
                o.column97 = dr.Field<string>("column97");
            }
            if (dr.IsNull("column98") == false)
            {
                o.column98 = dr.Field<string>("column98");
            }
            if (dr.IsNull("column99") == false)
            {
                o.column99 = dr.Field<string>("column99");
            }
            if (dr.IsNull("column100") == false)
            {
                o.column100 = dr.Field<string>("column100");
            }
            return o;
        }

    }
}
