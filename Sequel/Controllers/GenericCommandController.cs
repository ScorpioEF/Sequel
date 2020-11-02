using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sequel.Controllers.Base;
using Sequel.Infrastructure.Operations;
using Unity;

namespace Sequel.Controllers
{
    [ApiController]
    public class GenericCommandController<T> : BaseGenericCommandController<T>
        where T : ICommand
    {
        public GenericCommandController(IUnityContainer container)
            : base(container)
        {
        }

        [HttpPost("api/[group]/[controller]/CanExecute")]
        public Task<ActionResult> CanExecute(CancellationToken cancellationToken)
        {
            return Task<ActionResult>.Factory.StartNew(() => HandleCanExecute(), cancellationToken);
        }

        [HttpPost("api/[group]/[controller]/Execute")]
        public Task<ActionResult> Execute(CancellationToken cancellationToken)
        {
            return Task<ActionResult>.Factory.StartNew(() => HandleExecute(), cancellationToken);
        }
    }

    [ApiController]
    public class GenericCommandController<T, TArgs> : BaseGenericCommandController<T>
        where T : ICommand<TArgs, ValidationResult>
    {
        public GenericCommandController(IUnityContainer container)
            : base(container)
        {
        }

        [HttpPost("api/[group]/[controller]/CanExecute")]
        public Task<ActionResult<ValidationResult>> CanExecute(TArgs args, CancellationToken cancellationToken)
        {
            return Task<ActionResult<ValidationResult>>.Factory.StartNew(() => HandleCanExecute(args), cancellationToken);
        }

        [HttpPost("api/[group]/[controller]/Execute")]
        public Task<ActionResult> Execute(TArgs args, CancellationToken cancellationToken)
        {
            return Task<ActionResult>.Factory.StartNew(() => HandleExecute(args), cancellationToken);
        }
    }

    [ApiController]
    public class GenericCommandController<T, TArgs, TResult> : BaseGenericCommandController<T>
     where T : ICommandWithResult<TArgs, TResult, ValidationResult>
    {
        public GenericCommandController(IUnityContainer container)
            : base(container)
        {
        }

        [HttpPost("api/[group]/[controller]/CanExecute")]
        public Task<ActionResult<ValidationResult>> CanExecute(TArgs args, CancellationToken cancellationToken)
        {
            return Task<ActionResult<ValidationResult>>.Factory.StartNew(() => HandleCanExecute(args), cancellationToken);
        }

        [HttpPost("api/[group]/[controller]/Execute")]
        public Task<ActionResult<TResult>> Execute(TArgs args, CancellationToken cancellationToken)
        {
            return Task<ActionResult<TResult>>.Factory.StartNew(() => HandleExecute(args), cancellationToken);
        }
    }
}
