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
using Microsoft.Extensions.Configuration;

namespace CoreCard.Tesla.Falcon.Services
{
    public class AccountBAL : /*BaseBAL<Account>,*/ IAccountBAL
    {
        //private readonly IAccountRepository _accountRepository;
        private readonly IADOAccountRepository _adoaccountRepository;
        private readonly ICustomerBAL _customerBAL;
        private readonly IEmbossingBAL _embossingBAL;
        private readonly ILoyaltyPlanBAL _loyaltyPlanBAL;
        private readonly IAPILogBAL _aPILogBAL;
        private readonly TimeLogger _timeLogger;
        private readonly IBaseCockroachADO _baseCockroachADO;
        private readonly IAddressBAL _addressBAL;
        private Tuple<DBAdapter.IDataBaseCommand, object> newTuple;

        private readonly object lockObject = new object();
        protected IConfiguration _configuration;

        public AccountBAL(TimeLogger timeLogger, /*IAccountRepository accountRepository,*/ IADOAccountRepository adoAccountRepository, IConfiguration configuration) //: base(accountRepository)
        {
            //_accountRepository = accountRepository;
            _timeLogger = timeLogger;
            _adoaccountRepository = adoAccountRepository;
            _configuration = configuration;
        }
        public AccountBAL(TimeLogger timeLogger, /*IAccountRepository accountRepository, */IADOAccountRepository adoAccountRepository,IAddressBAL addressBAL,
            ICustomerBAL customerBAL, IEmbossingBAL embossingBAL, IBaseCockroachADO baseCockroachADO,
            ILoyaltyPlanBAL loyaltyPlanBAL, IAPILogBAL aPILogBAL, IConfiguration configuration) //: base(accountRepository)
        {
            //_accountRepository = accountRepository;
            _customerBAL = customerBAL;
            _embossingBAL = embossingBAL;
            _loyaltyPlanBAL = loyaltyPlanBAL;
            _aPILogBAL = aPILogBAL;
            _timeLogger = timeLogger;
            //_adoaccountRepository = adoAccountRepository;
            _baseCockroachADO = baseCockroachADO;
            _addressBAL = addressBAL;
            _configuration = configuration;
        }
        //public async Task<Account> GetAccountByNumber(UInt64 AccountNumber)
        //{
        //    string query = "Select  * FROM ACCOUNT WHERE accountnumber=" + AccountNumber.ToString();
        //    //SqlParameter parameterS = new SqlParameter("@AccountNumber", AccountNumber);
        //    Account account = await base.GetEntityAsync(query);
        //    return account;
        //}
        public async Task<BaseResponseDTO> AddAccountAsync(CustomerAddDTO customerDTO)
        {
            // lock (lockObject) {
            // ToDo: Nitin Currency symbol is not saved yet:-Create amount related columns into decimal type column.But currency symbol is not saved anywhere. It's open point to discuss.
            // ToDo: Nitin Encryption: -SSN, CardNumber, Account Number is saved in plain text format.Encryption part is pending.
            // ToDo: Nitin Auto Generation:-Generate CardNumber and AccountNumber using random number. Actual logic is pending.
            // ToDo: Nitin Validation Logic is pending.
            int responseType = 0;
            Account newAccount;
            _timeLogger.Start("AddAccountBAL");
            BaseResponseDTO baseResponseDTO = new BaseResponseDTO();
            _timeLogger.Start("BeginTransactionAsync");
            newTuple = await _baseCockroachADO.BeginTransactionAsync();
            _timeLogger.StopAndLog("BeginTransactionAsync");
            //string ccregion = "";
            try
            {

                _timeLogger.Start("AddAccountBAL");

                //using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }, TransactionScopeAsyncFlowOption.Enabled))
                //using (var scope = new TransactionScope( TransactionScopeAsyncFlowOption.Enabled))
                //{
                //ccregion = customerDTO.ccregion;// Convert.ToString(_configuration.GetSection("ccregion").Value);
                // Create Customer Object
                Customer newCustomer = CustomerAddDTO.MapToCustomer(customerDTO);
                newCustomer.ccregion = customerDTO.ccregion; 
                _timeLogger.Start("CustomerInsert");
                Guid newCustomerId = _customerBAL.Insert(newCustomer, newTuple.Item1);
                _timeLogger.StopAndLog("CustomerInsert");

                Address newAddres = AddressAddDTO.MapToAddress(customerDTO.CustomerAddress);
                newAddres.customerid = newCustomerId;
                newAddres.addresstype = 0;
                newAddres.ccregion = customerDTO.ccregion; 
                _timeLogger.Start("AddressInsert");
                _addressBAL.Insert(newAddres, newTuple.Item1);
                _timeLogger.StopAndLog("AddressInsert");
                // Create Account Object
                newAccount = new Account();
                //newAccount.accountid = Guid.NewGuid();
                newAccount.accountnumber = Convert.ToInt64(AccountNoGenerator.RandomAccountNumber());
                newAccount.customerid = newCustomerId;// newCustomer.customerid;
                newAccount.creditlimit = 5000;
                newAccount.currentbal = 0;
                newAccount.principal = 0;
                newAccount.purchaseamount = 0;
                newAccount.fees = 0;
                newAccount.interest = 0;
                newAccount.purchasecount = 0;
                newAccount.paymentamount = 0;
                newAccount.paymentcount = 0;
                newAccount.ccregion = customerDTO.ccregion; 
                //await _accountRepository.AddAsync(newAccount);
                _timeLogger.Start("AccountInsert");
                Guid accountId = _adoaccountRepository.Insert(newAccount, newTuple.Item1);
                _timeLogger.StopAndLog("AccountInsert");
                newAccount.accountid = accountId;
                // Embossing – (Generate CardNumber, encrypt and store)
                //await _embossingBAL.AddEmbossingAsync(newAccount.accountid);
                _timeLogger.Start("EmbossingInsert");
                _embossingBAL.Insert(accountId, newTuple.Item1);
                _timeLogger.StopAndLog("EmbossingInsert");
                // LoyaltyPlan
                //await _loyaltyPlanBAL.AddLoyaltyPlanAsync(newAccount.accountid);
                LoyaltyPlan loyaltyPlan = new LoyaltyPlan();
                //loyaltyPlan.loyaltyplanid = Guid.NewGuid();
                loyaltyPlan.accountid = accountId;
                loyaltyPlan.loyaltyplantype = 0;
                loyaltyPlan.rewardbal = 0;
                loyaltyPlan.ccregion = customerDTO.ccregion; 
                _timeLogger.Start("AccountLoyaltyplan");
                _loyaltyPlanBAL.Insert(loyaltyPlan, newTuple.Item1);
                _timeLogger.StopAndLog("AccountLoyaltyplan");

                _timeLogger.Start("AccountCommitTransactionAsync");
                await _baseCockroachADO.CommitTransactionAsync(newTuple.Item1, newTuple.Item2);
                _timeLogger.StopAndLog("AccountCommitTransactionAsync");

                //    scope.Complete();
                //}
                long dalTimeTaken = _timeLogger.StopAndLog("AddAccountBAL");

                baseResponseDTO.BaseEntityInstance = newAccount;
                baseResponseDTO.DataLayerTime = dalTimeTaken;
                return baseResponseDTO;
            }
            catch (Exception ex)
            {
                //_accountRepository.RejectChanges();

                await _baseCockroachADO.RollbackTransactionAsync(newTuple.Item1, newTuple.Item2);
                responseType = -1;
                throw;
            }
            finally
            {
                // APILog
                APILog aPILog = new APILog();
                //aPILog.logid = Guid.NewGuid();
                aPILog.apiname = "CreateAccount";
                aPILog.logtime = DateTime.UtcNow;
                aPILog.response = responseType;
                aPILog.ccregion = customerDTO.ccregion; 
                _aPILogBAL.Insert(aPILog);
                //_accountRepository.Save();
            }
            //}
        }

