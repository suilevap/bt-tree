using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BT
{
    /// <summary>
    /// Selector node picks first child node which returns Ok or Running status.
    /// </summary>
    /// <typeparam name="TBlackboard">Type of blackboard</typeparam>
    public class Selector<TBlackboard> : CompositeNode<TBlackboard> where TBlackboard : IBlackboard
    {
        internal Selector(string name, params Node<TBlackboard>[] childs)
            : base(name, childs)
        {

        }

        protected override CompositeStatus UpdateChilds(Context<TBlackboard> context, NodeContext<TBlackboard> nodeContext)
        {
            CompositeStatus result = new CompositeStatus()
            {
                Status = Status.Fail
            };
            for (int i = 0; i < Childs.Length; i++)
            {
                Node<TBlackboard> node = Childs[i];
                if (node == null)
                    throw new NullReferenceException("BTNode child can not be null");
                
                //check if this child is running in previous update
                bool isRunning = nodeContext.IsRunning && (i == nodeContext.ChildNodeIndex);
                //update child
                result.Status = node.Update(context);
                //stop if Succeed
                if (result.Status == Status.Ok || result.Status == Status.Running)
                {
                    result.ChidlNodeIndex = i;
                    break;
                }
            }
            return result;
        }

    }
}
