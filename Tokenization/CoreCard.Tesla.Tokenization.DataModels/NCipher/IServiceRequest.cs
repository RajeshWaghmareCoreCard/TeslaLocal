namespace CoreCard.Tesla.Tokenization.DataModels.Interfaces
{
    public interface IServiceRequest
    {
         byte[] GetRequestBytes();
         string RequestType {get;set;}
         string RequestId {get;set;}
    }
}