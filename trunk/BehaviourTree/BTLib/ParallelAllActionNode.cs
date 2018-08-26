using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BT
{
    /// <summary>
    /// Primitive All parallel action
    /// </summary>
    /// <typeparam name="TBlackboard">Type of blackboard</typeparam>
    public class ParallelAllActionNode<TBlackboard> : ActionNode<TBlackboard> where TBlackboard : IBlackboard
    {
        public ActionNode<TBlackboard>[] Childs { get; protected set; }

        internal ParallelAllActionNode(string name, params ActionNode<TBlackboard>[] childs)
            : base(name)
        {
            Childs = childs;
        }

        protected internal override bool Start(TBlackboard blackboard, NodeContext<TBlackboard> nodeContext)
        {
            return Childs.All(x => x.Start(blackboard, nodeContext));
        }

        protected internal override bool IsInProgress(TBlackboard blackboard, NodeContext<TBlackboard> nodeContext)
        {
            return Childs.All(x => x.IsInProgress(blackboard, nodeContext));
        }

        protected internal override void Tick(TBlackboard blackboard, NodeContext<TBlackboard> nodeContext)
        {
            //Array.ForEach(Childs, x => x.Tick(blackboard));
            foreach(var child in Childs)
            {
                child.Tick(blackboard, nodeContext);
            }
        }

        protected internal override bool Complete(TBlackboard blackboard, NodeContext<TBlackboard> nodeContext)
        {
            return Childs.All(x => x.Complete(blackboard, nodeContext));

        }
    }
}
