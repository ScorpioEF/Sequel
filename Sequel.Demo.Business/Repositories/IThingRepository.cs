using Sequel.Demo.Business.Models;
using Sequel.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sequel.Demo.Business.Repositories
{
    public interface IThingRepository : IRepository<Guid, Thing>
    {
    }
}
