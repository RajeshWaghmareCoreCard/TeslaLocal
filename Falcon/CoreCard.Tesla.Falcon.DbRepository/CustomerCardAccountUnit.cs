using System;
using System.Threading.Tasks;
using System.Transactions;
using CoreCard.Tesla.Common;
using CoreCard.Tesla.Falcon.DataModels.Common;
using CoreCard.Tesla.Falcon.DataModels.Repository;
using CoreCard.Tesla.Falcon.DbRepository.RepoInterfaces;
using Npgsql;

namespace CoreCard.Tesla.Falcon.DbRepository
{
    public class CustomerCardAccountUnit : BaseRepository, IDisposable, ICustomerCardAccountUnit
    {

        public CustomerCardAccountUnit(ICustomerRepository customerRepository, IAccountRepository accountRepository, ICardRepository cardRepository, IDatabaseConnectionResolver databaseConnection) : base(databaseConnection)
        {
            connection = databaseConnection.GetNpgsqlConnection();
            CreateSqlTransaction(databaseConnection).Wait();
            this.customerRepository = customerRepository;
            this.accountRepository = accountRepository;
            this.cardRepository = cardRepository;
        }
        #region Data members
        public CustomerModel Customer { get; set; }
        public AccountModel Account { get; set; }
        public CardModel Card { get; set; }
        readonly NpgsqlConnection connection;
        NpgsqlTransaction transaction;
        #endregion

        #region Interface members
        private readonly ICustomerRepository customerRepository;
        private readonly IAccountRepository accountRepository;
        private readonly ICardRepository cardRepository;
        #endregion

        #region Methods

        private async Task CreateSqlTransaction(IDatabaseConnectionResolver databaseConnection)
        {
            //TODO - Need to revisit
            await connection.OpenAsync();
            //transaction = await connection.BeginTransactionAsync(System.Data.IsolationLevel.Serializable);
        }
        public async Task GetAsync(string cardToken)
        {
            try
            {
                Card = await cardRepository.GetCardAsync(cardToken, connection);
                Customer = await customerRepository.GetCustomer(Card.CustomerId, connection);
                Account = await accountRepository.GetAccountAsync(Card.AccountId, connection);
            }
            catch
            {
            }
        }
        public async Task Update(TransactionModel transactionModel)
        {
            await Task.Delay(1);
            //await transaction?.CommitAsync();
        }
        public void Dispose()
        {
            connection.Dispose();
            // Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}