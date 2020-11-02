using Microsoft.Extensions.Configuration;
using Sequel.Demo.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sequel.Migrations
{
    public class DemoDbContextFactory : MigrationDbContextFactory<DemoDbContext>
    {
        public override DemoDbContext CreateDbContext(string[] args)
        {
            return new DemoDbContext(Configuration.GetConnectionString("Default"));
        }
    }
}
