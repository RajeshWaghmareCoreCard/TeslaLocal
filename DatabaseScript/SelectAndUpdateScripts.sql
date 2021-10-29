SELECT accountid, accountnumber,ifnull(customerid,'00000000-0000-0000-0000-000000000000') as customerid,creditlimit,ifnull(currentbal,0)as currentbal ,ifnull(principal,0)as principal,ifnull(purchaseamount,0)as purchaseamount,ifnull(fees,0)as fees,ifnull(interest,0)as interest,ifnull(purchasecount,0)as purchasecount,ifnull(paymentamount,0)as paymentamount,ifnull(paymentcount,0)as paymentcount, ifnull(status,0)as status, ccregion FROM Account where ccregion='"+ccregion+"' and accountid ='" + guid + "' for update;

SELECT accountid, accountnumber,ifnull(customerid,'00000000-0000-0000-0000-000000000000') as customerid,
 creditlimit,ifnull(currentbal,0)as currentbal ,ifnull(principal,0)as principal,ifnull(purchaseamount,0)as purchaseamount,
 ifnull(fees,0)as fees,ifnull(interest,0)as interest,ifnull(purchasecount,0)as purchasecount,
 ifnull(paymentamount,0)as paymentamount,ifnull(paymentcount,0)as paymentcount, ifnull(status,0)as status, ccregion
 FROM Account where ccregion='"+ccregion+"' and accountnumber =" + AccountNumber.ToString() + " for update;
 
 select embossingid, ifnull(accountid,'00000000-0000-0000-0000-000000000000') as accountid,ifnull(cardtype,0)as cardtype,ifnull(cardnumber,'') as cardnumber , ifnull(binnumber,0) as binnumber, ccregion from embossing where ccregion ='"+ccregion+"' and cardnumber = '" + cardnumber.Trim() + "' for update;
 
 Select * from loyaltyplan where ccregion='" + t.ccregion + "' and  accountid='" + t.accountid + "' FOR UPDATE; update loyaltyplan set rewardbal = " + t.rewardbal + " where ccregion='"+t.ccregion+"' and accountid = '" + t.accountid + "';
 
 SELECT planid, ifnull(accountid,'00000000-0000-0000-0000-000000000000') as accountid
 , ifnull(plantype,0) as plantype, CreationTime,ifnull(currentbal,0)as currentbal ,ifnull(principal,0)as principal,ifnull(purchaseamount,0)as purchaseamount
 , ifnull(fees,0)as fees,ifnull(interest,0)as interest,ifnull(purchasecount,0)as purchasecount
  ,ifnull(paymentamount,0)as paymentamount
  FROM plansegment where ccregion='"+ccregion+"' and  accountid ='" + id.ToString() + "' for update;
  
  
  "update account set currentbal = t.currentbal,  paymentamount = t.paymentamount,  paymentcount = t.paymentcount where ccregion ='"+t.ccregion+"' and accountid = '" +t.accountid.ToString() + "';
  
  "update plansegment set fees = p.fees, interest =  p.interest, principal = p.principal where ccregion='"+p.ccregion+"' and planid = '" + p.planid.ToString() + "';