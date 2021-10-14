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
using Npgsql;
using Microsoft.Extensions.Logging;

namespace CoreCard.Tesla.Falcon.Services
{
    public class PaymentBAL : /*BaseBAL<Transaction>, */IPaymentBAL
    {

        //private readonly ITransactionRepository _transactionRepository;
        //private readonly IADOTransactionRepository _adotransactionRepository;
        private readonly IAccountBAL _accountBAL;
        private readonly IPlanSegmentBAL _planSegmentBAL;
        private readonly ILogArTxnBAL _logArTxnBAL;
        private readonly ICBLogBAL _cblogBAL;
        private readonly ITransInAcctBAL _traninacct;
        private readonly TimeLogger _timeLogger;
        private readonly IAPILogBAL _apilogBAL;
        private readonly ITransactionBAL _transactionBAL;
        private int responseTy = 0;
        private Tuple<DBAdapter.IDataBaseCommand, object> newTuple;
        private readonly object lockObject = new object();
        private readonly IBaseCockroachADO _baseCockroachADO;
        private readonly ILogger<PaymentBAL> _logger;
        public PaymentBAL(TimeLogger timeLogger
           , IAccountBAL accountBAL, IPlanSegmentBAL planSegmentBAL, ILogArTxnBAL logArTxnBAL
            , ICBLogBAL cblogBAL, ITransInAcctBAL traninacct, IAPILogBAL apilogBAL, ILogger<PaymentBAL> logger) //: base(transactionRepository)
        {
            _timeLogger = timeLogger;
            //_transactionRepository = transactionRepository;
            _accountBAL = accountBAL;
            _planSegmentBAL = planSegmentBAL;
            _logArTxnBAL = logArTxnBAL;
            _cblogBAL = cblogBAL;
            _traninacct = traninacct;
            _apilogBAL = apilogBAL;
            _logger = logger;
        }
        public PaymentBAL(TimeLogger timeLogger
           , IBaseCockroachADO baseCockroachADO
           , IAccountBAL accountBAL, IPlanSegmentBAL planSegmentBAL, ILogArTxnBAL logArTxnBAL
            , ICBLogBAL cblogBAL, ITransInAcctBAL traninacct, IAPILogBAL apilogBAL, ITransactionBAL transactionBAL, ILogger<PaymentBAL> logger)// : base(transactionRepository)
        {
            _timeLogger = timeLogger;
            _accountBAL = accountBAL;
            // _transactionRepository = transactionRepository;
            _planSegmentBAL = planSegmentBAL;
            _logArTxnBAL = logArTxnBAL;
            _cblogBAL = cblogBAL;
            _traninacct = traninacct;
            _apilogBAL = apilogBAL;
            _baseCockroachADO = baseCockroachADO;
            _transactionBAL = transactionBAL;
            _logger = logger;
        }
        public async Task<BaseResponseDTO> AddPaymentADOAsync(PaymentAddDTO paymentaddDTO)
        {
            _timeLogger.Start("AddPayment");
            BaseResponseDTO baseResponseDTO = new BaseResponseDTO();
            Boolean isOk = true;
            Transaction t = new Transaction();
            _timeLogger.Start("BeginTransactionAsync");
            newTuple = await _baseCockroachADO.BeginTransactionAsync();
            _timeLogger.StopAndLog("BeginTransactionAsync");
            int retry = 0;
            int sqlexp = 0;
            //string ccregion = "UNKNOWN-PAYMENT-REGION";
            try
            {
                Guid guid = Guid.NewGuid();
                string savePointName = "payment_restart_" + guid.ToString();

                while (true)
                {
                    if (sqlexp > 0)
                    {
                        _logger.LogInformation("Retry for Account Number : " + paymentaddDTO.accountnumber + " : " + sqlexp);
                    }

                    try
                    {



                        if (paymentaddDTO.amount <= 0) { isOk = false; }
                        if (isOk)
                        {
                            _timeLogger.Start("SavePointAsync");
                            await _baseCockroachADO.SavePointAsync(newTuple.Item1, newTuple.Item2, savePointName);
                            _timeLogger.StopAndLog("SavePointAsync");

                            _timeLogger.Start("GetAccountByNumber_ADO");
                            Account account = _accountBAL.GetAccountByNumber_ADO(paymentaddDTO.accountnumber, paymentaddDTO.ccregion, newTuple.Item1);
                            //ccregion = account.ccregion;
                            _timeLogger.StopAndLog("GetAccountByNumber_ADO");

                            if (account != null)
                            {
                                //Account

                                decimal? Remainingamount = paymentaddDTO.amount;

                                if (account.fees > 0 & Remainingamount >= account.fees)
                                {
                                    account.fees = 0;
                                    Remainingamount = Remainingamount - account.fees;
                                }
                                else if (account.fees > 0 & Remainingamount < account.fees)
                                {
                                    account.fees = account.fees - Remainingamount;
                                    Remainingamount = 0;
                                }

                                if (Remainingamount > 0 & account.interest > 0 & Remainingamount >= account.interest)
                                {
                                    account.interest = 0;
                                    Remainingamount = Remainingamount - account.interest;
                                }
                                else if (Remainingamount > 0 & account.interest > 0 & Remainingamount < account.interest)
                                {
                                    account.interest = account.interest - Remainingamount;
                                    Remainingamount = 0;
                                }

                                if (Remainingamount > 0)
                                {
                                    account.principal = account.principal - Remainingamount;
                                    Remainingamount = 0;
                                }


                                account.currentbal = account.currentbal - paymentaddDTO.amount;
                                account.paymentamount = paymentaddDTO.amount;
                                account.paymentcount = account.paymentcount + 1;

                                //Plansegment

                                _timeLogger.Start("GetPlanSegmentsByAccountID_ADO");
                                List<PlanSegment> planSegments = _planSegmentBAL.GetPlanSegmentsByAccountID_ADO(account.accountid, account.ccregion, newTuple.Item1);
                                _timeLogger.StopAndLog("GetPlanSegmentsByAccountID_ADO");

                                if (planSegments.Count > 0)
                                {
                                    Remainingamount = paymentaddDTO.amount;

                                    //Fees--
                                    foreach (PlanSegment p in planSegments)
                                    {
                                        p.ccregion = account.ccregion;
                                        if (p.fees > 0 & Remainingamount <= p.fees)
                                        {
                                            p.fees = p.fees - Remainingamount;
                                            Remainingamount = 0;
                                        }
                                        else if (p.fees > 0 & Remainingamount > p.fees)
                                        {
                                            decimal? oldFees = p.fees;
                                            p.fees = 0;
                                            Remainingamount = Remainingamount - oldFees;
                                        }

                                        if (Remainingamount == 0) { break; }
                                    }
                                    //Interest
                                    if (Remainingamount > 0)
                                    {
                                        foreach (PlanSegment p in planSegments)
                                        {
                                            if (p.interest > 0 & Remainingamount <= p.interest)
                                            {
                                                p.interest = p.interest - Remainingamount;
                                                Remainingamount = 0;
                                            }
                                            else if (p.interest > 0 & Remainingamount > p.interest)
                                            {
                                                decimal? oldInt = p.interest;
                                                p.interest = 0;
                                                Remainingamount = Remainingamount - oldInt;
                                            }

                                            if (Remainingamount == 0) { break; }
                                        }
                                    }
                                    //Principal-1
                                    if (Remainingamount > 0)
                                    {
                                        foreach (PlanSegment p in planSegments)
                                        {
                                            if (p.principal > 0 & Remainingamount <= p.principal)
                                            {
                                                p.principal = p.principal - Remainingamount;
                                                Remainingamount = 0;
                                            }
                                            else if (p.principal > 0 & Remainingamount > p.principal)
                                            {
                                                decimal? oldPri = p.principal;
                                                p.principal = 0;
                                                Remainingamount = Remainingamount - oldPri;
                                            }

                                            if (Remainingamount == 0) { break; }
                                        }
                                    }

                                    //Principla-2

                                    if (Remainingamount > 0)
                                    {
                                        int idxLast = planSegments.Count - 1;
                                        planSegments[idxLast].principal = planSegments[idxLast].principal - Remainingamount;
                                    }

                                }

                                //Transaction
                                t.trantime = DateTime.Now;
                                t.trantype = "21";
                                t.trancode = 300;
                                t.accountid = account.accountid;
                                t.amount = paymentaddDTO.amount;
                                t.cardnumber = "";
                                t.ccregion = account.ccregion;
                                //Starting Transations
                                //Account updatedAccount = new Account();
                                _timeLogger.Start("UpdateAccountWithPayment");
                                _accountBAL.UpdateAccountWithPayment(account, newTuple.Item1);
                                _timeLogger.StopAndLog("UpdateAccountWithPayment");

                                _timeLogger.Start("UpdatePlanSegmentWithPayment");
                                _planSegmentBAL.UpdatePlanSegmentWithPayment(planSegments, newTuple.Item1);
                                _timeLogger.StopAndLog("UpdatePlanSegmentWithPayment");

                                //newtran = _transactionRepository.Add(t,)
                                _timeLogger.Start("AddTransactionADO");
                                t.tranid = _transactionBAL.AddTransactionADO(t, newTuple.Item1);
                                _timeLogger.StopAndLog("AddTransactionADO");

                                LogArTxn logartxn = new LogArTxn();
                                logartxn.artype = 1;
                                logartxn.tranid = t.tranid;
                                logartxn.businessdate = DateTime.Now;
                                logartxn.status = "success";
                                logartxn.ccregion = account.ccregion;

                                _timeLogger.Start("LogARTxnInsert");
                                _logArTxnBAL.Insert(logartxn, newTuple.Item1);
                                _timeLogger.StopAndLog("LogARTxnInsert");

                                CBLog cblog = new CBLog();
                                cblog.tranid = t.tranid;
                                cblog.accountid = t.accountid;
                                cblog.tranamount = paymentaddDTO.amount;
                                cblog.currentbal = account.currentbal;
                                cblog.posttime = DateTime.Now;
                                cblog.ccregion = account.ccregion;

                                _timeLogger.Start("CBLogInsert");
                                _cblogBAL.Insert(cblog, newTuple.Item1);
                                _timeLogger.StopAndLog("CBLogInsert");

                                Trans_in_Acct tacct = new Trans_in_Acct();
                                tacct.tranid = t.tranid;
                                tacct.accountid = account.accountid;
                                tacct.ccregion = account.ccregion;

                                _timeLogger.Start("TranAccountInsert");
                                _traninacct.Insert(tacct, newTuple.Item1);
                                _timeLogger.StopAndLog("TranAccountInsert");

                                await _baseCockroachADO.CommitTransactionAsync(newTuple.Item1, newTuple.Item2);
                                baseResponseDTO.DataLayerTime = _timeLogger.StopAndLog("AddPayment");
                                baseResponseDTO.BaseEntityInstance = t;
                                return baseResponseDTO;



                            }
                            else
                            {
                                await _baseCockroachADO.RollbackTransactionAsync(newTuple.Item1, newTuple.Item2);
                                baseResponseDTO.DataLayerTime = _timeLogger.StopAndLog("AddPayment");
                                baseResponseDTO.BaseEntityInstance = "Error{'Message':'Account does not exist.'}";
                                return baseResponseDTO;
                            }


                        }
                        else
                        {
                            await _baseCockroachADO.RollbackTransactionAsync(newTuple.Item1, newTuple.Item2);
                            baseResponseDTO.DataLayerTime = _timeLogger.StopAndLog("AddPayment");
                            baseResponseDTO.BaseEntityInstance = "Error{'Message':'Amount is Zero.'}";
                            return baseResponseDTO;
                        }

                    }
                    catch (TimeoutException te)
                    {
                        retry++;
                        if (retry == 5)
                            throw;
                        await _baseCockroachADO.RollbackTransactionAsync(newTuple.Item1, newTuple.Item2, savePointName);
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
                                await _baseCockroachADO.RollbackTransactionAsync(newTuple.Item1, newTuple.Item2, savePointName);
                            }
                            catch (Exception tx)
                            {
                                if (tx.Message.Contains("This NpgsqlTransaction has completed"))
                                {
                                    //baseResponseDTO.DataLayerTime = _timeLogger.StopAndLog("AddPayment");
                                    baseResponseDTO.BaseEntityInstance = t;
                                    return baseResponseDTO;
                                }
                                else
                                {
                                    _logger.LogError(tx, "AddPaymentADOAsync");
                                }
                            }
                        }
                        else
                        {
                            _logger.LogError(e, "AddPaymentADOAsync ErrorCode:");
                            throw;
                        }

                    }
                    sqlexp++;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "AddPaymentADOAsync");
                await _baseCockroachADO.RollbackTransactionAsync(newTuple.Item1, newTuple.Item2);


