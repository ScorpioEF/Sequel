using Sequel.Infrastructure.Operations;
using System;
using Unity;

namespace Sequel.Infrastructure.Component
{
    public abstract class Component : IComponent
    {
        protected readonly IUnityContainer Container;
        private readonly QueriesDefinitions _queriesDefinition;
        private readonly CommandsDefinitions _commandsDefinition;

        protected Component(IUnityContainer unityContainer)
        {
            Container = unityContainer;
            Dependencies = new Type[0];
            _queriesDefinition = Container.Resolve<QueriesDefinitions>();
            _commandsDefinition = Container.Resolve<CommandsDefinitions>();
        }

        public virtual bool ShouldBeLoaded()
        {
            return true;
        }

        public virtual Type[] Dependencies { get; }

        public virtual bool Load()
        {
            if (ShouldBeLoaded() == false)
                return false;

            RegisterRepositories(Container);
            RegisterQueries(_queriesDefinition);
            RegisterCommands(_commandsDefinition);

            return true;
        }

        public virtual void RegisterQueries(QueriesDefinitions queriesDefinition)
        {

        }

        public virtual void RegisterCommands(CommandsDefinitions commandsDefinition)
        {

        }

        public virtual void RegisterRepositories(IUnityContainer container)
        {

        }
    }
}
