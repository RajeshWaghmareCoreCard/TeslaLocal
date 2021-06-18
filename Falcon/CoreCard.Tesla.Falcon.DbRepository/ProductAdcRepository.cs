using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoreCard.Tesla.Common;
using CoreCard.Tesla.Falcon.DataModels.Repository;
using CoreCard.Tesla.Falcon.DbRepository.RepoInterfaces;
using Npgsql;

namespace CoreCard.Tesla.Falcon.DbRepository
{
    public class ProductAdcRepository : BaseRepository, IProductAdcRepository
    {
        public ProductAdcRepository(IDatabaseConnectionResolver databaseConnection) : base(databaseConnection)
        {

        }
        public async Task<List<ProductAdc>> GetAllProductAdcs()
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
                        cmd.CommandText = FalconSqlCommands.GetAllProductAdcs;

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            List<ProductAdc> productAdcs = new List<ProductAdc>();
                            while (await reader.ReadAsync())
                            {
                                ProductAdc productAdc = new ProductAdc();
                                productAdc.ProductAdcId = reader.TryGetOrdinal("Product_Adc_Id").ToString();
                                productAdc.ProductId = reader.TryGetOrdinal("Product_Id").ToString();
                                productAdc.AdcId = reader.TryGetOrdinal("Adc_Id").ToString();
                                productAdc.ResponseCode = reader.TryGetOrdinal("Response_Code").ToString();
                                productAdc.InternalResponseCode = reader.TryGetOrdinal("Internal_Response_Code").ToString();
                                productAdc.Active = reader.TryGetOrdinal("Active").ToString().TryToBool();
                                productAdc.ContinueOnTimeout = reader.TryGetOrdinal("Continue_On_Timeout").ToString().TryToBool();
                                productAdcs.Add(productAdc);
                            }
                            return productAdcs;
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

        public async Task<ProductAdc> GetAllProductAdcs(string accountId)
        {
            throw new System.NotImplementedException();

        }
    }
}