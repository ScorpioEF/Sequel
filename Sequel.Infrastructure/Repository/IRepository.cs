using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Sequel.Infrastructure.Repository
{
    public interface IRepository<TKey, TEntity> where TEntity : class
    {
        IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "");

        TEntity GetByID(TKey id);

        void Insert(TEntity entity);

        void Delete(TKey id);

        void Delete(TEntity entityToDelete);

        void Update(TEntity entityToUpdate);
    }
}
