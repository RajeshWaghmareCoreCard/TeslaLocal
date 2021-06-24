using CoreCard.Tesla.Falcon.DataModels.Entity;
using CoreCard.Tesla.Falcon.DataModels.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreCard.Tesla.Falcon.Services.Purchase
{
    public interface IPurchaseBAL
    {
        Task<BaseResponseDTO> AddTransactionAsync(TransactionAddDTO customerDTO);
    }
}
