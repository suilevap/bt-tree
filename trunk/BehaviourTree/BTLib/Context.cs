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
    public class Context<TBlackboard> where TBlackboard:IBlackboard
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


        /// <summary>
        /// Currently active action node
        /// </summary>
        public ActionNode<TBlackboard> LastRunningNode;

        public Context(Node<TBlackboard> root, TBlackboard blackboard)
        {
            Blackboard = blackboard;
            //IsCurrentPathRunning = false;
            _currentPath = new Path<TBlackboard>();
            _lastRunningPath = new Path<TBlackboard>();
            _isNodeRunning = new Stack<bool>(16);
            _root = root;
        }

        public Status Update()
        {
            return Update(TimeSpan.FromSeconds(1.0f), true);
        }

        /// <summary>
        /// Update BT
        /// </summary>
        /// <returns>result status</returns>
        public Status Update(TimeSpan time, bool forceUpdate)
        {
            Status status = Status.Fail;
            bool needUpdate = forceUpdate;
            if (LastRunningNode != null)
            {
                bool actionInProgres = Run();
                if (actionInProgres)
                {
                    status = Status.Running;
                }
                else 
                {
                    needUpdate = true;
                }
            }
            else
            {
                needUpdate = true;
            }

            if (needUpdate)
            {
                Blackboard.Update(time);
                status = Think();
            }

            return status;
        }

        /// <summary>
        /// Re evalate BT
        /// </summary>
        /// <returns>result status</returns>
        internal Status Think()
        {
            Status status;
            LastRunningNode = null;
            bool isRunning = (_lastRunningPath.Count != 0);
            status = _root.Update(this, 0, isRunning);

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
        /// Run Active Action
        /// </summary>
        /// <returns></returns>
        internal bool Run()
        {
            bool result = false;
            if (LastRunningNode != null)
            {
                result = LastRunningNode.Run(this);
            }
            return result;
        }

        /// <summary>
        /// Save currently visiting node
        /// </summary>
        /// <param name="nodeIndex">Index of visiting node (if parent is composite node)</param>
        /// <param name="node">Visitng node</param>
        /// <param name="isRunningNode">True if visiting node currently in running state</param>
        internal void PushVisitingNode(int nodeIndex, Node<TBlackboard> node, bool isRunningNode)
        {
            _isNodeRunning.Push(isRunningNode);
            _currentPath.Add(nodeIndex, node);
        }

        /// <summary>
        /// Forget last visiting node
        /// </summary>
        internal void PopVisitingNode()
        {
            _isNodeRunning.Pop();
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
            if ((_isNodeRunning.Count == 0 || _isNodeRunning.Peek()) && _lastRunningPath.Count > _currentPath.Count)
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
