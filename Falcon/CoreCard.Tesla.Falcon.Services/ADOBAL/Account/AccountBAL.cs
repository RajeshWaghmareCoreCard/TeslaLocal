using CoreCard.Tesla.Utilities;
using CoreCard.Tesla.Falcon.DataModels.Entity;
using CoreCard.Tesla.Falcon.DataModels.Model;
using System;
using System.Threading.Tasks;
using System.Transactions;
using System.Data.SqlClient;
using System.Linq;
using CoreCard.Tesla.Falcon.ADORepository;

namespace CoreCard.Tesla.Falcon.Services
{
    public class AccountBAL : IAccountBAL
    {
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

        public AccountBAL(TimeLogger timeLogger,  IADOAccountRepository adoAccountRepository) 
        {
            _timeLogger = timeLogger;
            _adoaccountRepository = adoAccountRepository;
        }
        public AccountBAL(TimeLogger timeLogger,IADOAccountRepository adoAccountRepository,
            ICustomerBAL customerBAL, IEmbossingBAL embossingBAL, IBaseCockroachADO baseCockroachADO, IAddressBAL addressBAL,
            ILoyaltyPlanBAL loyaltyPlanBAL, IAPILogBAL aPILogBAL)
        {
            _customerBAL = customerBAL;
            _embossingBAL = embossingBAL;
            _loyaltyPlanBAL = loyaltyPlanBAL;
            _aPILogBAL = aPILogBAL;
            _timeLogger = timeLogger;
            _adoaccountRepository = adoAccountRepository;
            _baseCockroachADO = baseCockroachADO;
            _addressBAL = addressBAL;
        }

        public async Task<BaseResponseDTO> AddAccountAsync(CustomerAddDTO customerDTO)
        {
            lock (lockObject)
            {
                // ToDo: Nitin Currency symbol is not saved yet:-Create amount related columns into decimal type column.But currency symbol is not saved anywhere. It's open point to discuss.
                // ToDo: Nitin Encryption: -SSN, CardNumber, Account Number is saved in plain text format.Encryption part is pending.
                // ToDo: Nitin Auto Generation:-Generate CardNumber and AccountNumber using random number. Actual logic is pending.
                // ToDo: Nitin Validation Logic is pending.
                int responseType = 0;
                Account newAccount;
                BaseResponseDTO baseResponseDTO = new BaseResponseDTO();
                newTuple = _baseCockroachADO.BeginTransaction();
                try
                {

                    _timeLogger.Start("AddAccountBAL");

                    //using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }, TransactionScopeAsyncFlowOption.Enabled))
                    //using (var scope = new TransactionScope( TransactionScopeAsyncFlowOption.Enabled))
                    //{

                    // Create Customer Object
                    //Customer newCustomer = await _customerBAL.AddCustomerAsync(customerDTO);
                    Guid newCustomerId = _customerBAL.Insert(CustomerAddDTO.MapToCustomer(customerDTO), newTuple.Item1);

                    Address newAddres = AddressAddDTO.MapToAddress(customerDTO.CustomerAddress);
                    newAddres.customerid = newCustomerId;
                    newAddres.addresstype = 0;
                    _addressBAL.Insert(newAddres, newTuple.Item1);
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

                    //await _accountRepository.AddAsync(newAccount);
                    Guid accountId = _adoaccountRepository.Insert(newAccount, newTuple.Item1);
                    newAccount.accountid = accountId;
                    // Embossing – (Generate CardNumber, encrypt and store)
                    //await _embossingBAL.AddEmbossingAsync(newAccount.accountid);
                    _embossingBAL.Insert(accountId, newTuple.Item1);
                    // LoyaltyPlan
                    //await _loyaltyPlanBAL.AddLoyaltyPlanAsync(newAccount.accountid);
                    LoyaltyPlan loyaltyPlan = new LoyaltyPlan();
                    //loyaltyPlan.loyaltyplanid = Guid.NewGuid();
                    loyaltyPlan.accountid = accountId;
                    loyaltyPlan.loyaltyplantype = 0;
                    loyaltyPlan.rewardbal = 0;
                    _loyaltyPlanBAL.Insert(loyaltyPlan, newTuple.Item1);

                    _baseCockroachADO.CommitTransaction(newTuple.Item1, newTuple.Item2);

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

                    _baseCockroachADO.RollbackTransaction(newTuple.Item1, newTuple.Item2);
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
                    _aPILogBAL.Insert(aPILog);
                    //_accountRepository.Save();
                }
            }
        }

        public Account GetAccountByID_ADO(Guid id)
        {
            return _adoaccountRepository.Get(id);
        }

        public Account UpdatePurchase(Account t)
        {
           return  _adoaccountRepository.UpdatePurchase(t);
        }

        public Account UpdatePurchase(Account t, DBAdapter.IDataBaseCommand dbCommand)
        {
            return _adoaccountRepository.UpdatePurchase(t, dbCommand);
        }

        public Account UpdateAccountWithPayment(Account t, DBAdapter.IDataBaseCommand dbCommand)
        {
            return _adoaccountRepository.UpdateAccountWithPayment(t, dbCommand);
        }

        public Account GetAccountByNumber_ADO(UInt64 AccountNumber)
        {
            return _adoaccountRepository.Get(AccountNumber);
        }
    }
}
