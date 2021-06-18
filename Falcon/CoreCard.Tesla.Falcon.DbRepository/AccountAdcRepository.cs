using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoreCard.Tesla.Common;
using CoreCard.Tesla.Falcon.DataModels.Repository;
using CoreCard.Tesla.Falcon.DbRepository.RepoInterfaces;
using Npgsql;

namespace CoreCard.Tesla.Falcon.DbRepository
{
    public class AccountAdcRepository : BaseRepository, IAccountAdcRepository
    {
        public AccountAdcRepository(IDatabaseConnectionResolver databaseConnection) : base(databaseConnection)
        {

        }
        
        public async Task<List<AccountAdc>> GetAllAccountAdcs(string accountId)
        {
            try
            {
                using (NpgsqlConnection connection = databaseConnection.GetNpgsqlConnection())
                {
                    await connection.OpenAsync();
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = connection;
                        cmd.Transaction = connection.BeginTransaction();
                        cmd.CommandText = FalconSqlCommands.GetAllAccountAdcsByAccountId;
                        cmd.Parameters.AddWithValue("Account_Id", accountId);
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            List<AccountAdc> accountAdcs = new List<AccountAdc>();
                            while (await reader.ReadAsync())
                            {
                                AccountAdc accountAdc = new AccountAdc();
                                accountAdc.AccountAdcId = reader.TryGetOrdinal("Account_Adc_Id").ToString();
                                accountAdc.AccountId = reader.TryGetOrdinal("Account_Id").ToString();
                                accountAdc.AdcId = reader.TryGetOrdinal("Adc_Id").ToString();
                                accountAdc.ResponseCode = reader.TryGetOrdinal("Response_Code").ToString();
                                accountAdc.InternalResponseCode = reader.TryGetOrdinal("Internal_Response_Code").ToString();
                                accountAdc.Active = reader.TryGetOrdinal("Active").ToString().TryToBool();
                                accountAdc.ContinueOnTimeout = reader.TryGetOrdinal("Continue_On_Timeout").ToString().TryToBool();
                                accountAdcs.Add(accountAdc);
                            }
                            return accountAdcs;
                        }
                        throw new TeslaException(FalconResponseCodes.InvalidAccountAdc, FalconResponseMessages.InvalidAccountAdc);
                    }
                }
            }
            catch (TeslaException ex)
            {
                var message = ex.Message;
                throw;
            }
            catch (NpgsqlException ex)
            {
                var message = ex.Message;
                throw new TeslaException(TokenizationRsponseCodes.DataBaseConnectivityIssue, TokenizationResponseMessages.DatabaseConnection);
            }
            catch (Exception ex)
            {
                //Log generic message
                var message = ex.Message;
                throw new TeslaException(FalconResponseCodes.InvalidAccountAdc, FalconResponseMessages.InvalidAccountAdc);
            }
        }
    }
}