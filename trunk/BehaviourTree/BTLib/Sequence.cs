using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BT
{
    public class Sequence<T> : CompositeNode<T>
    {
        internal Sequence(string name, params CompositeNode<T>[] childs)
            : base(name, childs)
        {

        }

        internal override Status Execute(Context<T> context)
        {

            Status status = Status.Fail;

            int startIndex = 0;
            context.TryGetCurrentRunningChildIndex(ref startIndex);

            for (int i = startIndex; i < Childs.Length; i++)
            {
                CompositeNode<T> node = Childs[i];
                if (node == null)
                    throw new NullReferenceException("BTNode child can not be null");

                context.PushVisitingNode(i, node);
                status = node.Execute(context);
                context.PopVisitingNode();

                if (status == Status.Fail || status == Status.Running)
                {
                    break;
                }
            }
            return status;
        }
    }
}
