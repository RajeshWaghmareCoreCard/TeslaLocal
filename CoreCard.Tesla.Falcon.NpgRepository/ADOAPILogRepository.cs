using CoreCard.Tesla.Falcon.DataModels.Entity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CoreCard.Tesla.Falcon.NpgRepository
{
    public class ADOAPILogRepository : BaseRepository, IADOAPILogRepository
    {
        public ADOAPILogRepository(IDatabaseConnectionResolver databaseConnection) : base(databaseConnection)
        {

        }

        public void Insert(APILog t)
        {
            throw new NotImplementedException();
        }
    }
}
