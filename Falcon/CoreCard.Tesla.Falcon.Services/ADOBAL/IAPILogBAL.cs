using CoreCard.Tesla.Falcon.DataModels.Entity;

namespace CoreCard.Tesla.Falcon.Services
{
    public interface IAPILogBAL//:IBaseBAL<APILog>
    {
        void Insert(APILog t);
    }
}