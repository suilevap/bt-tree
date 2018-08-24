using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BT
{
    /// <summary>
    /// Primitive aciton node
    /// </summary>
    /// <typeparam name="TBlackboard">Type of using Blackboard</typeparam>
    public abstract class ActionNode<TBlackboard> : Node<TBlackboard> where TBlackboard : IBlackboard
    {

        protected ActionNode(string name)
            : base(name)
        { }

        /// <summary>
        /// Run on node start 
        /// </summary>
        /// <param name="blackboard">Blackboard object</param>
        /// <returns>True if success and node moves to Status.Running state, False in case of Fail</returns>
        protected internal abstract bool Start(TBlackboard blackboard);

        /// <summary>
        /// Check if action is complete
        /// </summary>
        /// <param name="blackboard">Balckboard object</param>
        /// <returns>True if node is still in Status.Running state, False - node execution is complete</returns>
        protected internal abstract bool IsInProgress(TBlackboard blackboard);

        /// <summary>
        /// Run every tick, while node this node in Running State
        /// </summary>
        /// <param name="blackboard">Blackboard object</param>        
        protected internal abstract void Tick(TBlackboard blackboard);

        /// <summary>
        /// Run on node complete (immediately after Run method returs False )
        /// </summary>
        /// <param name="blackboard">Blackboard object</param>
        /// <returns>True in case of Ok status, false in case of Fail</returns>
        protected internal virtual bool Complete(TBlackboard blackboard)
        {
            return true;
        }

        protected override Status OnUpdate(Context<TBlackboard> context, bool isRunning)
        {
            Status status;
            if (!isRunning)
            {
                //start node, if it is not started
                isRunning = Start(context.Blackboard);
            }

            if (isRunning)
            {
                isRunning = IsInProgress(context.Blackboard);//Tick(context.Blackboard);//, isStarting);

                if (!isRunning)
                {
                    bool finalTest = Complete(context.Blackboard);
                    context.LastRunningNode = null;
                    status = finalTest ? Status.Ok : Status.Fail;
                }
                else
                {
                    context.LastRunningNode = this;
                    status = Status.Running;
                }
            }
            else
            {
                status = Status.Fail;
            }
            return status;
        }

        /// <summary>
        /// Run action
        /// </summary>
        /// <param name="context">Context object</param>
        /// <returns>False - action complete</returns>
        public bool Run(Context<TBlackboard> context)
        {
            bool result = false;
            if (IsInProgress(context.Blackboard))
            {
                Tick(context.Blackboard);
                result = true;
            }
            return result;
        }

    }
}
