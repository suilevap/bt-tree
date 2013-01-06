using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BT
{
    public class Sequence<T> : CompositeNode<T>
    {
        internal Sequence(string name, params Node<T>[] childs)
            : base(name, childs)
        {

        }

        protected override Status UpdateChilds(Context<T> context, int? runningNodeIndex)
        {
            Status status = Status.Ok;
            int startIndex = 0;
            if (runningNodeIndex.HasValue)
            { 
                int index = runningNodeIndex.Value;
                status = context.UpdateNode(index, Childs[index], true);
                startIndex = index + 1;
            }
            if (status == Status.Ok)
            {
                for (int i = startIndex; i < Childs.Length; i++)
                {
                    Node<T> node = Childs[i];
                    if (node == null)
                        throw new NullReferenceException("BTNode child can not be null");
                    
                    status = context.UpdateNode(i, node, false);

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
