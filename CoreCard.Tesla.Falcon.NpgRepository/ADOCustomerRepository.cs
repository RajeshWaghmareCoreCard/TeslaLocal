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
    public class ADOCustomerRepository: BaseRepository,IADOCustomerRepository
    {
        public ADOCustomerRepository(IDatabaseConnectionResolver databaseConnection) : base(databaseConnection)
        {

        }

        public Guid Insert(Customer t)
        {
            throw new NotImplementedException();
        }
    }
}
