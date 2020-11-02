using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using Sequel.Controllers;
using Sequel.Infrastructure.Operations;
using System;
using System.Collections.Generic;
using System.Reflection;
using Unity;

namespace Sequel.Middlewares
{
    public class GenericCommandControllerFeatureProvider : IApplicationFeatureProvider<ControllerFeature>
    {
        private readonly IUnityContainer _container;

        public GenericCommandControllerFeatureProvider(IUnityContainer container)
        {
            _container = container;
        }

        public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature)
        {
            CommandsDefinitions commands = _container.Resolve<CommandsDefinitions>();

            foreach (Type definitionOf in commands.Keys)
            {
                Array.ForEach(commands[definitionOf].Items, t => feature.Controllers.Add(MakeCommand(t)));
            }
        }

        private TypeInfo MakeCommand(Type commandType)
        {
            MethodInfo commandMethod = commandType.GetMethod("Execute");

            if (commandMethod == null)
                throw new ArgumentException("No execute method found in type.", "commandType");

            ParameterInfo[] parameters = commandMethod.GetParameters();
            Type returnType = commandMethod.ReturnType;
            if (parameters.Length == 0 && returnType == typeof(void))
            {
                return typeof(GenericCommandController<>).MakeGenericType(new Type[] { commandType }).GetTypeInfo();
            }
            else if (parameters.Length == 1 && returnType == typeof(void))
            {
                return typeof(GenericCommandController<,>).MakeGenericType(new Type[] { commandType, parameters[0].ParameterType }).GetTypeInfo();
            }
            else if (parameters.Length == 1 && returnType != typeof(void))
            {
                return typeof(GenericCommandController<,,>).MakeGenericType(new Type[] { commandType, parameters[0].ParameterType, returnType }).GetTypeInfo();
            }

            throw new ArgumentException("Invalid command type.", "commandType");
        }
    }
}