                //if (!ex.Message.ToLower().Contains("connect") && !ex.Message.ToLower().Contains("auth"))
                //{
                //    await _baseCockroachADO.RollbackTransactionAsync(newTuple.Item1, newTuple.Item2);
                //}
                responseTy = -1;
                throw;
            }
            finally
            {
                // APILog
                APILog aPILog = new APILog();
                aPILog.apiname = "Payment";
                aPILog.logtime = DateTime.UtcNow;
                aPILog.response = responseTy;
                //aPILog.ccregion = ccregion;
                _apilogBAL.Insert(aPILog);

            }





        }

        public BaseResponseDTO CheckDBConnection()
        {
            BaseResponseDTO baseResponseDTO = new BaseResponseDTO();
            try
            {
                if (_baseCockroachADO.OpenConnection())
                {
                    //_logger.LogInformation("Connection Opened;");
                    _baseCockroachADO.CloseConnection();
                    //_logger.LogInformation("Connection closed;");
                }

                baseResponseDTO.BaseEntityInstance = "Result{'Message':'API Responded Successfully for connection open and close'}";
            }
            catch (Exception ex)
            {

                throw;
            }
            //baseResponseDTO.BaseEntityInstance = "Result{'Message':'API Responded Successfully'}";
            return baseResponseDTO;
        }

        public async Task<string> CheckDBTransaction()
        {
            // BaseResponseDTO baseResponseDTO = new BaseResponseDTO();
            string message = "";
            try
            {
                try
                {
                    newTuple = await _baseCockroachADO.BeginTransactionAsync();
                    //_logger.LogInformation("Transaction Opened;");
                    await _baseCockroachADO.CommitTransactionAsync(newTuple.Item1, newTuple.Item2);
                    //_logger.LogInformation("Transaction Committed;");
                }
                catch (Exception e)
                {
                    await _baseCockroachADO.RollbackTransactionAsync(newTuple.Item1, newTuple.Item2);
                    //_logger.LogInformation("Transaction RolledBack;");
                }

                message = "Result{'Message':'API Responded Successfully for transaction open and close'}";
            }
            catch (Exception ex)
            {
                message = "Result{'Message':'Error Occurred for transaction open and close'} " + ex.StackTrace;
            }
            //baseResponseDTO.BaseEntityInstance = "Result{'Message':'API Responded Successfully'}";
            return message;
        }

        //public async Task<BaseResponseDTO> AddPaymentAsync(PaymentAddDTO paymentaddDTO)
        //{
        //    BaseResponseDTO baseResponseDTO = new BaseResponseDTO();
        //    Boolean isOk = true;
        //    String errorMsg = "";
        //    try
        //    {
        //        _timeLogger.Start("AddPayment");
        //        Transaction t = new Transaction();
        //        Transaction newtran = new Transaction();
        //        // Validation
        //        if (paymentaddDTO.amount <= 0) { isOk = false; errorMsg = "Payment Amount is Zero!"; }
        //        if (isOk)
        //        {
        //            Account account = await _accountBAL.GetAccountByNumber(paymentaddDTO.accountnumber);
        //            if (account == null)
        //            {
        //                errorMsg = "Invalid Account No!";

        //            }
        //            else
        //            {
        //                //Account
        //                List<PlanSegment> planSegments = await _planSegmentBAL.GetPlanSegmentByAccountId(account.accountid);

        //                decimal? Remainingamount = paymentaddDTO.amount;

        //                if (account.fees > 0 & Remainingamount >= account.fees)
        //                {
        //                    account.fees = 0;
        //                    Remainingamount = Remainingamount - account.fees;
        //                }
        //                else if (account.fees > 0 & Remainingamount < account.fees)
        //                {
        //                    account.fees = account.fees - Remainingamount;
        //                    Remainingamount = 0;
        //                }

        //                if (Remainingamount > 0 & account.interest > 0 & Remainingamount >= account.interest)
        //                {
        //                    account.interest = 0;
        //                    Remainingamount = Remainingamount - account.interest;
        //                }
        //                else if (Remainingamount > 0 & account.interest > 0 & Remainingamount < account.interest)
        //                {
        //                    account.interest = account.interest - Remainingamount;
        //                    Remainingamount = 0;
        //                }

        //                if (Remainingamount > 0)
        //                {
        //                    account.principal = account.principal - Remainingamount;
        //                    Remainingamount = 0;
        //                }


        //                account.currentbal = account.currentbal - paymentaddDTO.amount;
        //                account.paymentamount = paymentaddDTO.amount;
        //                account.paymentcount = account.paymentcount + 1;

        //                await _accountBAL.UpdateAsync(account, account.accountid);

        //                //Plansegment



        //                Remainingamount = paymentaddDTO.amount;

        //                //Fees--
        //                foreach (PlanSegment p in planSegments)
        //                {
        //                    if (p.fees > 0 & Remainingamount <= p.fees)
        //                    {
        //                        p.fees = p.fees - Remainingamount;
        //                        Remainingamount = 0;
        //                    }
        //                    else if (p.fees > 0 & Remainingamount > p.fees)
        //                    {
        //                        decimal? oldFees = p.fees;
        //                        p.fees = 0;
        //                        Remainingamount = Remainingamount - oldFees;
        //                    }

        //                    if (Remainingamount == 0) { break; }
        //                }
        //                //Interest
        //                if (Remainingamount > 0)
        //                {
        //                    foreach (PlanSegment p in planSegments)
        //                    {
        //                        if (p.interest > 0 & Remainingamount <= p.interest)
        //                        {
        //                            p.interest = p.interest - Remainingamount;
        //                            Remainingamount = 0;
        //                        }
        //                        else if (p.interest > 0 & Remainingamount > p.interest)
        //                        {
        //                            decimal? oldInt = p.interest;
        //                            p.interest = 0;
        //                            Remainingamount = Remainingamount - oldInt;
        //                        }

        //                        if (Remainingamount == 0) { break; }
        //                    }
        //                }
        //                //Principal-1
        //                if (Remainingamount > 0)
        //                {
        //                    foreach (PlanSegment p in planSegments)
        //                    {
        //                        if (p.principal > 0 & Remainingamount <= p.principal)
        //                        {
        //                            p.principal = p.principal - Remainingamount;
        //                            Remainingamount = 0;
        //                        }
        //                        else if (p.principal > 0 & Remainingamount > p.principal)
        //                        {
        //                            decimal? oldPri = p.principal;
        //                            p.principal = 0;
        //                            Remainingamount = Remainingamount - oldPri;
        //                        }

        //                        if (Remainingamount == 0) { break; }
        //                    }
        //                }

        //                //Principla-2

        //                if (Remainingamount > 0)
        //                {
        //                    int idxLast = planSegments.Count - 1;
        //                    planSegments[idxLast].principal = planSegments[idxLast].principal - Remainingamount;
        //                }
        //                foreach (PlanSegment p in planSegments)
        //                {
        //                    await _planSegmentBAL.UpdateAsync(p, p.planid);
        //                }

        //                //Transaction
        //                t.trantime = DateTime.Now;
        //                t.trantype = "21";
        //                t.trancode = 300;
        //                t.accountid = account.accountid;
        //                t.amount = paymentaddDTO.amount;



        //                newtran = await _transactionRepository.AddAsync(t);

        //                await _logArTxnBAL.AddLogArTxnAsync(newtran.tranid);
        //                await _cblogBAL.AddCBLogAsync(newtran.tranid, account.accountid, paymentaddDTO.amount, account.currentbal);
        //                await _traninacct.AddTansInAcctAsync(newtran.tranid, account.accountid);
        //                await _transactionRepository.SaveAsync();
        //            }
        //        }

        //        baseResponseDTO.DataLayerTime = _timeLogger.StopAndLog("AddPayment");



        //        if (errorMsg != "")
        //        {
        //            throw new Exception(errorMsg);
        //        }
        //        baseResponseDTO.BaseEntityInstance = newtran;
        //        return baseResponseDTO;
        //    }
        //    catch (Exception ex)
        //    {
        //        _transactionRepository.RejectChanges();
        //        responseTy = -1;
        //        throw;
        //    }
        //    finally
        //    {
        //        // APILog
        //        APILog aPILog = new APILog();
        //        //aPILog.logid = Guid.NewGuid();
        //        aPILog.apiname = "Payment";
        //        aPILog.logtime = DateTime.UtcNow;
        //        aPILog.response = responseTy;
        //        _apilogBAL.Add(aPILog);
        //        _transactionRepository.Save();
        //    }


        //    /*Customer customer = CustomerAddDTO.MapToCustomer(paymentaddDTO);
        //    //customer.customerid = Guid.NewGuid();
        //    customer.ssn = SSNGenerator.GenerateSSN();
        //    Customer newCustomer = await _customerRepository.AddAsync(customer);
        //    await _addressBAL.AddAddressAsync(paymentaddDTO.CustomerAddress, newCustomer.customerid);
        //    return newCustomer;*/
        //    //return new Transaction();
        //}

    }
}
