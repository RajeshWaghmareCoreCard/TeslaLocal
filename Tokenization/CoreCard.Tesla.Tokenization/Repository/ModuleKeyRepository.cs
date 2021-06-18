using System;
using System.Threading.Tasks;
using CoreCard.Tesla.Common;
using CoreCard.Tesla.Tokenization.DataModels.CommonTypes;
using CoreCard.Tesla.Tokenization.DataModels.DtoTypes;
using CoreCard.Tesla.Tokenization.DataModels.Interfaces;
using Npgsql;

namespace CoreCard.Tesla.Tokenization.Repository
{
    public class ModuleKeyRepository : IModuleKeyRepository
    {
        private readonly IDatabaseConnectionResolver databaseConnection;

        public ModuleKeyRepository(IDatabaseConnectionResolver databaseConnection)
        {
            this.databaseConnection = databaseConnection;
        }
        public IServiceProvider ServiceProvider { get; }

        public async Task<ModuleKey> GetModuleDetailsAsync(string moduleKeyId)
        {
            try
            {
                using (NpgsqlConnection connection = databaseConnection.GetNpgsqlConnection())
                {
                   await connection.OpenAsync();
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = connection;
                        cmd.CommandText = TokenizationSqlCommands.GetModuleKeyCommand;
                        cmd.Parameters.AddWithValue("Module_Key_id", moduleKeyId);
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                ModuleKey moduleKey = new ModuleKey();
                                moduleKey.ModuleId = reader.TryGetOrdinal("Module_Id").ToString();
                                moduleKey.ModuleKeyId = reader.TryGetOrdinal("Module_Key_Id").ToString();
                                moduleKey.PublicKey = reader.TryGetOrdinal("Public_Key").ToString();
                                moduleKey.Active=  reader.TryGetOrdinal("Active").ToString().TryToBool();
                                return moduleKey.ToModel();
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
    }
}