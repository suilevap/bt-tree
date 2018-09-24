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
    public class Context<TBlackboard> where TBlackboard : IBlackboard
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
        /// Currently active action node
        /// </summary>
        public ActionNode<TBlackboard> LastRunningNode;

        public Context(Node<TBlackboard> root, TBlackboard blackboard,
            INodeContextCreator<TBlackboard> nodeContextCreator = null)
        {
            nodeContextCreator = nodeContextCreator ?? PoolNodeContext<TBlackboard>.Instance;
            Blackboard = blackboard;
            //IsCurrentPathRunning = false;
            _currentPath = new Path<TBlackboard>(nodeContextCreator);
            _lastRunningPath = new Path<TBlackboard>(nodeContextCreator);
            _root = root;
        }

        public Status Update()
        {
            return Update(TimeSpan.FromSeconds(1.0f), true);
        }

        public void Reset()
        {
            _currentPath.Clear();
            _lastRunningPath.Clear();
            LastRunningNode = null;
            Blackboard.Reset();
        }
        /// <summary>
        /// Update BT
        /// </summary>
        /// <returns>result status</returns>
        public Status Update(TimeSpan time, bool forceUpdate)
        {
            Status status = Status.Fail;
            bool needUpdate = forceUpdate;
            if (LastRunningNode != null && !forceUpdate)
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
                var previousRunningNode = LastRunningNode;
                Blackboard.Update(time);
                status = Think();

                if (LastRunningNode != previousRunningNode)
                {
                    Blackboard.RunningActionChanged(LastRunningNode, _lastRunningPath);
                }
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
            status = _root.Update(this);

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
                result = LastRunningNode.Run(this, _lastRunningPath[_lastRunningPath.Count - 1]);
            }
            return result;
        }

        /// <summary>
        /// Save currently visiting node
        /// </summary>
        /// <param name="node">visiting node</param>
        internal NodeContext<TBlackboard> PushVisitingNode(Node<TBlackboard> node)
        {
            NodeContext<TBlackboard> result = _currentPath.Push(node, _lastRunningPath);
            return result;
        }

        /// <summary>
        /// Forget last visiting node
        /// </summary>
        internal void PopVisitingNode()
        {
            _currentPath.RemoveLast();
        }

        public override string ToString()
        {
            return _lastRunningPath.ToString();
        }
    }
}
