namespace CoreCard.Tesla.Tokenization.DataModels.Interfaces
{
    public interface IServiceResponse
    {
        byte[] GetResponseBytes();
        string ResponseType { get; set; }
        string RequestId { get; set; }
        string ResponseCode { get; set; }
        string ResponseMessage { get; set; }
    }
}