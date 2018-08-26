using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BT
{
    public class NodeContext<T> where T : IBlackboard
    {
        public int ChildNodeIndex;
        public bool IsRunning;
        public object Data;
        public Node<T> Node;

        public NodeContext(Node<T> node)
        {
            Node = node;
            IsRunning = false;
        }

        public TData GetData<TData>()
        {
            return (TData)Data;
        }
    }
}
