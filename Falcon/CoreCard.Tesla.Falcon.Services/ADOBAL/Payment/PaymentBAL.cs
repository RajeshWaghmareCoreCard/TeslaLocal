using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using CoreCard.Tesla.Falcon.ADORepository;
using CoreCard.Tesla.Utilities;
using CoreCard.Tesla.Falcon.DataModels.Model;
using CoreCard.Tesla.Falcon.DataModels.Entity;

namespace CoreCard.Tesla.Falcon.Services
{
    public class PaymentBAL :  IPaymentBAL
    {

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
        public PaymentBAL(TimeLogger timeLogger
           , IAccountBAL accountBAL, IPlanSegmentBAL planSegmentBAL, ILogArTxnBAL logArTxnBAL
            , ICBLogBAL cblogBAL, ITransInAcctBAL traninacct, IAPILogBAL apilogBAL) //: base(transactionRepository)
        {
            _timeLogger = timeLogger;
            //_transactionRepository = transactionRepository;
            _accountBAL = accountBAL;
            _planSegmentBAL = planSegmentBAL;
            _logArTxnBAL = logArTxnBAL;
            _cblogBAL = cblogBAL;
            _traninacct = traninacct;
            _apilogBAL = apilogBAL;
           
        }
        public PaymentBAL(TimeLogger timeLogger
           , IBaseCockroachADO baseCockroachADO
           , IAccountBAL accountBAL, IPlanSegmentBAL planSegmentBAL, ILogArTxnBAL logArTxnBAL
            , ICBLogBAL cblogBAL, ITransInAcctBAL traninacct, IAPILogBAL apilogBAL, ITransactionBAL transactionBAL) //: base(transactionRepository)
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
        }
        public async Task<BaseResponseDTO> AddPaymentADOAsync(PaymentAddDTO paymentaddDTO)
        {
            BaseResponseDTO baseResponseDTO = new BaseResponseDTO();
            Boolean isOk = true;
            String errorMsg = "";
            Transaction t = new Transaction();
            
            lock (lockObject)
            {

                try
                {
                    _timeLogger.Start("AddPayment");
                    if (paymentaddDTO.amount <= 0) { isOk = false; errorMsg = "Payment Amount is Zero!"; }
                    if (isOk)
                    {
                       
                        Account account = _accountBAL.GetAccountByNumber_ADO(paymentaddDTO.accountnumber);
                        if (account == null)
                        {
                            errorMsg = "Invalid Account No!";
                        }
                        else
                        {
                            //Account

                            decimal Remainingamount = paymentaddDTO.amount;

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

                            List<PlanSegment> planSegments = _planSegmentBAL.GetPlanSegmentsByAccountID_ADO(account.accountid);

                            if (planSegments.Count > 0)
                            {
                                Remainingamount = paymentaddDTO.amount;

                                //Fees--
                                foreach (PlanSegment p in planSegments)
                                {
                                    if (p.fees > 0 & Remainingamount <= p.fees)
                                    {
                                        p.fees = p.fees - Remainingamount;
                                        Remainingamount = 0;
                                    }
                                    else if (p.fees > 0 & Remainingamount > p.fees)
                                    {
                                        decimal oldFees = p.fees;
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
                                            decimal oldInt = p.interest;
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
                                            decimal oldPri = p.principal;
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
                            //Starting Transations
                            Account updatedAccount = new Account();
                            newTuple = _baseCockroachADO.BeginTransaction();
                            updatedAccount = _accountBAL.UpdateAccountWithPayment(account, newTuple.Item1);
                            _planSegmentBAL.UpdatePlanSegmentWithPayment(planSegments, newTuple.Item1);

                            //newtran = _transactionRepository.Add(t,)
                            t.tranid=_transactionBAL.AddTransactionADO(t, newTuple.Item1);

                            LogArTxn logartxn = new LogArTxn();
                            logartxn.artype = 1;
                            logartxn.tranid = t.tranid;
                            logartxn.businessdate = DateTime.Now;
                            logartxn.status = "success";

                            _logArTxnBAL.Insert(logartxn, newTuple.Item1);


                            CBLog cblog = new CBLog();
                            cblog.tranid = t.tranid;
                            cblog.accountid = t.accountid;
                            cblog.tranamount = paymentaddDTO.amount;
                            cblog.currentbal = account.currentbal;
                            cblog.posttime = DateTime.Now;

                            _cblogBAL.Insert(cblog, newTuple.Item1);

                            Trans_in_Acct tacct = new Trans_in_Acct();
                            tacct.tranid = t.tranid;
                            tacct.accountid = account.accountid;

                            _traninacct.Insert(tacct, newTuple.Item1);

                            _baseCockroachADO.CommitTransaction(newTuple.Item1, newTuple.Item2);

                            //    scope.Complete();
                            //}
                            

                            
                        }
                        
                        
                    }
                    if (errorMsg != "")
                    {
                        throw new Exception(errorMsg);
                    }
                    long dalTimeTaken = _timeLogger.StopAndLog("AddPayment");
                    baseResponseDTO.BaseEntityInstance =t;
                    baseResponseDTO.DataLayerTime = dalTimeTaken;
                    return baseResponseDTO;
                }
                catch (Exception ex)
                {
                    
                    if (errorMsg == "" && !ex.Message.ToLower().Contains("connect") && !ex.Message.ToLower().Contains("auth"))
                    {
                        _baseCockroachADO.RollbackTransaction(newTuple.Item1, newTuple.Item2);
                    }
                    errorMsg = ex.Message;
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
                    _apilogBAL.Insert(aPILog);

                }
            }
            
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

        //                decimal Remainingamount = paymentaddDTO.amount;

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

        //                List<PlanSegment> planSegments = await _planSegmentBAL.GetPlanSegmentByAccountId(account.accountid);

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
        //                        decimal oldFees = p.fees;
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
        //                            decimal oldInt = p.interest;
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
        //                            decimal oldPri = p.principal;
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


            /*Customer customer = CustomerAddDTO.MapToCustomer(paymentaddDTO);
            //customer.customerid = Guid.NewGuid();
            customer.ssn = SSNGenerator.GenerateSSN();
            Customer newCustomer = await _customerRepository.AddAsync(customer);
            await _addressBAL.AddAddressAsync(paymentaddDTO.CustomerAddress, newCustomer.customerid);
            return newCustomer;*/
            //return new Transaction();
        //}


    }
}
