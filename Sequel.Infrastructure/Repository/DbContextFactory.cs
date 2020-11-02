using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;

namespace Sequel.Infrastructure.Repository
{
    public class DbContextFactory
    {
        private readonly AppConfig _appConfig;

        public DbContextFactory(AppConfig appConfig)
        {
            _appConfig = appConfig;
        }

        public TDbContext GetDbContext<TDbContext>() where TDbContext : DbContext
        {
            TDbContext dbContext = (TDbContext)Activator.CreateInstance(typeof(TDbContext), _appConfig.DefaultConnectionString);

            if (!dbContext.Database.GetPendingMigrations().Any())
                dbContext.Database.Migrate();

            return dbContext;
        }
    }
}
