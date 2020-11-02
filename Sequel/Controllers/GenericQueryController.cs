using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sequel.Controllers.Base;
using Sequel.Infrastructure.Operations;
using Unity;

namespace Sequel.Controllers
{
    [ApiController]
    [Route("api/[Group]/[controller]")]
    public class GenericQueryController<T, TResult> : BaseGenericQueryController<T>
        where T : IQuery<TResult>
    {
        public GenericQueryController(IUnityContainer container)
            : base(container)
        {
        }

        public Task<ActionResult<TResult>> Query(CancellationToken cancellationToken)
        {
            return Task<ActionResult<TResult>>.Factory.StartNew(() => HandleQuery(), cancellationToken);
        }
    }

    [ApiController]
    [Route("api/[Group]/[controller]")]
    public class GenericQueryController<T, TArgs, TResult> : BaseGenericQueryController<T>
    where T : IQuery<TArgs, TResult>
    {
        public GenericQueryController(IUnityContainer container)
            : base(container)
        {
        }

        public Task<ActionResult<TResult>> Query(TArgs args, CancellationToken cancellationToken)
        {
            return Task<ActionResult<TResult>>.Factory.StartNew(() => HandleQuery(new object[] { args }), cancellationToken);
        }
    }
}
