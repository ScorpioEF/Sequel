using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace Sequel.Infrastructure.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        T GetComponent<T>() where T : DbContext;

        void SaveChanges();
    }

    public sealed class UnitOfWork : IUnitOfWork
    {
        private readonly List<DbContext> _dbContexts = new List<DbContext>();
        private readonly DbContextFactory _dbContextFactory;

        public UnitOfWork(DbContextFactory dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public void Dispose()
        {
            foreach (DbContext dbContext in _dbContexts)
                dbContext.Dispose();
        }

        public T GetComponent<T>() where T : DbContext
        {
            if (_dbContexts.Any(c => c.GetType() == typeof(T)))
                return (T)_dbContexts.First(c => c.GetType() == typeof(T));

            T resolve = _dbContextFactory.GetDbContext<T>();

            _dbContexts.Add(resolve);

            resolve.ChangeTracker.AutoDetectChangesEnabled = false;

            return resolve;
        }

        public void SaveChanges()
        {
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, TimeSpan.FromMinutes(30)))
            {
                foreach (var context in _dbContexts)
                {
                    context.ChangeTracker.AutoDetectChangesEnabled = true;
                    context.SaveChanges();
                    context.ChangeTracker.AutoDetectChangesEnabled = false;
                }

                ts.Complete();
            }
        }
    }
}
