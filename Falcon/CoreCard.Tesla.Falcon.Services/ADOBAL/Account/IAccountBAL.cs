using CoreCard.Tesla.Falcon.DataModels.Entity;
using CoreCard.Tesla.Falcon.DataModels.Model;
using DBAdapter;
using System;
using System.Threading.Tasks;

namespace CoreCard.Tesla.Falcon.Services
{
    public interface IAccountBAL 
    {
        Task<BaseResponseDTO> AddAccountAsync(CustomerAddDTO customerDTO);

        //Task<BaseResponseDTO> UpdateAccountAsync(Account account);
        //Task<Account> GetAccountByNumber(UInt64 AccountNumber);

        //Task<BaseResponseDTO> GetAccountNoAsync();
        Account GetAccountByID_ADO(Guid id);
        Account GetAccountByNumber_ADO(UInt64 AccountNumber);
        Account UpdatePurchase(Account t);
        Account UpdatePurchase(Account t, DBAdapter.IDataBaseCommand dbCommand);
        void UpdateAccountWithPayment(Account t, DBAdapter.IDataBaseCommand dbCommand);

        Account GetAccountByID_ADO(Guid id, string ccregion, DBAdapter.IDataBaseCommand dbCommand);
        Account GetAccountByNumber_ADO(UInt64 AccountNumber, string ccregion, DBAdapter.IDataBaseCommand dbCommand);
    }
}