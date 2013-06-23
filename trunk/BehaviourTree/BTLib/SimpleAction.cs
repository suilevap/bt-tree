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
        private readonly Action<TBlackboard> _actionExecute;
        private readonly Func<TBlackboard, bool> _actionComplete;
        private readonly Func<TBlackboard, bool> _checkInProgress;


        /// <summary>
        /// Create simple action
        /// </summary>
        /// <param name="name">Node name</param>
        /// <param name="actionStart">Run on node start, should return True if success and node moves to Status.Running state</param>
        internal SimpleAction(string name, Func<TBlackboard, bool> actionStart)
            : this(name, actionStart, null, null, null)
        {}

        /// <summary>
        /// Create simple action
        /// </summary>
        /// <param name="name">Node name</param>
        /// <param name="actionStart">Run on node start, should return True if success and node moves to Status.Running state</param>
        /// <param name="checkInProgress">Check every node tick, should return True if node is still in Status.Running state, false - node execution is complete</param>
        /// <param name="actionExecute">Run every node tick</param>
        internal SimpleAction(string name, Func<TBlackboard, bool> actionStart, Func<TBlackboard, bool> checkInProgress, Action<TBlackboard> actionExecute)
            : this(name, actionStart, checkInProgress, actionExecute, null)
        {}

        /// <summary>
        /// Create simple action
        /// </summary>
        /// <param name="name">Node name</param>
        /// <param name="actionStart">Run on node start, should return True if success and node moves to Status.Running state</param>
        /// <param name="checkInProgress">Check every node tick, should return True if node is still in Status.Running state, false - node execution is complete</param>
        /// <param name="actionExecute">Run every node tick</param>
        /// <param name="actionComplete">Run on node complete. Should return True in case of Ok, false in case of Fail</param>
        internal SimpleAction(string name, Func<TBlackboard, bool> actionStart, Func<TBlackboard, bool> checkInProgress, Action<TBlackboard> actionExecute, Func<TBlackboard, bool> actionComplete)
            :base(name)
        {
            _actionStart = actionStart;
            _actionExecute = actionExecute;
            _checkInProgress = checkInProgress;
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

        protected override void Tick(TBlackboard blackboard)
        {
            if (_actionExecute != null)
            {
                _actionExecute(blackboard);
            }
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
        
        protected override bool IsInProgress(TBlackboard blackboard)
        {
            bool status = false;//immediate complete by default
            if (_checkInProgress != null)
            {
                status = _checkInProgress(blackboard);
            }
            return status;
        }

    }
}
