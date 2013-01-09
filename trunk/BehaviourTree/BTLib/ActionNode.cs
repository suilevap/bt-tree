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
    public abstract class ActionNode<TBlackboard> : Node<TBlackboard>
    {

        protected ActionNode(string name)
            : base(name)
        { }

        /// <summary>
        /// Run on node start 
        /// </summary>
        /// <param name="blackboard">Blackboard object</param>
        /// <returns>True if success and node moves to Status.Running state, False in case of Fail</returns>
        protected abstract bool Start(TBlackboard blackboard);

        /// <summary>
        /// Run every tick, while node this node in Running State
        /// </summary>
        /// <param name="blackboard">Blackboard object</param>
        /// <param name="isFirstRun">True if Tick runs for this node first time</param>
        /// <returns>True if node is still in Status.Running state, False - node execution is complete</returns>
        protected abstract bool Tick(TBlackboard blackboard, bool isFirstRun);

        /// <summary>
        /// Run on node complete (immediately after Tick method returs False )
        /// </summary>
        /// <param name="blackboard">Blackboard object</param>
        /// <returns>True in case of Ok status, false in case of Fail</returns>
        protected virtual bool Complete(TBlackboard blackboard)
        {
            return true;
        }

        protected override Status OnUpdate(Context<TBlackboard> context, bool isRunning)
        {
            Status status;
            bool isStarting = !isRunning;
            if (!isRunning)
            {
                //start node, if it is not started
                isRunning = Start(context.Blackboard);
            }

            if (isRunning)
            {
                isRunning = Tick(context.Blackboard, isStarting);

                if (!isRunning)
                {
                    bool finalTest = Complete(context.Blackboard);
                    status = finalTest ? Status.Ok : Status.Fail;
                }
                else
                {
                    status = Status.Running;
                }
            }
            else
            {
                status = Status.Fail;
            }
            return status;
        }

    }
}
