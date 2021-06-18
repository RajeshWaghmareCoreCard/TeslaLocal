using System;
using System.Threading.Tasks;
using CoreCard.Tesla.Common;
using CoreCard.Tesla.Falcon.DataModels.Common;
using CoreCard.Tesla.Falcon.DataModels.Repository;
using CoreCard.Tesla.Falcon.DbRepository.RepoInterfaces;
using Npgsql;

namespace CoreCard.Tesla.Falcon.DbRepository
{
    public class CardRepository : BaseRepository, ICardRepository
    {
        public CardRepository(IDatabaseConnectionResolver databaseConnection) : base(databaseConnection)
        {
        }
        public Task<CardModel> GetCardAsync(string cardToken)
        {
            throw new System.NotImplementedException();
        }

        public async Task<CardModel> GetCardAsync(string cardToken, NpgsqlConnection connection)
        {
            try
            {
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = FalconSqlCommands.GetCardByToken;
                    cmd.Parameters.AddWithValue("card_token_id", cardToken);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            CardModel cardModel = new CardModel();
                            cardModel.CardId = reader.TryGetOrdinal("card_id").ToString();
                            cardModel.InstitutionId = reader.TryGetOrdinal("institution_id").ToString();
                            cardModel.CardBin = reader.TryGetOrdinal("bin").ToString();
                            cardModel.CardLast4 = reader.TryGetOrdinal("card_last4").ToString();
                            cardModel.ProductId = reader.TryGetOrdinal("product_id").ToString();
                            cardModel.AccountId = reader.TryGetOrdinal("account_id").ToString();
                            cardModel.CardStatus = reader.TryGetOrdinal("card_status").ToString();
                            cardModel.CustomerId = reader.TryGetOrdinal("customer_id").ToString();
                            cardModel.CardTokenId = reader.TryGetOrdinal("card_token_id").ToString();
                            cardModel.CardType = reader.TryGetOrdinal("card_type").ToString();
                            cardModel.ProgramManagerId = reader.TryGetOrdinal("program_manager_id").ToString();
                            cardModel.IsActivated = reader.TryGetOrdinal("activated").ToString().TryToBool();
                            // cardModel.DailyLimitAmount = reader.TryGetOrdinal("daily_limit_amount").ToString().TryToDecimal();
                            // cardModel.DailyLimitCount = reader.TryGetOrdinal("daily_limit_count").ToString().TryToInt();
                            // cardModel.DailyUsageAmount = reader.TryGetOrdinal("daily_usage_amount").ToString().TryToDecimal();
                            // cardModel.LastUsageDate = reader.TryGetOrdinal("last_usage_date").ToString().TryToDateTime();
                            cardModel.CardExpirationDate = reader.TryGetOrdinal("Card_Expiration_Date").ToString();
                            
                            return cardModel;
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