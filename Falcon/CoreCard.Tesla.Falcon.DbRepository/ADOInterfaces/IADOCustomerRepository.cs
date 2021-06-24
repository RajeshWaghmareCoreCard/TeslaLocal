using CoreCard.Tesla.Falcon.DataModels.Entity;
using DBAdapter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreCard.Tesla.Falcon.ADORepository
{
    public interface IADOCustomerRepository: IADOCockroachDBRepository<Customer>
    {
        Guid Insert(Customer t, IDataBaseCommand dbcommand);
    }
}
