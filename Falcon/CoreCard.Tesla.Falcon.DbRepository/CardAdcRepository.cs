using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoreCard.Tesla.Common;
using CoreCard.Tesla.Falcon.DataModels.Repository;
using CoreCard.Tesla.Falcon.DbRepository.RepoInterfaces;
using Npgsql;

namespace CoreCard.Tesla.Falcon.DbRepository
{
    public class CardAdcRepository : BaseRepository, ICardAdcRepository
    {
        public CardAdcRepository(IDatabaseConnectionResolver databaseConnection) : base(databaseConnection)
        {

        }
        public async Task<List<CardAdc>> GetAllCardAdcs(string cardId)
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
                        cmd.CommandText = FalconSqlCommands.GetAllCardAdcsByCardId;
                        cmd.Parameters.AddWithValue("card_id", cardId);

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            List<CardAdc> cardAdcs = new List<CardAdc>();
                            while (await reader.ReadAsync())
                            {
                                CardAdc cardAdc = new CardAdc();
                                cardAdc.CardAdcId = reader.TryGetOrdinal("Card_Adc_Id").ToString();
                                cardAdc.CardId = reader.TryGetOrdinal("Card_Id").ToString();
                                cardAdc.AdcId = reader.TryGetOrdinal("Adc_Id").ToString();
                                cardAdc.ResponseCode = reader.TryGetOrdinal("Response_Code").ToString();
                                cardAdc.InternalResponseCode = reader.TryGetOrdinal("Internal_Response_Code").ToString();
                                cardAdc.Active = reader.TryGetOrdinal("Active").ToString().TryToBool();
                                cardAdc.ContinueOnTimeout = reader.TryGetOrdinal("Continue_On_Timeout").ToString().TryToBool();
                                cardAdcs.Add(cardAdc);
                            }
                            return cardAdcs;
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