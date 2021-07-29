using CoreCard.Tesla.Falcon.DataModels.Entity;
using CoreCard.Tesla.Falcon.DataModels.Model;
using CoreCard.Tesla.Falcon.NpgRepository.Interface;
using Microsoft.Extensions.Logging;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreCard.Tesla.Falcon.NpgRepository
{
    public class PurchaseUnit : BaseRepository, IPurchaseUnit
    {
        //public Account Account { get; set; }
        //public Transaction Transaction { get; set; }
        //public PlanSegment PlanSegment { get; set; }
        //public CBLog CBLog { get; set; }
        //public LogArTxn LogArTxn { get; set; }
        //public Trans_in_Acct trans_In_Acct { get; set; }
        //public PaymentAddDTO paymentaddDTO { get; set; }
        private readonly IADOAccountRepository _accountRepository;
        private readonly IADOPlansegmentRepository _plansegmentRepository;
        private readonly IADOTransactionRepository _transactionRepository;
        private readonly IADOLogArTxnRepository _logArTxnRepository;
        private readonly IADOCBLogRepository _cblogRepository;
        private readonly IADOTranInAcctRepository _tranInAcctRepository;
        private readonly ILogger<PurchaseUnit> _logger;
        public PurchaseUnit(IDatabaseConnectionResolver databaseConnection, IADOAccountRepository accountRepository, IADOPlansegmentRepository plansegmentRepository,
            IADOTransactionRepository transactionRepository, IADOLogArTxnRepository logArTxnRepository, IADOCBLogRepository cblogRepository, IADOTranInAcctRepository tranInAcctRepository, ILogger<PurchaseUnit> logger) : base(databaseConnection)
        {
            _accountRepository = accountRepository;
            _plansegmentRepository = plansegmentRepository;
            _transactionRepository = transactionRepository;
            _tranInAcctRepository = tranInAcctRepository;
            _logArTxnRepository = logArTxnRepository;
            _cblogRepository = cblogRepository;
            _logger = logger;
        }

        public async Task<Transaction> MakePayment(PaymentAddDTO paymentaddDTO)
        {
            Transaction t = new Transaction();
            try
            {
                using (NpgsqlConnection connection = databaseConnection.GetNpgsqlConnection())
                {
                    await connection.OpenAsync();
                    NpgsqlTransaction tran = await connection.BeginTransactionAsync();
                    Guid guid = Guid.NewGuid();
                    string savePointName = "payment_restart_" + guid.ToString();
                    await tran.SaveAsync(savePointName);
                    while (true)
                    {
                        try
                        {
                            Account account = _accountRepository.GetAccountByNumber(Convert.ToInt64(paymentaddDTO.accountnumber), connection);
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

                                List<PlanSegment> planSegments = _plansegmentRepository.Get(account.accountid, connection);

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
                                //Starting Transations
                                //Account updatedAccount = new Account();

                                _accountRepository.UpdateAccountWithPayment(account, connection);
                                _plansegmentRepository.UpdatePlanSegmentWithPayment(planSegments, connection);

                                //newtran = _transactionRepository.Add(t,)
                                t.tranid = _transactionRepository.Add(t, connection);

                                LogArTxn logartxn = new LogArTxn();
                                logartxn.artype = 1;
                                logartxn.tranid = t.tranid;
                                logartxn.businessdate = DateTime.Now;
                                logartxn.status = "success";

                                _logArTxnRepository.Insert(logartxn, connection);


                                CBLog cblog = new CBLog();
                                cblog.tranid = t.tranid;
                                cblog.accountid = t.accountid;
                                cblog.tranamount = paymentaddDTO.amount;
                                cblog.currentbal = account.currentbal;
                                cblog.posttime = DateTime.Now;

                                _cblogRepository.Insert(cblog, connection);

                                Trans_in_Acct tacct = new Trans_in_Acct();
                                tacct.tranid = t.tranid;
                                tacct.accountid = account.accountid;

                                _tranInAcctRepository.Insert(tacct, connection);

                                await tran.CommitAsync();
                                //baseResponseDTO.DataLayerTime = _timeLogger.StopAndLog("AddPayment");

                                return t;


                            }
                        }
                        catch (NpgsqlException e)
                        {
                            if (e.SqlState == "40001")
                            {
                                // Signal the database that we will attempt a retry.
                                try
                                {
                                    await tran.RollbackAsync(savePointName);
                                }
                                catch (Exception tx)
                                {
                                    if (tx.Message.Contains("This NpgsqlTransaction has completed"))
                                    {
                                        //baseResponseDTO.DataLayerTime = _timeLogger.StopAndLog("AddPayment");
                                       
                                        return t;
                                    }
                                    else
                                    {
                                        _logger.LogError(tx, "AddPaymentADOAsync");
                                    }
                                }
                            }
                            else
                            {
                                _logger.LogError(e, "Error Occured in Purchase Unit");
                                throw;
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Occured in Purchase Unit");
                throw;
            }
        }
    }
}
