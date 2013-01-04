using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BT
{
    public class BTPath<T>
    {
        private List<BTGroupNode<T>> _runningNodes;
        private List<int> _runningNodesIndex;


        public int Count { get { return _runningNodes.Count; } }
        
        public BTGroupNode<T> this[int index]
        {
            get { return _runningNodes[index]; }
            set { _runningNodes[index] = value; }
        }

        public BTPath()
        {
            _runningNodes = new List<BTGroupNode<T>>(16);
            _runningNodesIndex = new List<int>(16);
        }


        public void Add(int nodeIndex, BTGroupNode<T> node)
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

        public override string ToString()
        {
            return _runningNodes.ToString();
        }


        internal void RemoveLast()
        {
            _runningNodes.RemoveAt(Count-1);
            _runningNodesIndex.RemoveAt(Count - 1);
        }
    }
}
