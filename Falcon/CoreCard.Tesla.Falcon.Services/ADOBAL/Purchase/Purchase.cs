using CoreCard.Tesla.Falcon.DataModels.Entity;
using CoreCard.Tesla.Falcon.DataModels.Model;
//using CockroachDb.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreCard.Tesla.Utilities;
using System.Data.SqlClient;
using CoreCard.Tesla.Falcon.ADORepository;
using DBAdapter;
using CoreCard.Tesla.Utilities;
using CoreCard.Tesla.Falcon.Services.Purchase;

namespace CoreCard.Tesla.Falcon.Services
{
    public class PurchaseBAL :  IPurchaseBAL
    {
        //private readonly ITransactionRepository _transactionRepository;
        //private readonly IEmbossingRepository _embosingRepository;
        //private readonly ILoyaltyPlanRepository _loyaltyPlanRepository;
        private readonly IPlanSegmentBAL _PlansegmentBAL;
        private readonly IAccountBAL _accountBAL;
        private readonly ILoyaltyPlanBAL _loyaltyPlanBAL;
        private readonly IAPILogBAL _aPILogBAL;
        private readonly TimeLogger _timeLogger;
        private readonly ICBLogBAL _cBLogBAL;
        private readonly ITransInAcctBAL _transInAcctBal;
        private readonly ILogArTxnBAL _logArTxnBAL;
        private readonly ITransactionBAL _transactionBAL;
        private readonly IEmbossingBAL _embossingBAL;
        private readonly IBaseCockroachADO _baseCockroachADO;
        private Tuple<DBAdapter.IDataBaseCommand, object> transactionTuple;

        private readonly object lockObject = new object();

        public PurchaseBAL(TimeLogger timeLogger, /*ITransactionRepository transactionRepository, IEmbossingRepository embosingRepository, ILoyaltyPlanRepository loyaltyPlanRepository,*/
            IPlanSegmentBAL plansegmentBAL, IAccountBAL accountBAL, ICBLogBAL cBLogBAL, ITransInAcctBAL transInAcctBal, ILogArTxnBAL logArTxnBAL, IEmbossingBAL embossingBAL, IBaseCockroachADO baseCockroachADO,
            ILoyaltyPlanBAL loyaltyPlanBAL, IAPILogBAL aPILogBAL, ITransactionBAL transactionBAL)// : base(transactionRepository)
        {
            //_transactionRepository = transactionRepository;
            //_embosingRepository = embosingRepository;
            //_loyaltyPlanRepository = loyaltyPlanRepository;
            _PlansegmentBAL = plansegmentBAL;
            _loyaltyPlanBAL = loyaltyPlanBAL;
            _aPILogBAL = aPILogBAL;
            _timeLogger = timeLogger;
            _accountBAL = accountBAL;
            _cBLogBAL = cBLogBAL;
            _transInAcctBal = transInAcctBal;
            _logArTxnBAL = logArTxnBAL;
            _transactionBAL = transactionBAL;
            _embossingBAL = embossingBAL;
            _baseCockroachADO = baseCockroachADO;
        }