        //public async Task<BaseResponseDTO> UpdateAccountAsync(Account account)
        //{
        //    _timeLogger.Start("UpdateAccount");
        //    Account updatetedAccount = _accountRepository.Get(account.accountid);
        //    updatetedAccount.column1 = "Column1 Value-" + DateTime.Now.ToString();
        //    await _accountRepository.UpdateAsync(updatetedAccount, updatetedAccount.accountid);
        //    await _accountRepository.SaveAsync();
        //    long elapsedTime = _timeLogger.StopAndLog("UpdateAccount");

        //    BaseResponseDTO baseResponseDTO = new BaseResponseDTO();
        //    baseResponseDTO.BaseEntityInstance = updatetedAccount;
        //    baseResponseDTO.DataLayerTime = elapsedTime;
        //    return baseResponseDTO;
        //}

        //public async Task<BaseResponseDTO> GetAccountNoAsync()
        //{
        //    _timeLogger.Start("GetAccountNo");
        //    long[] accountNoList = _accountRepository.GetAll().Select(i => i.accountnumber).ToArray();

        //    long elapsedTime = _timeLogger.StopAndLog("GetAccountNo");

        //    BaseResponseDTO baseResponseDTO = new BaseResponseDTO();
        //    baseResponseDTO.BaseEntityInstance = accountNoList;
        //    baseResponseDTO.DataLayerTime = elapsedTime;
        //    return baseResponseDTO;
        //}

        public Account GetAccountByID_ADO(Guid id)
        {
            return _adoaccountRepository.Get(id);
        }

        public Account GetAccountByNumber_ADO(UInt64 AccountNumber)
        {
            return _adoaccountRepository.Get(AccountNumber);
        }
        public Account GetAccountByNumber_ADO(UInt64 AccountNumber, string ccregion, DBAdapter.IDataBaseCommand dbCommand)
        {
            return _adoaccountRepository.GetAccountByNumber(AccountNumber, ccregion, dbCommand);
        }
        public Account UpdatePurchase(Account t)
        {
            return _adoaccountRepository.UpdatePurchase(t);
        }

        public Account UpdatePurchase(Account t, DBAdapter.IDataBaseCommand dbCommand)
        {
            return _adoaccountRepository.UpdatePurchase(t, dbCommand);
        }
        public void UpdateAccountWithPayment(Account t, DBAdapter.IDataBaseCommand dbCommand)
        {
            _adoaccountRepository.UpdateAccountWithPayment(t, dbCommand);
        }

        public Account GetAccountByID_ADO(Guid id, string ccregion, IDataBaseCommand dbCommand)
        {
            return _adoaccountRepository.GetAccountByID(id, ccregion, dbCommand);
        }
    }
}
