using CoreCard.Tesla.Falcon.DataModels.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreCard.Tesla.Falcon.ADORepository
{
    public interface IADOAPILogRepository: IADOCockroachDBRepository<APILog>
    {
        void Insert(APILog t);
    }
}
