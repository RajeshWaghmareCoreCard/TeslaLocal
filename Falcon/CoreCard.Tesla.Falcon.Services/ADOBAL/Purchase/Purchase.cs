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
using Npgsql;
using Microsoft.Extensions.Logging;

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
        private readonly ILogger<PurchaseBAL> _logger;

        private readonly object lockObject = new object();

        public PurchaseBAL(TimeLogger timeLogger, /*ITransactionRepository transactionRepository, IEmbossingRepository embosingRepository, ILoyaltyPlanRepository loyaltyPlanRepository,*/
            IPlanSegmentBAL plansegmentBAL, IAccountBAL accountBAL, ICBLogBAL cBLogBAL, ITransInAcctBAL transInAcctBal, ILogArTxnBAL logArTxnBAL, IEmbossingBAL embossingBAL, IBaseCockroachADO baseCockroachADO,
            ILoyaltyPlanBAL loyaltyPlanBAL, IAPILogBAL aPILogBAL, ITransactionBAL transactionBAL, ILogger<PurchaseBAL> logger)// : base(transactionRepository)
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
            _logger = logger;
        }

        public async Task<BaseResponseDTO> AddTransactionAsync(TransactionAddDTO transactionAddDTO)
        {
            _timeLogger.Start("AddPurchase");
            _timeLogger.Start("BeginTransactionAsync");
            transactionTuple = await _baseCockroachADO.BeginTransactionAsync();
            _timeLogger.StopAndLog("BeginTransactionAsync");
            //string ccregion = "UNKNOWN-PURCHASE-REGION";
            BaseResponseDTO baseResponseDTO = new BaseResponseDTO();
            int responseType = 0;
            int retry = 0;
            int sqlexp = 0;
            int trancommit = 0;
            Account updatedAccount = new Account();
            try
            {

                while (true)
                {
                    Guid guid = Guid.NewGuid();
                    string savePointName = "purchase_restart" + guid.ToString();

                    try
                    {

                        //using (TransactionScope transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                        //{
                        _timeLogger.Start("SavePointAsync");
                        await _baseCockroachADO.SavePointAsync(transactionTuple.Item1, transactionTuple.Item2, savePointName);
                        _timeLogger.StopAndLog("SavePointAsync");
                        Transaction intransact = TransactionAddDTO.MapToTransaction(transactionAddDTO);
                        _timeLogger.Start("GetEmbossingByCardNumber");
                        Embossing embossing = _embossingBAL.GetEmbossingByCardNumber(transactionAddDTO.cardnumber, transactionAddDTO.ccregion, transactionTuple.Item1);
                        _timeLogger.StopAndLog("GetEmbossingByCardNumber");
                        if (embossing != null)
                        {
                            _timeLogger.Start("GetAccountByID_ADO");

                            updatedAccount = _accountBAL.GetAccountByID_ADO(embossing.accountid, embossing.ccregion, transactionTuple.Item1);
                            _timeLogger.StopAndLog("GetAccountByID_ADO");
                        }
                        else
                        {
                            _timeLogger.Start("CommitTransactionAsync");
                            await _baseCockroachADO.CommitTransactionAsync(transactionTuple.Item1, transactionTuple.Item2);
                            _timeLogger.StopAndLog("CommitTransactionAsync");
                            //baseResponseDTO.DataLayerTime = _timeLogger.StopAndLog("AddPurchase");
                            baseResponseDTO.BaseEntityInstance = "Error{'Message':'Card does not exist.'}";
                            return baseResponseDTO;
                        }
                        if (updatedAccount != null)
                        {
                            //if (updatedAccount.currentbal + transactionAddDTO.amount <= updatedAccount.creditlimit)
                            //{
                            updatedAccount.currentbal = updatedAccount.currentbal + transactionAddDTO.amount;
                            updatedAccount.principal = updatedAccount.principal + transactionAddDTO.amount;
                            updatedAccount.purchaseamount = updatedAccount.purchaseamount + transactionAddDTO.amount;
                            updatedAccount.ccregion = embossing.ccregion;
                            _timeLogger.Start("UpdatePurchase");
                            updatedAccount = _accountBAL.UpdatePurchase(updatedAccount, transactionTuple.Item1);
                            _timeLogger.StopAndLog("UpdatePurchase");

                            PlanSegment planSegment = new PlanSegment();
                            planSegment.plantype = 1;// "Insatallment";
                            planSegment.creationtime = DateTime.UtcNow;
                            planSegment.purchaseamount = intransact.amount;
                            planSegment.principal = intransact.amount;
                            planSegment.accountid = updatedAccount.accountid;
                            planSegment.fees = 3;
                            planSegment.interest = intransact.amount * Convert.ToDecimal(0.15);
                            planSegment.purchasecount = 1;
                            planSegment.accountid = updatedAccount.accountid;
                            planSegment.ccregion = embossing.ccregion;
                            _timeLogger.Start("PlansegmentInsert");
                            _PlansegmentBAL.Insert(planSegment, transactionTuple.Item1);
                            _timeLogger.StopAndLog("PlansegmentInsert");

                            //LoyaltyPlan loyaltyplan = _loyaltyPlanRepository.GetEntity("Select * from loyaltyplan where accountid = '" + account.accountid + "'");
                            LoyaltyPlan loyaltyplan = new LoyaltyPlan();
                            loyaltyplan.accountid = updatedAccount.accountid;
                            loyaltyplan.rewardbal = loyaltyplan.rewardbal + 2;
                            loyaltyplan.ccregion = embossing.ccregion;
                            _timeLogger.Start("LoyaltyUpdatePurchase");
                            _loyaltyPlanBAL.UpdatePurchase(loyaltyplan, transactionTuple.Item1);
                            _timeLogger.StopAndLog("LoyaltyUpdatePurchase");

                            intransact.trancode = 100;
                            intransact.trantype = "40";
                            intransact.accountid = updatedAccount.accountid;
                            intransact.ccregion = embossing.ccregion;
                            //Entity.Transaction newTransact = _transactionBAL.AddTransactionADO(intransact); //await _transactionRepository.AddAsync(intransact);
                            _timeLogger.Start("PurchaseAddTransactionADO");
                            Guid newTranId = _transactionBAL.AddTransactionADO(intransact, transactionTuple.Item1);
                            _timeLogger.StopAndLog("PurchaseAddTransactionADO");

                            LogArTxn newLogARTxn = new LogArTxn();
                            newLogARTxn.businessdate = DateTime.UtcNow;
                            newLogARTxn.artype = 1;
                            newLogARTxn.tranid = newTranId;
                            newLogARTxn.status = "Success";
                            newLogARTxn.ccregion = embossing.ccregion;
                            //await _logArTxnBAL.AddAsync(newLogARTxn);
                            _timeLogger.Start("PurchaseLogArInsert");
                            _logArTxnBAL.Insert(newLogARTxn, transactionTuple.Item1);
                            _timeLogger.StopAndLog("PurchaseLogArInsert");

                            CBLog cbLog = new CBLog();
                            cbLog.accountid = updatedAccount.accountid;
                            cbLog.currentbal = updatedAccount.currentbal;
                            cbLog.tranid = newTranId;
                            cbLog.tranamount = transactionAddDTO.amount;
                            cbLog.posttime = DateTime.UtcNow;
                            cbLog.ccregion = embossing.ccregion;
                            // await _cBLogBAL.AddAsync(cbLog);
                            _timeLogger.Start("PurchaseCBLogInsert");
                            _cBLogBAL.Insert(cbLog, transactionTuple.Item1);
                            _timeLogger.StopAndLog("PurchaseCBLogInsert");

                            Trans_in_Acct trans_In_Acct = new Trans_in_Acct();
                            trans_In_Acct.accountid = updatedAccount.accountid;
                            trans_In_Acct.tranid = newTranId;
                            trans_In_Acct.ccregion = embossing.ccregion;
                            //await _transInAcctBal.AddAsync(trans_In_Acct);
                            _timeLogger.Start("PurchaseTranAcctInsert");
                            _transInAcctBal.Insert(trans_In_Acct, transactionTuple.Item1);
                            _timeLogger.StopAndLog("PurchaseTranAcctInsert");

                            _timeLogger.Start("PurchaseCommitTransactionAsync");
                            await _baseCockroachADO.CommitTransactionAsync(transactionTuple.Item1, transactionTuple.Item2);
                            _timeLogger.StopAndLog("PurchaseCommitTransactionAsync");
                            baseResponseDTO.DataLayerTime = _timeLogger.StopAndLog("AddPurchase");
                            baseResponseDTO.BaseEntityInstance = updatedAccount;
                            return baseResponseDTO;
                            //}
                            //else
                            //{
                            //    await _baseCockroachADO.RollbackTransactionAsync(transactionTuple.Item1, transactionTuple.Item2);
                            //    baseResponseDTO.DataLayerTime = _timeLogger.StopAndLog("AddPurchase");
                            //    baseResponseDTO.BaseEntityInstance = "Error{'Message':'Purchase Denied.Purchase amount + Current Balance greater than credit limit.'}";
                            //    return baseResponseDTO;
                            //}
                        }
                        else
                        {
                            await _baseCockroachADO.RollbackTransactionAsync(transactionTuple.Item1, transactionTuple.Item2);
                            baseResponseDTO.DataLayerTime = _timeLogger.StopAndLog("AddPurchase");
                            baseResponseDTO.BaseEntityInstance = "Error{'Message':'Account does not exist.'}";
                            return baseResponseDTO;
                        }
                    }
                    catch (TimeoutException t)
                    {
                        retry++;
                        if (retry == 5)
                            throw;
                        try
                        {
                            await _baseCockroachADO.RollbackTransactionAsync(transactionTuple.Item1, transactionTuple.Item2, savePointName);
                        }
                        catch (Exception tx)
                        {
                            if (tx.Message.Contains("This NpgsqlTransaction has completed"))
                            {
                                baseResponseDTO.DataLayerTime = _timeLogger.StopAndLog("AddPurchase");
                                baseResponseDTO.BaseEntityInstance = updatedAccount;
                                return baseResponseDTO;
                            }
                            else
                            {
                                _logger.LogError(tx, "AddPurchaseAsync");
                            }
                        }
                        //_logger.LogError(e, "AddTransactionAsync ErrorCode:" + e.SqlState);
                    }
                    catch (NpgsqlException e)
                    {
                        // Check if the error code indicates a SERIALIZATION_FAILURE.
                        if (e.SqlState == "40001")
                        {
                            // Signal the database that we will attempt a retry.
                            //Thread.Sleep(1000);
                            try
                            {
                                await _baseCockroachADO.RollbackTransactionAsync(transactionTuple.Item1, transactionTuple.Item2, savePointName);
                            }
                            catch (Exception tx)
                            {
                                if (tx.Message.Contains("This NpgsqlTransaction has completed"))
                                {
                                    baseResponseDTO.DataLayerTime = _timeLogger.StopAndLog("AddPurchase");
                                    baseResponseDTO.BaseEntityInstance = updatedAccount;
                                    return baseResponseDTO;
                                }
                                else
                                {
                                    _logger.LogError(tx, "AddPurchaseAsync");
                                }
                            }
                        }
                        else
                        {
                            _logger.LogError(e, "AddPurchaseAsync");
                            throw;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "AddPurchaseAsync");
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
                //aPILog.ccregion = embossing.ccregion;
                _aPILogBAL.Insert(aPILog);
                //_transactionRepository.Save();
            }

            //}
            //BaseResponseDTO baseResponseDTO = new BaseResponseDTO();
            //try
            //{
            //    transactionTuple = await _baseCockroachADO.BeginTransactionAsync();
            //    _logger.LogInformation("Transaction Opened;");
            //    await _baseCockroachADO.CommitTransactionAsync(transactionTuple.Item1, transactionTuple.Item2);
            //    _logger.LogInformation("Transaction Committed;");
            //    baseResponseDTO.BaseEntityInstance = "Response{'Message':'Transaction executed successfully'}";
            //}
            //catch (Exception e)
            //{
            //    await _baseCockroachADO.RollbackTransactionAsync(transactionTuple.Item1, transactionTuple.Item2);
            //    baseResponseDTO.BaseEntityInstance = "Response{'Message':'Transaction Rolledback successfully'}";
            //    _logger.LogInformation("Transaction RolledBack;");
            //}
            //return baseResponseDTO;
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
