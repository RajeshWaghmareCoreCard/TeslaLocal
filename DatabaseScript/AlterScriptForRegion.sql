update embossing set binnumber = 0 where binnumber is null;
alter table embossing alter column binnumber set not null;
alter table embossing alter primary key using columns (binnumber, embossingid);

drop index embossing_embossingid_key cascade;
create unique index embossing_embossingid_key on embossing (binnumber, embossingid);
drop index embossing_cardnumber_idx;
create index embossing_cardnumber_idx on embossing (binnumber, cardnumber);


alter table "transaction" add column binnumber Int8 not null default 0;
alter table "transaction" alter primary key using columns (binnumber, tranid);
drop index index_transaction_cardno;
create index index_transaction_cardno on "transaction" (binnumber, cardnumber);
drop index index_transaction_trantype;
create index index_transaction_trantype on "transaction" (binnumber, trantype);

alter table account add column ccregion text not null default '';
alter table account alter primary key using columns (ccregion, accountid);
drop index account_accountnumber_idx;
create index account_accountnumber_idx on account (ccregion, accountnumber);

alter table address add column ccregion text not null default '';
alter table address alter primary key using columns (ccregion, addressid);

alter table apilog add column ccregion text not null default '';
alter table apilog alter primary key using columns (ccregion, logid);

alter table cblog add column ccregion text not null default '';
alter table cblog alter primary key using columns (ccregion, cblogid);
drop index cblog@index_cblog_id;
drop index trans_in_acct@index_cblog_id;
create index index_cblog_id on cblog (ccregion, accountid, tranid,posttime);
drop index index_cblog_amount;
create index index_cblog_amount on cblog (ccregion, tranamount, currentbal);

alter table trans_in_acct add column ccregion text not null default '';
alter table trans_in_acct alter primary key using columns (ccregion, transinacctid);
create index index_trans_in_acct_id on cblog (ccregion, accountid, tranid);

alter table customer add column ccregion text not null default '';
alter table customer alter primary key using columns (ccregion, customerid);

alter table logartxn add column ccregion text not null default '';
alter table logartxn alter primary key using columns (ccregion, logartxnid);
drop index index_logartxn;
create index index_logartxn on logartxn (ccregion, artype, businessdate,"status");

alter table loyaltyplan add column ccregion text not null default '';
alter table loyaltyplan alter primary key using columns (ccregion, loyaltyplanid);
drop index loyaltyplan_accountid_idx;
create index loyaltyplan_accountid_idx on loyaltyplan (ccregion, accountid);


alter table plansegment add column ccregion text not null default '';
alter table plansegment alter primary key using columns (ccregion, planid);
drop index index_plansegment_plantype;
create index index_plansegment_plantype on plansegment (ccregion, plantype);
drop index plansegment_accountid_idx;
create index plansegment_accountid_idx on plansegment (ccregion, accountid);



