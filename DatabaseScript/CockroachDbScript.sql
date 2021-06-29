DROP DATABASE IF EXISTS PaymentDb CASCADE;

CREATE DATABASE IF NOT EXISTS PaymentDb;

GRANT ALL ON DATABASE PaymentDb TO "suraj.kovoor";

DROP TABLE IF EXISTS CUSTOMER CASCADE;

CREATE TABLE IF NOT EXISTS CUSTOMER(
CUSTOMERID UUID PRIMARY KEY DEFAULT gen_random_uuid(),
SSN STRING,
FIRSTNAME STRING,
LASTNAME STRING,
column1	STRING,
column2	STRING,
column3	STRING,
column4	STRING,
column5	STRING,
column6	STRING,
column7	STRING,
column8	STRING,
column9	STRING,
column10	STRING,
column11	STRING,
column12	STRING,
column13	STRING,
column14	STRING,
column15	STRING,
column16	STRING,
column17	STRING,
column18	STRING,
column19	STRING,
column20	STRING,
column21	STRING,
column22	STRING,
column23	STRING,
column24	STRING,
column25	STRING,
column26	STRING,
column27	STRING,
column28	STRING,
column29	STRING,
column30	STRING,
column31	STRING,
column32	STRING,
column33	STRING,
column34	STRING,
column35	STRING,
column36	STRING,
column37	STRING,
column38	STRING,
column39	STRING,
column40	STRING,
column41	STRING,
column42	STRING,
column43	STRING,
column44	STRING,
column45	STRING,
column46	STRING,
column47	STRING,
column48	STRING,
column49	STRING,
column50	STRING
);


DROP TABLE IF EXISTS ADDRESS CASCADE;

CREATE TABLE IF NOT EXISTS ADDRESS(
ADDRESSID UUID PRIMARY KEY DEFAULT gen_random_uuid(),
CUSTOMERID UUID,
ADDRESSTYPE INT8,
HOUSENUMBER STRING,
STREET STRING,
CITY STRING,
STATE STRING,
ZIPCODE STRING,
column1	STRING,
column2	STRING,
column3	STRING,
column4	STRING,
column5	STRING,
column6	STRING,
column7	STRING,
column8	STRING,
column9	STRING,
column10	STRING,
column11	STRING,
column12	STRING,
column13	STRING,
column14	STRING,
column15	STRING,
column16	STRING,
column17	STRING,
column18	STRING,
column19	STRING,
column20	STRING,
column21	STRING,
column22	STRING,
column23	STRING,
column24	STRING,
column25	STRING
);


DROP TABLE IF EXISTS ACCOUNT CASCADE;

CREATE TABLE IF NOT EXISTS ACCOUNT(
ACCOUNTID UUID PRIMARY KEY DEFAULT gen_random_uuid(),
CUSTOMERID UUID,
ACCOUNTNUMBER INT8,
CREDITLIMIT DECIMAL,
CURRENTBAL DECIMAL,
PRINCIPAL DECIMAL,
INTEREST DECIMAL,
FEES DECIMAL,
PURCHASEAMOUNT DECIMAL,
PURCHASECOUNT INT8,
PAYMENTAMOUNT DECIMAL,
PAYMENTCOUNT INT8,
STATUS INT8,
column1	STRING,
column2	STRING,
column3	STRING,
column4	STRING,
column5	STRING,
column6	STRING,
column7	STRING,
column8	STRING,
column9	STRING,
column10	STRING,
column11	STRING,
column12	STRING,
column13	STRING,
column14	STRING,
column15	STRING,
column16	STRING,
column17	STRING,
column18	STRING,
column19	STRING,
column20	STRING,
column21	STRING,
column22	STRING,
column23	STRING,
column24	STRING,
column25	STRING,
column26	STRING,
column27	STRING,
column28	STRING,
column29	STRING,
column30	STRING,
column31	STRING,
column32	STRING,
column33	STRING,
column34	STRING,
column35	STRING,
column36	STRING,
column37	STRING,
column38	STRING,
column39	STRING,
column40	STRING,
column41	STRING,
column42	STRING,
column43	STRING,
column44	STRING,
column45	STRING,
column46	STRING,
column47	STRING,
column48	STRING,
column49	STRING,
column50	STRING,
column51	STRING,
column52	STRING,
column53	STRING,
column54	STRING,
column55	STRING,
column56	STRING,
column57	STRING,
column58	STRING,
column59	STRING,
column60	STRING,
column61	STRING,
column62	STRING,
column63	STRING,
column64	STRING,
column65	STRING,
column66	STRING,
column67	STRING,
column68	STRING,
column69	STRING,
column70	STRING,
column71	STRING,
column72	STRING,
column73	STRING,
column74	STRING,
column75	STRING,
column76	STRING,
column77	STRING,
column78	STRING,
column79	STRING,
column80	STRING,
column81	STRING,
column82	STRING,
column83	STRING,
column84	STRING,
column85	STRING,
column86	STRING,
column87	STRING,
column88	STRING,
column89	STRING,
column90	STRING,
column91	STRING,
column92	STRING,
column93	STRING,
column94	STRING,
column95	STRING,
column96	STRING,
column97	STRING,
column98	STRING,
column99	STRING,
column100	STRING
);


