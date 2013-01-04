using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BT
{
    public class BTPath
    {
        private List<BTNode> _runningNodes;
        private List<int> _runningNodesIndex;


        public int Count { get { return _runningNodes.Count; } }
        
        public BTNode this[int index]
        {
            get { return _runningNodes[index]; }
            set { _runningNodes[index] = value; }
        }

        public BTPath()
        {
            _runningNodes = new List<BTNode>(16);
            _runningNodesIndex = new List<int>(16);
        }


        public void Add(int nodeIndex, BTNode node)
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
