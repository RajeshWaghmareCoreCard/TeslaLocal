using CoreCard.Tesla.Falcon.DataModels.Model;
using System.Threading.Tasks;

namespace CoreCard.Tesla.Falcon.Services

{
    public interface IPaymentBAL
    {
        //Task<BaseResponseDTO> AddPaymentAsync(PaymentAddDTO paymentaddDTO);
        Task<BaseResponseDTO> AddPaymentADOAsync(PaymentAddDTO paymentaddDTO);
    }
}
