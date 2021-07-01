using DBAdapter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreCard.Tesla.Falcon.ADORepository
{
    public interface IBaseCockroachADO
    {
        Tuple<IDataBaseCommand,object> BeginTransaction();
        void CommitTransaction(IDataBaseCommand dbcommand, object tran);
        void RollbackTransaction(IDataBaseCommand dbcommand, object tran);
        Task<Tuple<IDataBaseCommand, object>> BeginTransactionAsync();
        Task CommitTransactionAsync(IDataBaseCommand dbcommand, object tran);
        Task RollbackTransactionAsync(IDataBaseCommand dbcommand, object tran);
    }
}
