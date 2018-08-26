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
    public abstract class Node<TBlackboard> where TBlackboard : IBlackboard
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
        /// Run on update this node
        /// </summary>
        /// <param name="context">Context for BT current update</param>
        /// <param name="nodeContext">node context</param>
        /// <returns></returns>
        protected abstract Status OnUpdate(Context<TBlackboard> context, NodeContext<TBlackboard> nodeContext);


        internal Status Update(Context<TBlackboard> context)
        {
            Status status;
            //store path to this node
            NodeContext<TBlackboard> nodeContext = context.PushVisitingNode(this);

            status = OnUpdate(context, nodeContext);

            //clear running path if do not need store tis path
            if (status != Status.Running)
            {
                context.PopVisitingNode();
            }
            return status;
        }

        public override string ToString()
        {
            return string.Format("{0}", Name);
        }

    }
}
