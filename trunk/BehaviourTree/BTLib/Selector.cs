using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BT
{
    public class Selector<T> : CompositeNode<T>
    {
        internal Selector(string name, params CompositeNode<T>[] childs)
            : base(name, childs)
        {

        }

        internal override Status Execute(Context<T> context)
        {

            Status status = Status.Fail;
            CompositeNode<T> runningNode = null;
            for (int i = 0; i < Childs.Length; i++)
            {
                CompositeNode<T> node = Childs[i];
                if (node == null)
                    throw new NullReferenceException("BTNode child can not be null");

                context.PushVisitingNode(i, node);
                status = node.Execute(context);
                context.PopVisitingNode();

                if (status == Status.Ok || status == Status.Running)
                {
                    runningNode = node;
                    break;
                }
            }
            return status;
        }

    }
}
