using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Sequel.Infrastructure.Operations;
using Unity;

namespace Sequel.Controllers.Base
{
    public abstract class BaseGenericQueryController<T> : BaseGenericController
        where T : IQuery
    {
        private readonly IUnityContainer _container;

        protected BaseGenericQueryController(IUnityContainer container)
        {
            _container = container;
        }

        protected T GetInstance()
        {
            return _container.Resolve<T>();
        }

        protected ActionResult HandleQuery(params object[] args)
        {
            T inst = GetInstance();
            dynamic data = inst.GetType().InvokeMember("Query", BindingFlags.InvokeMethod, null, inst, args);

            if (data is IOperationResult)
                return HandleOperationResult(data as IOperationResult);

            return Ok(data);
        }
    }
}
