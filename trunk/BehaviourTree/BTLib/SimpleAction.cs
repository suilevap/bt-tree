using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BT
{
    /// <summary>
    /// Simple implementaion of ActionNode
    /// </summary>
    /// <typeparam name="TBlackboard">Type of Blackboard</typeparam>
    public class SimpleAction<TBlackboard> : ActionNode<TBlackboard>
    {
        private readonly Func<TBlackboard, bool> _actionStart;
        private readonly Func<TBlackboard, bool> _actionExecute;
        private readonly Func<TBlackboard, bool> _actionComplete;


        /// <summary>
        /// Create simple action
        /// </summary>
        /// <param name="name">Node name</param>
        /// <param name="actionStart">Run on node start, should return True if success and node moves to Status.Running state</param>
        internal SimpleAction(string name, Func<TBlackboard, bool> actionStart)
            : this(name, actionStart, null, null)
        {}

        /// <summary>
        /// Create simple action
        /// </summary>
        /// <param name="name">Node name</param>
        /// <param name="actionStart">Run on node start, should return True if success and node moves to Status.Running state</param>
        /// <param name="actionExecute">Run every node tick, should return True if node is still in Status.Running state, false - node execution is complete</param>
        internal SimpleAction(string name, Func<TBlackboard, bool> actionStart, Func<TBlackboard, bool> actionExecute)
            : this(name, actionStart, actionExecute, null)
        {}

        /// <summary>
        /// Create simple action
        /// </summary>
        /// <param name="name">Node name</param>
        /// <param name="actionStart">Run on node start, should return True if success and node moves to Status.Running state</param>
        /// <param name="actionExecute">Run every node tick, should return True if node is still in Status.Running state, false - node execution is complete</param>
        /// <param name="actionComplete">Run on node complete. Should return True in case of Ok, false in case of Fail</param>
        internal SimpleAction(string name, Func<TBlackboard, bool> actionStart, Func<TBlackboard, bool> actionExecute, Func<TBlackboard, bool> actionComplete)
            :base(name)
        {
            _actionStart = actionStart;
            _actionExecute = actionExecute;
            _actionComplete = actionComplete;
        }


        protected override bool Start(TBlackboard blackboard)
        {
            bool status = false;//Fail by default
            if (_actionStart != null)
            {
                status = _actionStart(blackboard);
            }
            return status;
        }

        protected override bool Tick(TBlackboard blackboard, bool isFirstRun)
        {
            bool status = true;//immediate complete by default
            if (_actionExecute != null)
            {
                status = _actionExecute(blackboard);
            }
            return status;
        }

        protected override bool Complete(TBlackboard blackboard)
        {
            bool status = true; //Success complete by default
            if (_actionComplete != null)
            {
                status = _actionComplete(blackboard);
            }
            return status;
        }

    }
}
