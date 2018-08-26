using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BT
{
    /// <summary>
    /// Dynamice node choose which node execute next in runtime
    /// </summary>
    /// <typeparam name="TBlackboard"></typeparam>
    public abstract class DynamicNode<TBlackboard> : Node<TBlackboard> where TBlackboard : IBlackboard
    {


        public DynamicNode(string name)
            : base(name)
        {
        }

        protected abstract Node<TBlackboard> GetCurrentNode(TBlackboard blackboard);
        protected abstract void OnComplete(TBlackboard blackboard, Status status);


        protected override Status OnUpdate(Context<TBlackboard> context, NodeContext<TBlackboard> nodeContext)
        {
            Status result;
            var node = GetCurrentNode(context.Blackboard);
            if (node != null)
            {
                result = node.Update(context);
            }
            else
            {
                result = Status.Fail;
            }

            if (result != Status.Running)
            {
                OnComplete(context.Blackboard, result);
            }
            
            return result;

        }


    }
}
