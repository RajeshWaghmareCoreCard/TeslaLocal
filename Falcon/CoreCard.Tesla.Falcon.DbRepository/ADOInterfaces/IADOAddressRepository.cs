using CoreCard.Tesla.Falcon.ADORepository;
using CoreCard.Tesla.Falcon.DataModels.Entity;
using DBAdapter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreCard.Tesla.Falcon.ADORepository
{
    public interface IADOAddressRepository:IADOCockroachDBRepository<Address>
    {
        void Insert(Address t, IDataBaseCommand databaseCommand);
    }
}
