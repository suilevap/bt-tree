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

        public NodeContext()
        {
        }

        public void Init(Node<T> node)
        {
            Node = node;
            IsRunning = false;
            Data = null;
            ChildNodeIndex = 0;
        }

        public TData GetData<TData>()
        {
            return (TData)Data;
        }

        public void SetData<TData>(TData data)
        {
            Data = data;
        }

        public void CopyFrom(NodeContext<T> fromoNode)
        {
            ChildNodeIndex = fromoNode.ChildNodeIndex;
            IsRunning = fromoNode.IsRunning;
            Data = fromoNode.Data;
            Node = fromoNode.Node;
        }
    }
}
