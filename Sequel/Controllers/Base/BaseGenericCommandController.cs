using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Sequel.Infrastructure.Operations;
using Unity;

namespace Sequel.Controllers.Base
{
    public abstract class BaseGenericCommandController<T> : BaseGenericController
        where T : IBaseCommand
    {
        private readonly IUnityContainer _container;

        protected BaseGenericCommandController(IUnityContainer container)
        {
            _container = container;
        }

        protected T GetInstance()
        {
            return _container.Resolve<T>();
        }

        protected ActionResult HandleCanExecute(params object[] args)
        {
            T inst = GetInstance();
            object result = inst.GetType().InvokeMember("CanExecute", BindingFlags.InvokeMethod, null, inst, args);

            return Ok(result);
        }

        protected ActionResult HandleExecute(params object[] args)
        {
            T inst = GetInstance();
            object result = inst.GetType().InvokeMember("Execute", BindingFlags.InvokeMethod, null, inst, args);

            if (result is IOperationResult)
                return HandleOperationResult(result as IOperationResult);

            return Ok(result);
        }
    }
}