        public async Task<BaseResponseDTO> AddTransactionAsync(TransactionAddDTO transactionAddDTO)
        {
            transactionTuple = await _baseCockroachADO.BeginTransactionAsync();
            BaseResponseDTO baseResponseDTO = new BaseResponseDTO();
            int responseType = 0;
            Account updatedAccount = new Account();
            try
            {
                _timeLogger.Start("AddPurchase");
                //using (TransactionScope transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                //{
                Transaction intransact = TransactionAddDTO.MapToTransaction(transactionAddDTO);
                Embossing embossing = _embossingBAL.GetEmbossingByCardNumber(transactionAddDTO.cardnumber, transactionTuple.Item1);
                if (embossing != null)
                {
                    updatedAccount = _accountBAL.GetAccountByID_ADO(embossing.accountid, transactionTuple.Item1);
                }
                if (updatedAccount != null)
                {
                    if (updatedAccount.currentbal + transactionAddDTO.amount <= updatedAccount.creditlimit)
                    {
                        updatedAccount.currentbal = updatedAccount.currentbal + transactionAddDTO.amount;
                        updatedAccount.principal = updatedAccount.principal + transactionAddDTO.amount;
                        updatedAccount.purchaseamount = updatedAccount.purchaseamount + transactionAddDTO.amount;
                        updatedAccount = _accountBAL.UpdatePurchase(updatedAccount, transactionTuple.Item1);

                        PlanSegment planSegment = new PlanSegment();
                        planSegment.plantype = 1;// "Insatallment";
                        planSegment.creationtime = DateTime.UtcNow;
                        planSegment.purchaseamount = intransact.amount;
                        planSegment.principal = intransact.amount;
                        planSegment.accountid = updatedAccount.accountid;
                        planSegment.fees = 3;
                        planSegment.interest = intransact.amount * Convert.ToDecimal(0.15);
                        planSegment.purchasecount = 1;
                        _PlansegmentBAL.Insert(planSegment, transactionTuple.Item1);

                        //LoyaltyPlan loyaltyplan = _loyaltyPlanRepository.GetEntity("Select * from loyaltyplan where accountid = '" + account.accountid + "'");
                        LoyaltyPlan loyaltyplan = new LoyaltyPlan();
                        loyaltyplan.accountid = updatedAccount.accountid;
                        loyaltyplan.rewardbal = loyaltyplan.rewardbal + 2;
                        _loyaltyPlanBAL.UpdatePurchase(loyaltyplan, transactionTuple.Item1);

                        intransact.trancode = 100;
                        intransact.trantype = "40";
                        intransact.accountid = updatedAccount.accountid;
                        //Entity.Transaction newTransact = _transactionBAL.AddTransactionADO(intransact); //await _transactionRepository.AddAsync(intransact);
                        Guid newTranId = _transactionBAL.AddTransactionADO(intransact, transactionTuple.Item1);

                        LogArTxn newLogARTxn = new LogArTxn();
                        newLogARTxn.businessdate = DateTime.UtcNow;
                        newLogARTxn.artype = 1;
                        newLogARTxn.tranid = newTranId;
                        newLogARTxn.status = "Success";
                        //await _logArTxnBAL.AddAsync(newLogARTxn);
                        _logArTxnBAL.Insert(newLogARTxn, transactionTuple.Item1);

                        CBLog cbLog = new CBLog();
                        cbLog.accountid = updatedAccount.accountid;
                        cbLog.currentbal = updatedAccount.currentbal;
                        cbLog.tranid = newTranId;
                        cbLog.tranamount = transactionAddDTO.amount;
                        cbLog.posttime = DateTime.UtcNow;
                        // await _cBLogBAL.AddAsync(cbLog);
                        _cBLogBAL.Insert(cbLog, transactionTuple.Item1);

                        Trans_in_Acct trans_In_Acct = new Trans_in_Acct();
                        trans_In_Acct.accountid = updatedAccount.accountid;
                        trans_In_Acct.tranid = newTranId;
                        //await _transInAcctBal.AddAsync(trans_In_Acct);
                        _transInAcctBal.Insert(trans_In_Acct, transactionTuple.Item1);


                        await _baseCockroachADO.CommitTransactionAsync(transactionTuple.Item1, transactionTuple.Item2);
                        baseResponseDTO.DataLayerTime = _timeLogger.StopAndLog("AddPurchase");
                        baseResponseDTO.BaseEntityInstance = updatedAccount;
                        return baseResponseDTO;
                    }
                    else
                    {
                        await _baseCockroachADO.RollbackTransactionAsync(transactionTuple.Item1, transactionTuple.Item2);
                        baseResponseDTO.DataLayerTime = _timeLogger.StopAndLog("AddPurchase");
                        baseResponseDTO.BaseEntityInstance = "Error{'Message':'Purchase Denied.Purchase amount + Current Balance greater than credit limit.'}";
                        return baseResponseDTO;
                    }
                }
                else
                {
                    await _baseCockroachADO.RollbackTransactionAsync(transactionTuple.Item1, transactionTuple.Item2);
                    baseResponseDTO.DataLayerTime = _timeLogger.StopAndLog("AddPurchase");
                    baseResponseDTO.BaseEntityInstance = "Error{'Message':'Account does not exist.'}";
                    return baseResponseDTO;
                }
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "AddTransactionAsync");
                //_transactionRepository.RejectChanges();
                await _baseCockroachADO.RollbackTransactionAsync(transactionTuple.Item1, transactionTuple.Item2);
                responseType = -1;
                throw;
            }
            finally
            {
                // APILog
                APILog aPILog = new APILog();
                //aPILog.logid = Guid.NewGuid();
                aPILog.apiname = "Purchase";
                aPILog.logtime = DateTime.UtcNow;
                aPILog.response = responseType;
                _aPILogBAL.Insert(aPILog);
                //_transactionRepository.Save();
            }

            //}


        }

