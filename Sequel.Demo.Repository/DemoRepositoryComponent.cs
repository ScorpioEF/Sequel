using Sequel.Demo.Business.Repositories;
using Sequel.Demo.Repository.Repositories;
using Sequel.Infrastructure.Component;
using System;
using System.Collections.Generic;
using System.Text;
using Unity;

namespace Sequel.Demo.Repository
{
    public class DemoRepositoryComponent : Component
    {
        public DemoRepositoryComponent(IUnityContainer unityContainer) : base(unityContainer)
        {
        }

        public override void RegisterRepositories(IUnityContainer container)
        {
            container.RegisterType<IThingRepository, ThingRepository>();
        }
    }
}
