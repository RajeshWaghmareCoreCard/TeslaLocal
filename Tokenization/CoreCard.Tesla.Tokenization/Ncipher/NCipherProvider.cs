using System.Threading.Tasks;
using CoreCard.Tesla.Tokenization.DataModels.Interfaces;
using CoreCard.Tesla.Tokenization.DataModels.NCipher;
namespace CoreCard.Tesla.Tokenization
{
    public class NCipherProvider : INCipherProvider
    {
        private readonly IChannel ncipherChannel;

        public NCipherProvider(IChannel ncipherChannel)
        {
            this.ncipherChannel = ncipherChannel;
        }
        public async Task<EncryptResponse> EncryptAsync(EncryptRequest request)
        {
            EncryptResponse response = new EncryptResponse();
            response.EncryptedData = request.Data;
            response.Length = request.Data.Length;
            response.ResponseCode = "00";
            return await Task.FromResult(response);
        }

        public Task<DecryptResponse> DecryptAsync(DecryptRequest request)
        {
            return null;
        }
    }
}