using System;
using System.Threading.Tasks;
using CoreCard.Tesla.Common;
using CoreCard.Tesla.Tokenization.DataModels.CommonTypes;
using CoreCard.Tesla.Tokenization.DataModels.DtoModels;
using CoreCard.Tesla.Tokenization.DataModels.Repository;
using Npgsql;

namespace CoreCard.Tesla.Tokenization.Repository
{
    public class OtherTokenFamilyRepository : IOtherTokenFamilyRepository
    {
        private readonly IDatabaseConnectionResolver databaseConnection;

        public OtherTokenFamilyRepository(IDatabaseConnectionResolver databaseConnection)
        {
            this.databaseConnection = databaseConnection;
        }
        public async Task<OtherFamilyModel> GetToken(string token)
        {
            try
            {
                using (NpgsqlConnection connection = databaseConnection.GetNpgsqlConnection())
                {
                    await connection.OpenAsync();
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = connection;
                        cmd.CommandText = TokenizationSqlCommands.GetOtherTokenCommand;
                        cmd.Parameters.AddWithValue("Other_Token_Id", token);
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                OtherFamilyModel otherFamilyModel = new OtherFamilyModel();
                                otherFamilyModel.OtherTokenId = reader.TryGetOrdinal("card_token_id").ToString();
                                otherFamilyModel.InstitutionId = reader.TryGetOrdinal("institution_id").ToString();
                                otherFamilyModel.EncryptedData = reader.TryGetOrdinal("Encrypted_Data").ToString();
                                otherFamilyModel.TokenFamilyId = reader.TryGetOrdinal("Token_Family_Id").ToString();
                                otherFamilyModel.TokenHash = reader.TryGetOrdinal("Token_Hash").ToString();
                                otherFamilyModel.Active = reader.TryGetOrdinal("Active").ToString().TryToBool();
                                return otherFamilyModel;
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

        public async Task<OtherFamilyModel> GetTokenByHash(string tokenHash)
        {
            try
            {
                using (NpgsqlConnection connection = databaseConnection.GetNpgsqlConnection())
                {
                    await connection.OpenAsync();
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = connection;
                        cmd.CommandText = TokenizationSqlCommands.GetOtherTokenByHashCommand;
                        cmd.Parameters.AddWithValue("Token_Hash", tokenHash);
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                OtherFamilyModel otherFamilyModel = new OtherFamilyModel();
                                otherFamilyModel.OtherTokenId = reader.TryGetOrdinal("Other_Token_Id").ToString();
                                otherFamilyModel.InstitutionId = reader.TryGetOrdinal("institution_id").ToString();
                                otherFamilyModel.EncryptedData = reader.TryGetOrdinal("Encrypted_Data").ToString();
                                otherFamilyModel.TokenFamilyId = reader.TryGetOrdinal("Token_Family_Id").ToString();
                                otherFamilyModel.TokenHash = reader.TryGetOrdinal("Token_Hash").ToString();
                                otherFamilyModel.Active = reader.TryGetOrdinal("Active").ToString().TryToBool();
                                return otherFamilyModel;
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

        public async Task InsertToken(OtherFamilyModel otherFamilyModel)
        {
            try
            {
                using (NpgsqlConnection connection = databaseConnection.GetNpgsqlConnection())
                {
                    await connection.OpenAsync();
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = connection;
                        cmd.CommandText = TokenizationSqlCommands.InsertOtherTokenCommand;
                        cmd.Parameters.AddWithValue("Other_Token_Id", otherFamilyModel.OtherTokenId);
                        cmd.Parameters.AddWithValue("Institution_Id", otherFamilyModel.InstitutionId);
                        cmd.Parameters.AddWithValue("Active", otherFamilyModel.Active.TryToBit());
                        cmd.Parameters.AddWithValue("Encrypted_Data", otherFamilyModel.EncryptedData);
                        cmd.Parameters.AddWithValue("Token_Family_Id", otherFamilyModel.TokenFamilyId);
                        cmd.Parameters.AddWithValue("Token_Hash", otherFamilyModel.TokenHash);
                        cmd.Parameters.AddWithValue("Hsm_Active_Key_Id", otherFamilyModel.HsmActiveKeyId);
                        var rowAffected = await cmd.ExecuteNonQueryAsync();
                        if (rowAffected <= 0)
                        {
                            throw new TeslaException(TokenizationRsponseCodes.TokenNotFound, TokenizationResponseMessages.TokenNotFound);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                throw new TeslaException(TokenizationRsponseCodes.TokenNotFound, TokenizationResponseMessages.TokenNotFound);
            }
        }
    }
}