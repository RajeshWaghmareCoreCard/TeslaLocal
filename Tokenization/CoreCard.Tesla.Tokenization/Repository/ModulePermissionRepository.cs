using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoreCard.Tesla.Common;
using CoreCard.Tesla.Tokenization.DataModels.CommonTypes;
using CoreCard.Tesla.Tokenization.DataModels.DtoModels;
using CoreCard.Tesla.Tokenization.DataModels.Interfaces;
using Npgsql;

namespace CoreCard.Tesla.Tokenization.Repository
{
    public class ModulePermissionRepository : IModulePermissionRepository
    {
        private readonly IDatabaseConnectionResolver databaseConnection;


        public ModulePermissionRepository(IDatabaseConnectionResolver databaseConnection)
        {
            this.databaseConnection = databaseConnection;
        }
        public Task<List<ModulePermissionModel>> GetAllModulePermissions(string moduleId)
        {
            throw new System.NotImplementedException();
        }

        public async Task<ModulePermissionModel> GetModulePermissions(string moduleId, string tokenFamily)
        {
            try
            {
                using (NpgsqlConnection connection = databaseConnection.GetNpgsqlConnection())
                {
                    await connection.OpenAsync();
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = connection;
                        cmd.CommandText = TokenizationSqlCommands.GetModulePermissionCommand;
                        cmd.Parameters.AddWithValue("module_id", moduleId);
                        cmd.Parameters.AddWithValue("token_family_id", tokenFamily);
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                ModulePermissionModel modulePermissionModel = new ModulePermissionModel();
                                modulePermissionModel.ModulePermissionId = reader.TryGetOrdinal("Module_Permission_Id").ToString();
                                modulePermissionModel.IsDetokenizationAllowed = reader.TryGetOrdinal("Detokenization_Allowed").ToString().TryToBool();
                                modulePermissionModel.IsTokenizationAllowed = reader.TryGetOrdinal("Tokenization_Allowed").ToString().TryToBool();
                                modulePermissionModel.ModuleId = reader.TryGetOrdinal("ModuleId").ToString();
                                modulePermissionModel.NotifyDetokenizationOperation = reader.TryGetOrdinal("Detokenization_Notify").ToString().TryToBool();
                                modulePermissionModel.NotifyTokenizationOperation = reader.TryGetOrdinal("tokenization_Notify").ToString().TryToBool();
                                modulePermissionModel.TokenFamilyId = reader.TryGetOrdinal("Token_Family_Id").ToString();
                                return modulePermissionModel;
                            }
                        }
                        throw new TeslaException(TokenizationRsponseCodes.TokenNotFound, TokenizationResponseMessages.TokenNotFound);
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
                throw new TeslaException(TokenizationRsponseCodes.TokenNotFound, TokenizationResponseMessages.TokenNotFound);
            }
        }
    }
}