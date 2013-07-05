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

        protected internal override bool Start(TBlackboard blackboard)
        {
            return Childs.All(x => x.Start(blackboard));
        }

        protected internal override bool IsInProgress(TBlackboard blackboard)
        {
            return Childs.All(x => x.IsInProgress(blackboard));
        }

        protected internal override void Tick(TBlackboard blackboard)
        {
            Array.ForEach(Childs, x => x.Tick(blackboard));
        }

        protected internal override bool Complete(TBlackboard blackboard)
        {
            return Childs.All(x => x.Complete(blackboard));

        }
    }
}
