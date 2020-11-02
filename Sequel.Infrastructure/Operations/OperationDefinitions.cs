using System;
using System.Collections.Generic;

namespace Sequel.Infrastructure.Operations
{
    public sealed class Generic { }

    public class OperationDefinitions<TOperation> where TOperation : class
    {
        private readonly Dictionary<Type, OperationDefinition<TOperation>> _allDefinitions;

        public OperationDefinitions()
        {
            _allDefinitions = new Dictionary<Type, OperationDefinition<TOperation>>();
        }

        public OperationDefinition<TOperation> Of<T>()
        {
            if (_allDefinitions.ContainsKey(typeof(T)) == false)
                _allDefinitions.Add(typeof(T), new OperationDefinition<TOperation>());
            return _allDefinitions[typeof(T)];
        }

        public Dictionary<Type, OperationDefinition<TOperation>>.KeyCollection Keys
        {
            get { return _allDefinitions.Keys; }
        }

        public OperationDefinition<TOperation> this[Type key]
        {
            get { return _allDefinitions[key]; }
            set { _allDefinitions[key] = value; }
        }

        public OperationDefinition<TOperation> OfNothing
        {
            get { return Of<Generic>(); }
        }

        public Type Find(Type type)
        {
            foreach (var operationDefinition in _allDefinitions)
            {
                if (operationDefinition.Value.Contains(type))
                    return operationDefinition.Key;
            }

            throw new ArgumentException($"{type.Name} is not defined", "type");
        }
    }

    public class OperationDefinition<TOperation>
    {
        private readonly List<Type> _definitions;

        public OperationDefinition()
        {
            _definitions = new List<Type>();
        }

        public OperationDefinition<TOperation> Define<T>() where T : TOperation
        {
            Define(typeof(T));
            return this;
        }

        public void Define(Type type)
        {
            _definitions.Add(type);
        }

        public Type[] Items
        {
            get { return _definitions.ToArray(); }
        }

        public bool Contains(Type type)
        {
            return _definitions.Contains(type);
        }
    }

    public class QueriesDefinitions : OperationDefinitions<IQuery>
    {
    }

    public class CommandsDefinitions : OperationDefinitions<IBaseCommand>
    {
    }
}