DROP TABLE IF EXISTS EMBOSSING CASCADE;

CREATE TABLE IF NOT EXISTS EMBOSSING(
EMBOSSINGID UUID PRIMARY KEY DEFAULT gen_random_uuid(),
ACCOUNTID UUID,
CARDNUMBER STRING,
CARDTYPE INT8,
column1	STRING,
column2	STRING,
column3	STRING,
column4	STRING,
column5	STRING,
column6	STRING,
column7	STRING,
column8	STRING,
column9	STRING,
column10	STRING,
column11	STRING,
column12	STRING,
column13	STRING,
column14	STRING,
column15	STRING,
column16	STRING,
column17	STRING,
column18	STRING,
column19	STRING,
column20	STRING,
column21	STRING,
column22	STRING,
column23	STRING,
column24	STRING,
column25	STRING,
column26	STRING,
column27	STRING,
column28	STRING,
column29	STRING,
column30	STRING,
column31	STRING,
column32	STRING,
column33	STRING,
column34	STRING,
column35	STRING,
column36	STRING,
column37	STRING,
column38	STRING,
column39	STRING,
column40	STRING,
column41	STRING,
column42	STRING,
column43	STRING,
column44	STRING,
column45	STRING,
column46	STRING,
column47	STRING,
column48	STRING,
column49	STRING,
column50	STRING
);

DROP TABLE IF EXISTS LOYALTYPLAN CASCADE;

CREATE TABLE IF NOT EXISTS LOYALTYPLAN(
LOYALTYPLANID UUID PRIMARY KEY DEFAULT gen_random_uuid(),
ACCOUNTID UUID,
LOYALTYPLANTYPE INT8,
REWARDBAL DECIMAL,
column1	STRING,
column2	STRING,
column3	STRING,
column4	STRING,
column5	STRING,
column6	STRING,
column7	STRING,
column8	STRING,
column9	STRING,
column10	STRING,
column11	STRING,
column12	STRING,
column13	STRING,
column14	STRING,
column15	STRING,
column16	STRING,
column17	STRING,
column18	STRING,
column19	STRING,
column20	STRING,
column21	STRING,
column22	STRING,
column23	STRING,
column24	STRING,
column25	STRING
);

DROP TABLE IF EXISTS APILOG CASCADE;

CREATE TABLE IF NOT EXISTS APILOG(
LOGID UUID PRIMARY KEY DEFAULT gen_random_uuid(),
APINAME STRING,
LOGTIME TIMESTAMPTZ,
RESPONSE INT8,
column1	STRING,
column2	STRING,
column3	STRING,
column4	STRING,
column5	STRING
);


