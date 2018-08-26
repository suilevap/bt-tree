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

        private readonly List<NodeContext<TBlackboard>> _nodesContext;


        public int Count { get { return _nodesContext.Count; } }

        internal NodeContext<TBlackboard> this[int index]
        {
            get { return _nodesContext[index]; }
            set { _nodesContext[index] = value; }
        }

        internal Path()
        {
            _nodesContext = new List<NodeContext<TBlackboard>>(16);
        }


        private void Add(NodeContext<TBlackboard> nodeContext)
        {
            _nodesContext.Add(nodeContext);
        }

        internal NodeContext<TBlackboard> Push(Node<TBlackboard> node, Path<TBlackboard> runningPath)
        {
            NodeContext<TBlackboard> result;
            int currentLevel = Count;
            bool prevRunning = currentLevel == 0 || _nodesContext[currentLevel - 1].IsRunning;
            //if previous node is last running path
            if (prevRunning && currentLevel < runningPath.Count )
            {
                var candidate = runningPath[currentLevel];
                if (candidate.Node == node)
                {
                    //if this node is on last running path
                    result = candidate;
                    result.IsRunning = true;
                }
                else
                {
                    result = new NodeContext<TBlackboard>(node);
                }
            }
            else
            {
                result = new NodeContext<TBlackboard>(node);
            }
            Add(result);
            return result;
        }

        internal void Clear()
        {
            _nodesContext.Clear();
        }

        internal void RemoveLast()
        {
            int lastIndex = Count - 1;
            _nodesContext.RemoveAt(lastIndex);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (NodeContext<TBlackboard> node in _nodesContext)
            {
                sb.AppendFormat("/{0}", node.Node.Name);
            }
            return sb.ToString();
        }

    }
}
