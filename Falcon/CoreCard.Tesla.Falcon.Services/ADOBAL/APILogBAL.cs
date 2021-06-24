using CoreCard.Tesla.Falcon.ADORepository;
using CoreCard.Tesla.Falcon.DataModels.Entity;
//using CockroachDb.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreCard.Tesla.Falcon.Services
{
    public class APILogBAL : IAPILogBAL
    {
        //private readonly IAPILogRepository _apiLogRepository;
        private readonly IADOAPILogRepository _iADOAPILogRepository;

        public APILogBAL(IADOAPILogRepository iADOAPILogRepository) //: base(apiLogRepository)
        {
            //_apiLogRepository = apiLogRepository;
            _iADOAPILogRepository = iADOAPILogRepository;
        }

        public void Insert(APILog t)
        {
            _iADOAPILogRepository.Insert(t);
        }
    }
}
