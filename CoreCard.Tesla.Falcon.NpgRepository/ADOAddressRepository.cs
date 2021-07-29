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
    public class ADOAddressRepository : BaseRepository, IADOAddressRepository
    {
        public ADOAddressRepository(IDatabaseConnectionResolver databaseConnection) : base(databaseConnection)
        {

        }
        public void Insert(Address t)
        {
            throw new NotImplementedException();
        }
    }
}
