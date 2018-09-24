using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BT
{
    /// <summary>
    /// Base class for Composite nodes, return status depend on child node  status and concrete implementation
    /// </summary>
    /// <typeparam name="TBlackboard">Type of blackboard</typeparam>
    public abstract class CompositeNode<TBlackboard> : Node<TBlackboard> where TBlackboard : IBlackboard
    {
        public struct CompositeStatus
        {
            public Status Status;
            public int ChidlNodeIndex;
        }

        public Node<TBlackboard>[] Childs { get; protected set; }

        protected CompositeNode(string name, params Node<TBlackboard>[] childs)
            :base(name)
        {
            Childs = childs;
        }

        /// <summary>
        /// Evaluate state of composite node based on their child nodes
        /// </summary>
        /// <param name="context">BT Context</param>
        /// <param name="runningNodeIndex">Index of child node, which currantly in running state, null if node does not have such child node</param>
        /// <returns></returns>
        protected abstract CompositeStatus UpdateChilds(Context<TBlackboard> context, NodeContext<TBlackboard> nodeContext);

        protected override Status OnUpdate(Context<TBlackboard> context, NodeContext<TBlackboard> nodeContext)
        {
            CompositeStatus status = UpdateChilds(context, nodeContext);
            if (status.Status == Status.Running)
            {
                nodeContext.ChildNodeIndex = status.ChidlNodeIndex;
            }
            return status.Status;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(base.ToString());
            sb.Append('(');
            foreach(Node<TBlackboard> node in Childs)
            {
                sb.AppendFormat("{0},", node);
            }
            sb.Append(')');
            return sb.ToString();
        }
    }
}
