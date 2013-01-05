using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BT
{
    public class Path<T>
    {
        private List<Node<T>> _runningNodes;
        private List<int> _runningNodesIndex;


        public int Count { get { return _runningNodes.Count; } }

        public Node<T> this[int index]
        {
            get { return _runningNodes[index]; }
            set { _runningNodes[index] = value; }
        }

        public Path()
        {
            _runningNodes = new List<Node<T>>(16);
            _runningNodesIndex = new List<int>(16);
        }


        public void Add(int nodeIndex, Node<T> node)
        {
            _runningNodes.Add(node);
            _runningNodesIndex.Add(nodeIndex);
        }

        public int GetNodeIndex(int pos)
        {
            return _runningNodesIndex[pos];
        }

        public void Clear()
        {
            _runningNodes.Clear();
            _runningNodesIndex.Clear();
        }

        internal void RemoveLast()
        {
            int lastIndex = Count - 1;
            _runningNodes.RemoveAt(lastIndex);
            _runningNodesIndex.RemoveAt(lastIndex);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (Node<T> node in _runningNodes)
            {
                sb.AppendFormat("/{0}", node.ToString());
            }
            return sb.ToString();
        }

    }
}
