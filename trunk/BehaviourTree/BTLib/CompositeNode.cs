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
    public abstract class CompositeNode<TBlackboard> : Node<TBlackboard>
    {
        public Node<TBlackboard>[] Childs { get; protected set; }

        protected CompositeNode(string name, params Node<TBlackboard>[] childs)
            : base(name)
        {
            Childs = childs;
        }

        /// <summary>
        /// Evaluate state of composite node based on their child nodes
        /// </summary>
        /// <param name="context">BT Context</param>
        /// <param name="runningNodeIndex">Index of child node, which currantly in running state, null if node does not have such child node</param>
        /// <returns></returns>
        protected abstract Status OnVisit(Context<TBlackboard> context, bool resume, int? runningNodeIndex);

        protected override Status Start(Context<TBlackboard> context)
        {
            throw new NotImplementedException();
        }

        protected override Status Resume(Context<TBlackboard> context)
        {
            throw new NotImplementedException();
        }

        protected abstract IEnumerable<int> 

        private Status Visit(Context<TBlackboard> context, bool resume)
        {
            int? runningIndex = context.GetCurrentRunningChildIndex();
            Status status = OnVisit(context, resume, runningIndex);
            return status;
        }

        public override string ToString()
        {
            return string.Format("{0}({1})", base.ToString(), string.Join(",", Childs.ToString()));
        }
    }
}
