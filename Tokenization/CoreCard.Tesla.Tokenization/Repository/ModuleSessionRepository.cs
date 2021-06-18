using System;
using System.Threading.Tasks;
using CoreCard.Tesla.Tokenization.DataModels.Interfaces;
using CoreCard.Tesla.Tokenization.DataModels.Types;
using Npgsql;
using CoreCard.Tesla.Common;
using CoreCard.Tesla.Tokenization.DataModels;
using CoreCard.Tesla.Tokenization.DataModels.DtoTypes;
using CoreCard.Tesla.Tokenization.DataModels.CommonTypes;

namespace CoreCard.Tesla.Tokenization.Repository
{
    public class ModuleSessionRepository : IModuleSessionRepository
    {
       private readonly IDatabaseConnectionResolver databaseConnection;

        public ModuleSessionRepository(IDatabaseConnectionResolver databaseConnection)
        {
            this.databaseConnection = databaseConnection;
        }

        public IServiceProvider ServiceProvider { get; }

        public async Task<ModuleSessionModel> GetModuleSessionAsync(string sessionId)
        {

            try
            {
                 using (NpgsqlConnection connection = databaseConnection.GetNpgsqlConnection())
                {
                    await connection.OpenAsync();

                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = connection;
                        cmd.CommandText = TokenizationSqlCommands.GetModuleSessionCommand;
                        cmd.Parameters.AddWithValue("session_id", sessionId);
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                ModuleSessionModel sessionDetails = new ModuleSessionModel();
                                sessionDetails.ModuleId = reader.TryGetOrdinal("Module_Id").ToString();
                                sessionDetails.KeyDetails = reader.TryGetOrdinal("Key_Details").ToString().TryFromJson<KeyDetails>();
                                sessionDetails.SessionId = reader.TryGetOrdinal("Session_Id").ToString();
                                sessionDetails.SessionExpiryDate = reader.TryGetOrdinal("Session_Expiry_Date").ToString().TryToDateTime();
                                if (sessionDetails.SessionExpiryDate < DateTime.Now)
                                {
                                    throw new TeslaException(TokenizationRsponseCodes.SessionNotFound, TokenizationResponseMessages.SessionNotFound);
                                }
                                return sessionDetails;
                            }
                        }
                        throw new TeslaException(TokenizationRsponseCodes.SessionNotFound, TokenizationResponseMessages.SessionNotFound);
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
                throw new TeslaException(TokenizationRsponseCodes.SessionNotFound, TokenizationResponseMessages.SessionNotFound);
            }
        }

        public async Task InsertModuleSessionAsync(ModuleSessionModel sessionDetails)
        {
            try
            {
                using (NpgsqlConnection connection = databaseConnection.GetNpgsqlConnection())
                {
                    await connection.OpenAsync();
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = connection;
                        cmd.CommandText = TokenizationSqlCommands.InsertModuleSessionCommand;
                        cmd.Parameters.AddWithValue("module_id", sessionDetails.ModuleId);
                        cmd.Parameters.AddWithValue("session_id", sessionDetails.SessionId);
                        cmd.Parameters.AddWithValue("Key_Details", sessionDetails.KeyDetails.TryToJson());
                        cmd.Parameters.AddWithValue("created_at", DateTime.Now);
                        cmd.Parameters.AddWithValue("created_by", AuditUsers.CodeUser);
                        cmd.Parameters.AddWithValue("updated_by", AuditUsers.CodeUser);
                        cmd.Parameters.AddWithValue("updated_date", DateTime.Now);
                        cmd.Parameters.AddWithValue("updated_date", DateTime.Now);
                        cmd.Parameters.AddWithValue("session_expiry_date", sessionDetails.SessionExpiryDate);
                        var rowAffected = await cmd.ExecuteNonQueryAsync();
                        if (rowAffected <= 0)
                        {
                            throw new TeslaException(TokenizationRsponseCodes.SessionNotFound, TokenizationResponseMessages.SessionNotFound);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                throw new TeslaException(TokenizationRsponseCodes.SessionNotFound, TokenizationResponseMessages.SessionNotFound);
            }
        }
    }
}