using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Sequel.Controllers;
using Sequel.Infrastructure.Operations;
using System;
using Unity;

namespace Sequel.Middlewares
{
    public class GenericRouteConvention : IControllerModelConvention
    {
        private readonly QueriesDefinitions _queriesDefinitions;
        private readonly CommandsDefinitions _commandsDefinitions;

        public GenericRouteConvention(IUnityContainer container)
        {
            _queriesDefinitions = container.Resolve<QueriesDefinitions>();
            _commandsDefinitions = container.Resolve<CommandsDefinitions>();
        }

        public void Apply(ControllerModel controller)
        {
            if (controller.ControllerType.GetGenericTypeDefinition() == typeof(GenericCommandController<>)
                || controller.ControllerType.GetGenericTypeDefinition() == typeof(GenericCommandController<,>)
                || controller.ControllerType.GetGenericTypeDefinition() == typeof(GenericCommandController<,,>))
                ApplyForCommands(controller);
            else if (controller.ControllerType.GetGenericTypeDefinition() == typeof(GenericQueryController<,>)
                || controller.ControllerType.GetGenericTypeDefinition() == typeof(GenericQueryController<,,>))
                ApplyForQueries(controller);
        }

        private void ApplyForQueries(ControllerModel controller)
        {
            Type queryType = controller.ControllerType.GenericTypeArguments[0];
            Type ofType = _queriesDefinitions.Find(queryType);

            controller.ControllerName = queryType.Name;
            controller.RouteValues["Group"] = ofType.Name;
            controller.RouteValues["Controller"] = queryType.Name;
        }

        private void ApplyForCommands(ControllerModel controller)
        {
            Type commandType = controller.ControllerType.GenericTypeArguments[0];
            Type ofType = _commandsDefinitions.Find(commandType);

            controller.ControllerName = commandType.Name;
            controller.RouteValues["Group"] = ofType.Name;
            controller.RouteValues["Controller"] = commandType.Name;
        }
    }
}
