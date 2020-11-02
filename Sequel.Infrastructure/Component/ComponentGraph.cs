using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Sequel.Infrastructure.Component
{
    public class ComponentGraph
    {
        private enum DependencyRelation
        {
            NotRelated,
            Parent,
            Child
        }

        private class Node
        {
            public Node()
            {
                Dependencies = new List<Node>();
            }

            public Node(IComponent component)
            {
                Component = component;
                Type = component.GetType();
                Dependencies = new List<Node>();
            }

            public IComponent Component { get; private set; }
            public Type Type { get; private set; }
            public List<Node> Dependencies { get; private set; }
        }

        private readonly List<IComponent> _queued = new List<IComponent>();
        private readonly Node _root = new Node() { };

        public ComponentGraph()
        {
            AppDomain.CurrentDomain.AssemblyResolve += AssemblyResolve;
        }

        private Assembly AssemblyResolve(object sender, ResolveEventArgs args)
        {
            return AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.FullName == args.Name);
        }

        public ComponentState AddToComponentPool(IComponent component)
        {
            Node node = FindNode(_root, component.GetType());
            if (node != null)
                return ComponentState.AlreadyExist;

            ComponentState componentState = AddToGraph(component);
            if (componentState != ComponentState.Loadable)
            {
                _queued.Add(component);
                return componentState;
            }

            int i = 0;
            while (i < _queued.Count)
            {
                if (AddToGraph(_queued[i]) == ComponentState.Loadable)
                {
                    _queued.RemoveAt(i);
                    i = 0;
                }
                else
                    i++;
            }

            return ComponentState.Loadable;
        }

        private ComponentState AddToGraph(IComponent component)
        {
            //AssemblyName[] assemblydependencies = component.GetType().Assembly.GetReferencedAssemblies().Where(an => an.FullName.StartsWith("Yuguo")).ToArray();
            //Assembly[] loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies().Where(a => a.FullName.StartsWith("Yuguo")).ToArray();

            //IEnumerable<string> missingAssemblies = assemblydependencies.Select(a => a.FullName).Except(loadedAssemblies.Select(a => a.FullName));

            //if (missingAssemblies.Count() != 0)
            //    return ModuleState.MissingAssembly;

            if (component.Dependencies.Length == 0)
            {
                _root.Dependencies.Add(new Node(component));
                return ComponentState.Loadable;
            }

            List<Node> parents = new List<Node>();

            foreach (Type dependencies in component.Dependencies)
            {
                Node parent = FindNode(_root, dependencies);
                if (parent == null)
                    return ComponentState.MissingDependency;
                parents.Add(parent);
            }

            IEnumerable<Node> mostCommonAncestors = FindBranches(parents.ToArray());

            Node treeComponent = new Node(component);
            foreach (Node mostCommonAncestor in mostCommonAncestors)
            {
                mostCommonAncestor.Dependencies.Add(treeComponent);
            }
            return ComponentState.Loadable;
        }

        private Node FindNode(Node node, Type toFind)
        {
            if (node == null)
                return null;

            foreach (Node child in node.Dependencies)
            {
                if (child.Type.FullName == toFind.FullName)
                    return child;
                Node found = FindNode(child, toFind);
                if (found != null)
                    return found;
            }
            return null;
        }

        private IEnumerable<Node> FindBranches(Node[] nodes)
        {
            List<Node> lastChilds = new List<Node>();

            if (nodes.Length < 2)
                return nodes;

            DependencyRelation firstRelation = AreRelated(nodes[0], nodes[1]);
            if (firstRelation == DependencyRelation.NotRelated)
            {
                lastChilds.Add(nodes[0]);
                lastChilds.Add(nodes[1]);
            }
            else if (firstRelation == DependencyRelation.Child)
                lastChilds.Add(nodes[1]);
            else
                lastChilds.Add(nodes[0]);

            for (int i = 2; i < nodes.Length; i++)
            {
                foreach (Node node in nodes)
                {
                    DependencyRelation relation = AreRelated(node, nodes[i]);
                    if (relation == DependencyRelation.NotRelated)
                        lastChilds.Add(nodes[i]);
                    else if (relation == DependencyRelation.Parent)
                        nodes[i] = node;
                }
            }
            return lastChilds;
        }

        private DependencyRelation AreRelated(Node nodeOne, Node nodeTwo)
        {
            if (FindNode(nodeOne, nodeTwo.Type) != null)
                return DependencyRelation.Child;
            if (FindNode(nodeTwo, nodeOne.Type) != null)
                return DependencyRelation.Parent;
            return DependencyRelation.NotRelated;
        }

        public IComponent[] OrderComponents()
        {
            IEnumerable<Node> nodeList = ReverseGraphWalk(_root, new List<Node>());
            return nodeList.Select(n => n.Component).ToArray();
        }

        List<Node> ReverseGraphWalk(Node parentNode, List<Node> nodeList)
        {
            foreach (Node node in parentNode.Dependencies)
            {
                if (nodeList.Contains(node) == false)
                    nodeList = ReverseGraphWalk(node, nodeList);
            }

            if (parentNode.Component != null)
                nodeList.Add(parentNode);
            return nodeList;
        }
    }
}
