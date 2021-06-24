using CoreCard.Tesla.Falcon.DataModels.Entity;
using CoreCard.Tesla.Falcon.DataModels.Model;
using DBAdapter;
using System;
using System.Threading.Tasks;

namespace CoreCard.Tesla.Falcon.Services
{
    public interface ICustomerBAL //: IBaseBAL<Customer>
    {
        //Task<Customer> AddCustomerAsync(CustomerAddDTO customerDTO);

        Guid Insert(Customer t, IDataBaseCommand databaseCommand);
    }
}