        //public async Task<BaseResponseDTO> AddTransactionAsync(TransactionAddDTO transactionAddDTO)
        //{
        //    BaseResponseDTO baseResponseDTO = new BaseResponseDTO();
        //    int responseType = 0;
        //    Account updatedAccount = new Account();
        //    try
        //    {
        //        _timeLogger.Start("AddPurchase");
        //        //using (TransactionScope transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        //        //{
        //        Entity.Transaction intransact = TransactionAddDTO.MapToTransaction(transactionAddDTO);

        //        Embossing embossing = _embosingRepository.GetEntity("Select * from embossing where CardNumber = '" + transactionAddDTO.cardnumber + "'");
        //        Account account = _accountBAL.Get(embossing.accountid);
        //        if (account.currentbal + transactionAddDTO.amount <= account.creditlimit)
        //        {
        //            account.currentbal = account.currentbal + transactionAddDTO.amount;
        //            account.principal = account.principal + transactionAddDTO.amount;
        //            account.purchaseamount = account.purchaseamount + transactionAddDTO.amount;
        //            updatedAccount = _accountBAL.Update(account, account.accountid);

        //            PlanSegment planSegment = new PlanSegment();
        //            planSegment.plantype = 1;// "Insatallment";
        //            planSegment.creationtime = DateTime.UtcNow;
        //            planSegment.purchaseamount = intransact.amount;
        //            planSegment.principal = intransact.amount;
        //            planSegment.fees = 3;
        //            planSegment.interest = intransact.amount * Convert.ToDecimal(0.15);
        //            planSegment.purchasecount = 1;
        //            PlanSegment newPlansegment = await _PlansegmentBAL.AddAsync(planSegment);


        //            LoyaltyPlan loyaltyplan = _loyaltyPlanRepository.GetEntity("Select * from loyaltyplan where accountid = '" + account.accountid + "'");
        //            loyaltyplan.rewardbal = loyaltyplan.rewardbal + 2;
        //            await _loyaltyPlanBAL.UpdateAsync(loyaltyplan, loyaltyplan.loyaltyplanid);

        //            intransact.trancode = 100;
        //            intransact.trantype = "40";
        //            intransact.accountid = account.accountid;
        //            Entity.Transaction newTransact = await _transactionRepository.AddAsync(intransact);

        //            LogArTxn newLogARTxn = new LogArTxn();
        //            newLogARTxn.businessdate = DateTime.UtcNow;
        //            newLogARTxn.artype = 1;
        //            newLogARTxn.tranid = newTransact.tranid;
        //            newLogARTxn.status = "Success";
        //            await _logArTxnBAL.AddAsync(newLogARTxn);

        //            CBLog cbLog = new CBLog();
        //            cbLog.accountid = account.accountid;
        //            cbLog.currentbal = account.currentbal;
        //            cbLog.tranid = newTransact.tranid;
        //            cbLog.tranamount = newTransact.amount;
        //            cbLog.posttime = DateTime.UtcNow;
        //            await _cBLogBAL.AddAsync(cbLog);

        //            Trans_in_Acct trans_In_Acct = new Trans_in_Acct();
        //            trans_In_Acct.accountid = account.accountid;
        //            trans_In_Acct.tranid = newTransact.tranid;
        //            await _transInAcctBal.AddAsync(trans_In_Acct);

        //            _transactionRepository.Save();

        //            //_accountBAL.Save();
        //            //_PlansegmentBAL.Save();
        //            //_loyaltyPlanBAL.Save();
        //            //_transactionRepository.Save();
        //            //_logArTxnBAL.Save();
        //            //_cBLogBAL.Save();

        //            //    transaction.Complete();

        //            //}
        //        }

        //        baseResponseDTO.DataLayerTime = _timeLogger.StopAndLog("AddPurchase");
        //        baseResponseDTO.BaseEntityInstance = updatedAccount;
        //        return baseResponseDTO;

        //    }
        //    catch (Exception ex)
        //    {
        //        _transactionRepository.RejectChanges();
        //        responseType = -1;
        //        throw;
        //    }
        //    finally
        //    {
        //        // APILog
        //        APILog aPILog = new APILog();
        //        //aPILog.logid = Guid.NewGuid();
        //        aPILog.apiname = "Purchase";
        //        aPILog.logtime = DateTime.UtcNow;
        //        aPILog.response = responseType;
        //        _aPILogBAL.Add(aPILog);
        //        _transactionRepository.Save();
        //    }


        //}
    }
}
