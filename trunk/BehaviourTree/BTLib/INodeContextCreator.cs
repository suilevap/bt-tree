using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BT
{
    public interface INodeContextCreator<T> where T : IBlackboard
    {
        NodeContext<T> Get(Node<T> node);
        void Release(NodeContext<T> nodeContext);
    }

    internal class SimpleNodeContextCreator<T> : INodeContextCreator<T> where T : IBlackboard
    {
        public NodeContext<T> Get(Node<T> node)
        {
            var result = new NodeContext<T>();
            result.Init(node);
            return result;
        }

        public void Release(NodeContext<T> nodeContext)
        {
            //nothing
        }
    }

    internal class PoolNodeContext<T> : INodeContextCreator<T> where T : IBlackboard
    {
        public static INodeContextCreator<T> Instance = new PoolNodeContext<T>();

        private readonly Stack<NodeContext<T>> _freeObjects;

        public PoolNodeContext(int capacity = 8)
        {
            _freeObjects = new Stack<NodeContext<T>>(capacity);
        }

        public NodeContext<T> Get(Node<T> node)
        {
            NodeContext<T> result;
            if (_freeObjects.Count != 0)
            {
                result = _freeObjects.Pop();
            }
            else
            {
                result = new NodeContext<T>();
            }
            result.Init(node);
            return result;
        }

        public void Release(NodeContext<T> nodeContext)
        {
            _freeObjects.Push(nodeContext);
        }
    }
}
