use PaymentDb;
/*
CRDD Team you are free to alter the scripts as per the Region requirement.
*/

/*
Verify region names before running
*/

ALTER DATABASE paymentdb PRIMARY REGION "us-east1";
ALTER DATABASE paymentdb ADD REGION "us-east2";
ALTER DATABASE paymentdb ADD REGION "us-west2";
ALTER DATABASE  cdb_demo SURVIVE REGION FAILURE;

alter table embossing add column ccregion crdb_internal_region not null ;
alter table embossing alter primary key using columns (ccregion, embossingid);

drop index embossing_embossingid_key cascade;
create unique index embossing_embossingid_key on embossing (ccregion, embossingid);
drop index embossing_cardnumber_idx;
create index embossing_cardnumber_idx on embossing (ccregion, cardnumber);


alter table "transaction" add column ccregion crdb_internal_region not null ;
alter table "transaction" alter primary key using columns (ccregion, tranid);
drop index index_transaction_cardno;
create index index_transaction_cardno on "transaction" (ccregion, cardnumber);
drop index index_transaction_trantype;
create index index_transaction_trantype on "transaction" (ccregion, trantype);

alter table account add column ccregion crdb_internal_region not null ;
alter table account alter primary key using columns (ccregion, accountid);
drop index account_accountnumber_idx;
create index account_accountnumber_idx on account (ccregion, accountnumber);

alter table address add column ccregion crdb_internal_region not null ;
alter table address alter primary key using columns (ccregion, addressid);

/*
alter table apilog add column ccregion crdb_internal_region not null ;
alter table apilog alter primary key using columns (ccregion, logid);
*/
alter table cblog add column ccregion crdb_internal_region not null ;
alter table cblog alter primary key using columns (ccregion, cblogid);
drop index cblog@index_cblog_id;
drop index trans_in_acct@index_cblog_id;
create index index_cblog_id on cblog (ccregion, accountid, tranid,posttime);
drop index index_cblog_amount;
create index index_cblog_amount on cblog (ccregion, tranamount, currentbal);

alter table trans_in_acct add column ccregion crdb_internal_region not null ;
alter table trans_in_acct alter primary key using columns (ccregion, transinacctid);
create index index_trans_in_acct_id on cblog (ccregion, accountid, tranid);

alter table customer add column ccregion crdb_internal_region not null ;
alter table customer alter primary key using columns (ccregion, customerid);

alter table logartxn add column ccregion crdb_internal_region not null ;
alter table logartxn alter primary key using columns (ccregion, logartxnid);
drop index index_logartxn;
create index index_logartxn on logartxn (ccregion, artype, businessdate,"status");

alter table loyaltyplan add column ccregion crdb_internal_region not null ;
alter table loyaltyplan alter primary key using columns (ccregion, loyaltyplanid);
drop index loyaltyplan_accountid_idx;
create index loyaltyplan_accountid_idx on loyaltyplan (ccregion, accountid);


alter table plansegment add column ccregion crdb_internal_region not null ;
alter table plansegment alter primary key using columns (ccregion, planid);
drop index index_plansegment_plantype;
create index index_plansegment_plantype on plansegment (ccregion, plantype);
drop index plansegment_accountid_idx;
create index plansegment_accountid_idx on plansegment (ccregion, accountid);



ALTER TABLE Embossing SET LOCALITY REGIONAL BY ROW AS CCREGION;
ALTER TABLE account SET LOCALITY REGIONAL BY ROW AS CCREGION;
ALTER TABLE address SET LOCALITY REGIONAL BY ROW AS CCREGION;
/*
 Use default hidden column for apilog.
*/
ALTER TABLE apilog SET LOCALITY REGIONAL BY ROW ;
ALTER TABLE cblog SET LOCALITY REGIONAL BY ROW AS CCREGION;
ALTER TABLE customer SET LOCALITY REGIONAL BY ROW AS CCREGION;

ALTER TABLE logartxn SET LOCALITY REGIONAL BY ROW AS CCREGION;
ALTER TABLE loyaltyplan SET LOCALITY REGIONAL BY ROW AS CCREGION;
ALTER TABLE plansegment SET LOCALITY REGIONAL BY ROW AS CCREGION;
ALTER TABLE trans_in_acct SET LOCALITY REGIONAL BY ROW AS CCREGION;


