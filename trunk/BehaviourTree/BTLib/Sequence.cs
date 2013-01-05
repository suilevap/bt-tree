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


        private Status Run(Context<T> context, int startIndex)
        {
            Status status = Status.Fail;

            for (int i = startIndex; i < Childs.Length; i++)
            {
                Node<T> node = Childs[i];
                if (node == null)
                    throw new NullReferenceException("BTNode child can not be null");

                status = context.StartNode(startIndex, node);


                if (status == Status.Fail || status == Status.Running)
                {
                    break;
                }
            }
            return status;
        }


        internal override Status Start(Context<T> context)
        {
            return Run(context, 0);
        }

        internal override Status Execute(Context<T> context)
        {
            Status status = Status.Fail;
            int startIndex = 0;
            if (context.TryGetCurrentRunningChildIndex(ref startIndex))
            {
                Node<T> node = Childs[startIndex];
                status = context.ExecuteRunningNode(startIndex, node);
            }
            else
            {
                throw new InvalidOperationException("Call Execute for not running node");
            }

            if (status == Status.Ok)
            {
                status = Run(context, startIndex + 1);
            }
            return status;
        }

    }
}
