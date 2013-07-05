using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BT
{
    /// <summary>
    /// Context for BT update, store curent BT state, previous BT state and reference to the blackboard
    /// </summary>
    /// <typeparam name="TBlackboard">Type of blackboard</typeparam>
    public class Context<TBlackboard>
    {
        /// <summary>
        /// Reference to the blackboard
        /// </summary>
        internal TBlackboard Blackboard { get; private set; }

        /// <summary>
        /// BT root node
        /// </summary>
        private readonly Node<TBlackboard> _root;

        /// <summary>
        /// Curernt path, used for BT update
        /// </summary>
        private Path<TBlackboard> _currentPath;

        /// <summary>
        /// Previous path, store last running nodes
        /// </summary>
        private Path<TBlackboard> _lastRunningPath;

        /// <summary>
        /// Used in BT update to optimize check: is current node in a running state or not
        /// </summary>
        private readonly Stack<bool> _isNodeRunning;


        internal Context(Node<TBlackboard> root, TBlackboard blackboard)
        {
            Blackboard = blackboard;
            //IsCurrentPathRunning = false;
            _currentPath = new Path<TBlackboard>();
            _lastRunningPath = new Path<TBlackboard>();
            _isNodeRunning = new Stack<bool>(16);
            _root = root;
        }

        /// <summary>
        /// Visit BT
        /// </summary>
        /// <returns>result status</returns>
        public Status Update()
        {
            Status status;
            bool isRunning = (_lastRunningPath.Count != 0);
            status = _root.Visit(this, 0, isRunning);

            if (status == Status.Running)
            {
                //save current BT state for next time

                //swap
                var tmp = _lastRunningPath;
                _lastRunningPath = _currentPath;
                _currentPath = tmp;
            }
            else
            {
                _lastRunningPath.Clear();
            }
            _currentPath.Clear();
            _isNodeRunning.Clear();

            return status;
        }

        /// <summary>
        /// Save currently visiting node
        /// </summary>
        /// <param name="nodeIndex">Index of visiting node (if parent is composite node)</param>
        /// <param name="node">Visitng node</param>
        internal void PushVisitingNode(int nodeIndex, Node<TBlackboard> node)
        {
            _currentPath.Add(nodeIndex, node);

        }

        /// <summary>
        /// Forget last visiting node
        /// </summary>
        internal void PopVisitingNode(int nodeIndex)
        {
            _currentPath.RemoveLast();
        }
        /// <summary>
        /// Return index of running node (if visiting node is composite node)
        /// </summary>
        /// <returns>Child index or null</returns>
        internal int? GetCurrentRunningChildIndex()
        {
            int? result = null;
            //check if vising noe is running
            if (_lastRunningPath.Count > _currentPath.Count)
            {
                //get index from pevious running nodes
                result = _lastRunningPath.GetNodeIndex(_currentPath.Count);
            }
            return result;
        }

        public override string ToString()
        {
            return _lastRunningPath.ToString();
        }
    }
}
