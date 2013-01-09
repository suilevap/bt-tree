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
    public class Sequence<TBlackboard> : CompositeNode<TBlackboard>
    {
        internal Sequence(string name, params Node<TBlackboard>[] childs)
            : base(name, childs)
        {

        }

        protected override Status UpdateChilds(Context<TBlackboard> context, int? runningNodeIndex)
        {
            Status status = Status.Ok;
            int startIndex = 0;
            //if node contains running childs, update it firstly
            if (runningNodeIndex.HasValue)
            { 
                int index = runningNodeIndex.Value;
                Node<TBlackboard> node = Childs[index];
                status = node.Update(context, index, true);

                startIndex = index + 1;
            }
            //if previous node was successful
            if (status == Status.Ok)
            {
                for (int i = startIndex; i < Childs.Length; i++)
                {
                    Node<TBlackboard> node = Childs[i];
                    if (node == null)
                        throw new NullReferenceException("BTNode child can not be null");
                    
                    status = node.Update(context, i, false);
                    //stop if node Fails or in running state
                    if (status == Status.Fail || status == Status.Running)
                    {
                        break;
                    }
                }
            }

            return status;
        }


    }
}
