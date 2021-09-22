create INDEX account_accountnumber_idx on account (accountnumber ASC)
create INDEX index_cblog_id on cblog(accountid ASC, tranid ASC, posttime ASC)
create INDEX index_cblog_amount on cblog  (tranamount ASC, currentbal ASC)
create INDEX embossing_cardnumber_idx on embossing (cardnumber ASC)
create INDEX index_logartxn on logartxn (artype ASC, businessdate ASC, status ASC)
create INDEX loyaltyplan_accountid_idx on loyaltyplan (accountid ASC)
create INDEX index_plansegment_plantype on plansegment (plantype ASC)
create INDEX plansegment_accountid_idx on plantsegment (accountid ASC)
create INDEX index_cblog_id on trans_in_acct (accountid ASC, tranid ASC)
create INDEX index_transaction_trantype on transaction (trantype ASC)
create INDEX index_transaction_cardno on transaction (cardnumber ASC)