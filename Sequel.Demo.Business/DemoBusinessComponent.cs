using Sequel.Demo.Business.Commands;
using Sequel.Demo.Business.Models;
using Sequel.Demo.Business.Queries;
using Sequel.Infrastructure.Component;
using Sequel.Infrastructure.Operations;
using System;
using System.Collections.Generic;
using System.Text;
using Unity;

namespace Sequel.Demo.Business
{
    public class DemoBusinessComponent : Component
    {
        public DemoBusinessComponent(IUnityContainer unityContainer) : base(unityContainer)
        {
        }

        public override void RegisterQueries(QueriesDefinitions queriesDefinition)
        {
            queriesDefinition.Of<Thing>().Define<GetAllThingsQuery>();
        }

        public override void RegisterCommands(CommandsDefinitions commandsDefinition)
        {
            commandsDefinition.Of<Thing>().Define<CreateThingWithOtherStuffsCommand>();
        }
    }
}
