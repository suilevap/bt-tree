using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BT
{
    /// <summary>
    /// Base BT node
    /// </summary>
    /// <typeparam name="TBlackboard">Type of Blackboard</typeparam>
    public abstract class Node<TBlackboard>
    {
        /// <summary>
        /// Node name
        /// </summary>
        public string Name { get; private set; }
        
        internal Node(string name)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                Name = name;
            }
            else
            {
                Name = GetType().Name;
            }
        }

        /// <summary>
        /// Start node
        /// </summary>
        /// <param name="context">Context for BT current update</param>
        /// <returns>Status</returns>
        protected abstract Status Start(Context<TBlackboard> context);

        /// <summary>
        /// Resume node (node in running state)
        /// </summary>
        /// <param name="context">Context for BT current update</param>
        /// <returns></returns>
        protected abstract Status Resume(Context<TBlackboard> context);

        ///// <summary>
        ///// Run on node visiting
        ///// </summary>
        ///// <param name="context">Context for visit</param>
        ///// <param name="index">Index of visiting node</param>
        ///// <param name="runnindIndex">Index of running child node</param>
        ///// <param name="resume">True, if node is resumed, fasle- started</param>
        ///// <returns></returns>
        //protected abstract Status OnVisit(Context<TBlackboard> context, int index, int? runnindIndex, bool resume);


        //internal Status Visit(Context<TBlackboard> context, int index, bool resume )
        //{
            
        //    Status status;
        //    //store path to this node
        //    context.PushVisitingNode(index, this);

        //    int? runningChildIndex = null;
        //    if (resume)
        //    {
        //        runningChildIndex = context.GetCurrentRunningChildIndex();
        //    }

        //    status = OnVisit(context, index, runningChildIndex, resume);

        //    //clear running path if do not need store tis path
        //    if (status != Status.Running)
        //    {
        //        context.PopVisitingNode(index);
        //    }
        //    return status;
        //}

        public override string ToString()
        {
            return string.Format("{0}", Name);
        }

    }
}
