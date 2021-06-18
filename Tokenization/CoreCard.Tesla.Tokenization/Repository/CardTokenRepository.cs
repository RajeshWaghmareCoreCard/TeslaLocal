using System;
using System.Threading.Tasks;
using CoreCard.Tesla.Common;
using CoreCard.Tesla.Tokenization.DataModels.CommonTypes;
using CoreCard.Tesla.Tokenization.DataModels.DtoTypes;
using CoreCard.Tesla.Tokenization.DataModels.Interfaces;
using CoreCard.Tesla.Tokenization.DataModels.Types;
using Npgsql;

namespace CoreCard.Tesla.Tokenization.Repository
{
    public class CardTokenRepository : ICardTokenRepository
    {
        private readonly IDatabaseConnectionResolver databaseConnection;


        public CardTokenRepository(IDatabaseConnectionResolver databaseConnection)
        {
            this.databaseConnection = databaseConnection;
        }
        public async Task InsertCardToken(CardTokenModel card)
        {
            try
            {
                using (NpgsqlConnection connection = databaseConnection.GetNpgsqlConnection())
                {
                    await connection.OpenAsync();
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = connection;
                        cmd.CommandText = TokenizationSqlCommands.InsertCardTokenCommand;
                        cmd.Parameters.AddWithValue("card_token_id", card.CardTokenId);
                        cmd.Parameters.AddWithValue("institution_id", card.InstitutionId);
                        cmd.Parameters.AddWithValue("network_name", card.NetworkName);
                        cmd.Parameters.AddWithValue("card_bin", card.CardBin);
                        cmd.Parameters.AddWithValue("card_last4", card.CardLast4);
                        cmd.Parameters.AddWithValue("card_expiration", card.CardExpiration);
                        cmd.Parameters.AddWithValue("card_hash", card.CardHash);
                        cmd.Parameters.AddWithValue("encrypted_card", card.EncryptedCardNumber);
                        cmd.Parameters.AddWithValue("active", 1);
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

        public async Task<CardTokenModel> GetCardToken(string token)
        {
            try
            {
                using (NpgsqlConnection connection = databaseConnection.GetNpgsqlConnection())
                {
                    await connection.OpenAsync();
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = connection;
                        cmd.CommandText = TokenizationSqlCommands.GetCardTokenCommand;
                        cmd.Parameters.AddWithValue("card_token_id", token);
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                CardTokenModel cardTokenDto = new CardTokenModel();
                                cardTokenDto.CardTokenId = reader.TryGetOrdinal("card_token_id").ToString();
                                cardTokenDto.InstitutionId = reader.TryGetOrdinal("institution_id").ToString();
                                cardTokenDto.CardBin = reader.TryGetOrdinal("card_bin").ToString();
                                cardTokenDto.CardLast4 = reader.TryGetOrdinal("card_last4").ToString();
                                cardTokenDto.CardHash = reader.TryGetOrdinal("card_hash").ToString();
                                cardTokenDto.CardExpiration = reader.TryGetOrdinal("card_expiration").ToString();
                                cardTokenDto.EncryptedCardNumber = reader.TryGetOrdinal("encrypted_card").ToString();
                                return cardTokenDto;
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
        public async Task<CardTokenModel> GetCardTokenByHash(string cardHash)
        {
            try
            {
                using (NpgsqlConnection connection = databaseConnection.GetNpgsqlConnection())
                {
                   await connection.OpenAsync();
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = connection;
                        cmd.Transaction= connection.BeginTransaction();
                        cmd.CommandText = TokenizationSqlCommands.GetCardTokenByCardHashCommand;
                        cmd.Parameters.AddWithValue("card_hash", cardHash);
                        
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                CardTokenModel cardTokenDto = new CardTokenModel();
                                cardTokenDto.CardTokenId = reader.TryGetOrdinal("card_token_id").ToString();
                                cardTokenDto.InstitutionId = reader.TryGetOrdinal("institution_id").ToString();
                                cardTokenDto.CardBin = reader.TryGetOrdinal("card_bin").ToString();
                                cardTokenDto.CardLast4 = reader.TryGetOrdinal("card_last4").ToString();
                                cardTokenDto.CardHash = reader.TryGetOrdinal("card_hash").ToString();
                                cardTokenDto.CardExpiration = reader.TryGetOrdinal("card_expiration").ToString();
                                cardTokenDto.EncryptedCardNumber = reader.TryGetOrdinal("encrypted_card").ToString();
                                return cardTokenDto;
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

        public Task<CardTokenModel> UpdateCardToken(CardTokenModel CardDetails)
        {
            return null;
        }
    }
}