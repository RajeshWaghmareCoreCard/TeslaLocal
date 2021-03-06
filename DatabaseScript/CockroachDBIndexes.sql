use PaymentDb;
CREATE INDEX IF NOT EXISTS account_accountnumber_idx on account (accountnumber ASC);
CREATE INDEX IF NOT EXISTS index_cblog_id on cblog(accountid ASC, tranid ASC, posttime ASC);
CREATE INDEX IF NOT EXISTS index_trans_in_acct_id on trans_in_acct (accountid ASC, tranid ASC);
CREATE INDEX IF NOT EXISTS index_cblog_amount on cblog  (tranamount ASC, currentbal ASC);
CREATE INDEX IF NOT EXISTS embossing_cardnumber_idx on embossing (cardnumber ASC);
CREATE INDEX IF NOT EXISTS index_logartxn on logartxn (artype ASC, businessdate ASC, status ASC);
CREATE INDEX IF NOT EXISTS loyaltyplan_accountid_idx on loyaltyplan (accountid ASC);
CREATE INDEX IF NOT EXISTS index_plansegment_plantype on plansegment (plantype ASC);
CREATE INDEX IF NOT EXISTS plansegment_accountid_idx on plansegment (accountid ASC);
CREATE INDEX IF NOT EXISTS index_transaction_trantype on transaction (trantype ASC);
CREATE INDEX IF NOT EXISTS index_transaction_cardno on transaction (cardnumber ASC);
