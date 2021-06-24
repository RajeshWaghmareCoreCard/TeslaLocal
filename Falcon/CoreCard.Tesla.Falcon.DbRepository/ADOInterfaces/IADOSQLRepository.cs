using CoreCard.Tesla.Falcon.DataModels.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CoreCard.Tesla.Falcon.ADORepository
{
    public interface IADOSQLRepository<TEntity> where TEntity : BaseEntity//, IADOBaseRepository<BaseEntity>
    {
        List<TEntity> GetEntityList(string sql, object[] parameters);
        Task<List<TEntity>> GetEntityListAsync(string sql, object[] parameters, CancellationToken token = default(CancellationToken));
        List<TEntity> GetAll();
        TEntity Get(Guid id);
        Task<TEntity> GetAsync(Guid id, CancellationToken token = default(CancellationToken));

        TEntity GetEntity(string sql, object[] parameters = null);
        Task<TEntity> GetEntityAsync(string sql, object[] parameters = null, CancellationToken token = default(CancellationToken));

        TEntity Find(Expression<Func<TEntity, bool>> match);


        TEntity Add(TEntity t);
        Task<TEntity> AddAsync(TEntity t, CancellationToken token = default(CancellationToken));

        TEntity Update(TEntity t, object key);
        Task<TEntity> UpdateAsync(TEntity t, object key, CancellationToken token = default(CancellationToken));

        void Delete(TEntity entity);

        Task DeleteAsync(Guid id, CancellationToken token = default(CancellationToken));

        void Save();
        Task<int> SaveAsync(CancellationToken token = default(CancellationToken));

        void RejectChanges();
    }
}
