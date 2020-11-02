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
    public class GenericQueryControllerFeatureProvider : IApplicationFeatureProvider<ControllerFeature>
    {
        private readonly IUnityContainer _container;

        public GenericQueryControllerFeatureProvider(IUnityContainer container)
        {
            _container = container;
        }

        public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature)
        {
            QueriesDefinitions queries = _container.Resolve<QueriesDefinitions>();

            foreach (Type definitionOf in queries.Keys)
            {
                Array.ForEach(queries[definitionOf].Items, t => feature.Controllers.Add(MakeQuery(t)));
            }
        }

        private TypeInfo MakeQuery(Type queryType)
        {
            MethodInfo queryMethod = queryType.GetMethod("Query");

            if (queryMethod == null)
                throw new ArgumentException("No query method found in type.", "queryType");

            ParameterInfo[] parameters = queryType.GetMethod("Query").GetParameters();
            Type returnType = queryType.GetMethod("Query").ReturnType;
            if (parameters.Length == 0)
            {
                return typeof(GenericQueryController<,>).MakeGenericType(new Type[] { queryType, returnType }).GetTypeInfo();
            }
            else if (parameters.Length == 1)
            {
                Type argType = parameters[0].ParameterType;
                return typeof(GenericQueryController<,,>).MakeGenericType(new Type[] { queryType, argType, returnType }).GetTypeInfo();
            }
            throw new ArgumentException("Invalid query type.", "queryType");
        }
    }
}