drop table if exists plansegment;
CREATE TABLE IF NOT EXISTS plansegment(
planid UUID PRIMARY KEY DEFAULT gen_random_uuid(),
accountid uuid,
PlanType   INT8, 
CreationTime timestamp default CURRENT_TIMESTAMP, 
CurrentBal DECIMAL, 
Principal DECIMAL, 
Interest DECIMAL, 
Fees DECIMAL, 
PurchaseAmount DECIMAL, 
PurchaseCount INT8, 
PaymentAmount DECIMAL,
column1	STRING,
column2	STRING,
column3	STRING,
column4	STRING,
column5	STRING,
column6	STRING,
column7	STRING,
column8	STRING,
column9	STRING,
column10	STRING,
column11	STRING,
column12	STRING,
column13	STRING,
column14	STRING,
column15	STRING,
column16	STRING,
column17	STRING,
column18	STRING,
column19	STRING,
column20	STRING,
column21	STRING,
column22	STRING,
column23	STRING,
column24	STRING,
column25	STRING,
column26	STRING,
column27	STRING,
column28	STRING,
column29	STRING,
column30	STRING,
column31	STRING,
column32	STRING,
column33	STRING,
column34	STRING,
column35	STRING,
column36	STRING,
column37	STRING,
column38	STRING,
column39	STRING,
column40	STRING,
column41	STRING,
column42	STRING,
column43	STRING,
column44	STRING,
column45	STRING,
column46	STRING,
column47	STRING,
column48	STRING,
column49	STRING,
column50	STRING,
INDEX index_plansegment_plantype (PlanType)
);

drop table if exists Transaction;
CREATE TABLE IF NOT EXISTS Transaction(
Tranid UUID PRIMARY KEY DEFAULT gen_random_uuid(),
accountid uuid,
TranType   string, 
Trancode INT8,
TranTime timestamp default CURRENT_TIMESTAMP, 
Amount DECIMAL, 
CardNumber string, 
column1	STRING,
column2	STRING,
column3	STRING,
column4	STRING,
column5	STRING,
column6	STRING,
column7	STRING,
column8	STRING,
column9	STRING,
column10	STRING,
column11	STRING,
column12	STRING,
column13	STRING,
column14	STRING,
column15	STRING,
column16	STRING,
column17	STRING,
column18	STRING,
column19	STRING,
column20	STRING,
column21	STRING,
column22	STRING,
column23	STRING,
column24	STRING,
column25	STRING,
column26	STRING,
column27	STRING,
column28	STRING,
column29	STRING,
column30	STRING,
column31	STRING,
column32	STRING,
column33	STRING,
column34	STRING,
column35	STRING,
column36	STRING,
column37	STRING,
column38	STRING,
column39	STRING,
column40	STRING,
column41	STRING,
column42	STRING,
column43	STRING,
column44	STRING,
column45	STRING,
column46	STRING,
column47	STRING,
column48	STRING,
column49	STRING,
column50	STRING,
INDEX index_Transaction_Trantype (TranType),
INDEX index_Transaction_CardNo (CardNumber)
);

drop table if exists LogArTxn;
CREATE TABLE IF NOT EXISTS LogArTxn(
LogArTxnId UUID PRIMARY KEY DEFAULT gen_random_uuid(),
TranId uuid,
ARType INT8,
Businessdate   date default current_date, 
status string, 
column1	STRING,
column2	STRING,
column3	STRING,
column4	STRING,
column5	STRING,
column6	STRING,
column7	STRING,
column8	STRING,
column9	STRING,
column10	STRING,
INDEX index_LogArTxn (ARType,Businessdate,status)
);

drop table if exists CBLog;
CREATE TABLE IF NOT EXISTS CBLog(
CBLogId UUID PRIMARY KEY DEFAULT gen_random_uuid(),
AccountId uuid,
TranId uuid,
TranAmount decimal,
CurrentBal   decimal, 
PostTime timestamp default current_timestamp, 
INDEX index_CBLog_ID (AccountId,TranId,PostTime),
INDEX index_CBLog_Amount (TranAmount,CurrentBal)
);

drop table if exists Trans_in_acct;
CREATE TABLE IF NOT EXISTS Trans_in_acct(
TransinacctId UUID PRIMARY KEY DEFAULT gen_random_uuid(),
AccountId uuid,
TranId uuid,
column1	STRING,
column2	STRING,
column3	STRING,
INDEX index_CBLog_ID (AccountId,TranId)
);

GRANT ALL ON TABLE paymentdb.* TO "suraj.kovoor";