using System;
using System.Threading.Tasks;
using CoreCard.Tesla.Common;
using CoreCard.Tesla.Falcon.DataModels.Repository;
using CoreCard.Tesla.Falcon.DbRepository.RepoInterfaces;
using Npgsql;

namespace CoreCard.Tesla.Falcon.DbRepository
{
    public class AccountRepository : BaseRepository, IAccountRepository
    {
        public AccountRepository(IDatabaseConnectionResolver databaseConnection) : base(databaseConnection)
        {
        }

        public Task<AccountModel> GetAccountAsync(string accountId)
        {
            throw new System.NotImplementedException();
        }

        public async Task<AccountModel> GetAccountAsync(string accountId, NpgsqlConnection connection)
        {
            try
            {
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = FalconSqlCommands.GetAccountById;
                    cmd.Parameters.AddWithValue("account_id", accountId);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            AccountModel accountModel = new AccountModel();
                            accountModel.AccountId = reader.TryGetOrdinal("Account_Id").ToString();
                            accountModel.AccountNumber = reader.TryGetOrdinal("Account_Number").ToString();
                            accountModel.CurrentBalance = reader.TryGetOrdinal("Current_Balance").ToString().TryToDecimal();
                            accountModel.CreditLimit = reader.TryGetOrdinal("Credit_Limit").ToString().TryToDecimal();
                            accountModel.ProductId = reader.TryGetOrdinal("Product_Id").ToString();
                            accountModel.CustomerId = reader.TryGetOrdinal("Customer_Id").ToString();
                            accountModel.DailyLimitAmount = reader.TryGetOrdinal("Daily_Limit_Amount").ToString().TryToDecimal();
                            accountModel.AccountStatus = reader.TryGetOrdinal("Account_Status").ToString();
                            accountModel.CashBalance = reader.TryGetOrdinal("Cash_Balance").ToString().TryToDecimal();
                            accountModel.CashCreditLimit = reader.TryGetOrdinal("Cash_Credit_Limit").ToString().TryToDecimal();
                            accountModel.CurrentDailyLimitAmount = reader.TryGetOrdinal("Current_Daily_Limit_Amount").ToString().TryToDecimal();
                            accountModel.CurrentDailyLimitCount = reader.TryGetOrdinal("Current_Daily_Limit_Count").ToString().TryToInt();
                            accountModel.DailyLimitCount = reader.TryGetOrdinal("Daily_Limit_Count").ToString().TryToInt();
                            accountModel.DailyLimitDate = reader.TryGetOrdinal("Daily_Limit_Date").ToString().TryToDateTime();
                            return accountModel;
                        }
                    }
                    throw new TeslaException(TokenizationRsponseCodes.TokenNotFound, TokenizationResponseMessages.TokenNotFound);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}