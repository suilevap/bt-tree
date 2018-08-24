using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BT
{
    /// <summary>
    /// Class to store path throught tree
    /// </summary>
    /// <typeparam name="TBlackboard">Type of blackboard</typeparam>
    public class Path<TBlackboard> where TBlackboard : IBlackboard
    {
        private readonly List<Node<TBlackboard>> _nodes;
        private readonly List<int> _nodesIndex;


        public int Count { get { return _nodes.Count; } }

        internal Node<TBlackboard> this[int index]
        {
            get { return _nodes[index]; }
            set { _nodes[index] = value; }
        }

        internal Path()
        {
            _nodes = new List<Node<TBlackboard>>(16);
            _nodesIndex = new List<int>(16);
        }


        internal void Add(int nodeIndex, Node<TBlackboard> node)
        {
            _nodes.Add(node);
            _nodesIndex.Add(nodeIndex);
        }

        internal int GetNodeIndex(int pos)
        {
            return _nodesIndex[pos];
        }

        internal void Clear()
        {
            _nodes.Clear();
            _nodesIndex.Clear();
        }

        internal void RemoveLast()
        {
            int lastIndex = Count - 1;
            _nodes.RemoveAt(lastIndex);
            _nodesIndex.RemoveAt(lastIndex);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (Node<TBlackboard> node in _nodes)
            {
                sb.AppendFormat("/{0}", node.Name);
            }
            return sb.ToString();
        }

    }
}
