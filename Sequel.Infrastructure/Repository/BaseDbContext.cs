using Microsoft.EntityFrameworkCore;
using System;

namespace Sequel.Infrastructure.Repository
{
    public abstract class BaseDbContext : DbContext
    {
        private readonly Type[] _mappingTypes;
        private readonly string _connectionString;

        protected BaseDbContext(string connectionString, params Type[] mappingTypes)
        {
            _mappingTypes = mappingTypes;
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseLazyLoadingProxies()
                .UseNpgsql(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            if (_mappingTypes == null)
                return;

            foreach (Type mappingType in _mappingTypes)
            {
                dynamic m = Activator.CreateInstance(mappingType);
                modelBuilder.ApplyConfiguration(m);
            }
        }
    }
}
