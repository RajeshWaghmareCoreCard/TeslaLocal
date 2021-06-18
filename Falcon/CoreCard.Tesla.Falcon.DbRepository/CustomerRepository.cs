using System.Threading.Tasks;
using CoreCard.Tesla.Common;
using CoreCard.Tesla.Falcon.DataModels.Repository;
using CoreCard.Tesla.Falcon.DbRepository.RepoInterfaces;
using Npgsql;

namespace CoreCard.Tesla.Falcon.DbRepository
{
    public class CustomerRepository : BaseRepository, ICustomerRepository
    {
        public CustomerRepository(IDatabaseConnectionResolver databaseConnection) : base(databaseConnection)
        {
        }

        public Task<CustomerModel> GetCustomer(string customerId)
        {
            throw new System.NotImplementedException();
        }

        public async Task<CustomerModel> GetCustomer(string customerId, NpgsqlConnection connection)
        {
            try
            {
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = FalconSqlCommands.GetCustomerById;
                    cmd.Parameters.AddWithValue("customer_id", customerId);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            CustomerModel customerModel = new CustomerModel();
                            customerModel.CustomerId = reader.TryGetOrdinal("Customer_Id").ToString();
                            customerModel.InstitutionId = reader.TryGetOrdinal("Institution_Id").ToString();
                            customerModel.CustomerName = reader.TryGetOrdinal("Customer_Name").ToString();
                            customerModel.CustomerAddress = reader.TryGetOrdinal("Customer_Address").ToString();
                            customerModel.SSN = reader.TryGetOrdinal("SSN").ToString();
                            customerModel.IsActive = reader.TryGetOrdinal("Active").ToString().TryToBool();
                            return customerModel;
                        }
                    }
                    throw new TeslaException(TokenizationRsponseCodes.TokenNotFound, TokenizationResponseMessages.TokenNotFound);
                }
            }
            catch
            {
                throw;
            }
        }
    }
}