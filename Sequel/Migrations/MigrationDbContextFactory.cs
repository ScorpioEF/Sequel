using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;

namespace Sequel.Migrations
{
    public abstract class MigrationDbContextFactory<TDbContext> : IDesignTimeDbContextFactory<TDbContext> where TDbContext : DbContext
    {
        protected IConfiguration Configuration
        {
            get
            {
                ConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
                configurationBuilder.SetBasePath(AppContext.BaseDirectory)
                    .AddJsonFile("appsettings.Migration.json");
                return configurationBuilder.Build();
            }
        }

        public abstract TDbContext CreateDbContext(string[] args);
    }
}
