using System;
using System.Collections.Generic;
using System.Linq;
using Unity;

namespace Sequel.Infrastructure.Component
{
    public class ComponentManager
    {
        private readonly IUnityContainer _container;
        private readonly List<IComponent> _loadedComponents;
        private readonly IComponent[] _allComponents;
        private readonly ComponentGraph _componentGraph;

        public ComponentManager(IUnityContainer container, IEnumerable<Type> componentsList)
        {
            _container = container;
            _loadedComponents = new List<IComponent>();
            _componentGraph = new ComponentGraph();

            _allComponents = componentsList.Select(type => (IComponent)_container.Resolve(type)).ToArray();
        }

        public void LoadAllComponents()
        {
            foreach (IComponent component in _allComponents)
                _componentGraph.AddToComponentPool(component);

            LoadComponents();
        }

        public void AddAndLoadComponents(params Type[] components)
        {
            IEnumerable<IComponent> componentInstances = components.Select(type => (IComponent)_container.Resolve(type));

            foreach (IComponent component in componentInstances)
                _componentGraph.AddToComponentPool(component);

            LoadComponents();
        }

        public void LoadComponents()
        {
            IComponent[] componentsToLoad = _componentGraph.OrderComponents().Except(_loadedComponents).ToArray();

            foreach (IComponent component in componentsToLoad)
            {
                if (component.Load())
                    _loadedComponents.Add(component);
            }
        }
    }
}
