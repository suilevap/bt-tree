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



        private Status Run(Context<T> context, int runningNodeIndex)
        {
            Status status = Status.Fail;
            for (int i = 0; i < Childs.Length; i++)
            {
                CompositeNode<T> node = Childs[i];
                if (node == null)
                    throw new NullReferenceException("BTNode child can not be null");

                if (i != runningNodeIndex)
                {
                    status = context.StartNode(i, node);
                }
                else
                {
                    status = context.ExecuteNode(i, node);
                }

                if (status == Status.Ok || status == Status.Running)
                {
                    break;
                }
            }
            return status;
        }


        internal override Status Start(Context<T> context)
        {
            Status status = Run(context, -1);
            return status;
        }

        internal override Status Execute(Context<T> context)
        {
            int runningIndex = -1;
            bool runningNodeExsists = context.TryGetCurrentRunningChildIndex(ref runningIndex);
            if (!runningNodeExsists)
                throw new InvalidOperationException("Call Execute for not running node");

            Status status = Run(context, runningIndex);
            return status;
        }

    }
}
