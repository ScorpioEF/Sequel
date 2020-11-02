using Sequel.Demo.Business.Models;
using Sequel.Demo.Business.Repositories;
using Sequel.Infrastructure.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sequel.Demo.Business.Queries
{
    public class GetAllThingsQuery : BaseQuery<Thing[]>
    {
        private readonly IThingRepository _thingRepository;

        public GetAllThingsQuery(IThingRepository thingRepository)
        {
            _thingRepository = thingRepository;
        }

        public override OperationResult<Thing[]> Query()
        {
            return new OperationResult<Thing[]>(OperationStatus.Success, _thingRepository.Get().ToArray());
        }
    }
}
