using Sequel.Demo.Business.Models;
using Sequel.Demo.Business.Repositories;
using Sequel.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sequel.Demo.Repository.Repositories
{
    public class ThingRepository : BaseRepository<Guid, Thing, DemoDbContext>, IThingRepository
    {
        public ThingRepository(IUnitOfWork uow) : base(uow)
        {
        }
    }
}
