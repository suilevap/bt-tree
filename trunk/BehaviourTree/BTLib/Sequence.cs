using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BT
{
    /// <summary>
    /// Selector node run one child to finish after the other
    /// </summary>
    /// <typeparam name="TBlackboard"></typeparam>
    public class Sequence<TBlackboard> : CompositeNode<TBlackboard> where TBlackboard : IBlackboard
    {
        internal Sequence(string name, params Node<TBlackboard>[] childs)
            : base(name, childs)
        {

        }

        protected override CompositeStatus UpdateChilds(Context<TBlackboard> context, NodeContext<TBlackboard> nodeContext)
        {
            CompositeStatus result = new CompositeStatus()
            {
                Status = Status.Ok
            };

            int startIndex = 0;
            //if node contains running childs, update it firstly
            if (nodeContext.IsRunning)
            { 
                int index = nodeContext.ChildNodeIndex;
                Node<TBlackboard> node = Childs[index];
                result.Status = node.Update(context);
                result.ChidlNodeIndex = index;
                startIndex = index + 1;
            }
            //if previous node was successful
            if (result.Status == Status.Ok)
            {
                for (int i = startIndex; i < Childs.Length; i++)
                {
                    Node<TBlackboard> node = Childs[i];
                    if (node == null)
                        throw new NullReferenceException("BTNode child can not be null");

                    result.Status = node.Update(context);
                    //stop if node Fails or in running state
                    if (result.Status == Status.Fail || result.Status == Status.Running)
                    {
                        result.ChidlNodeIndex = i;
                        break;
                    }
                }
            }

            return result;
        }


    }
}
