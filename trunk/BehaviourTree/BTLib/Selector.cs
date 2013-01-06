using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BT
{
    public class Selector<T> : CompositeNode<T>
    {
        internal Selector(string name, params Node<T>[] childs)
            : base(name, childs)
        {

        }

        protected override Status UpdateChilds(Context<T> context, int? runningNodeIndex)
        {
            Status status = Status.Fail;
            for (int i = 0; i < Childs.Length; i++)
            {
                Node<T> node = Childs[i];
                if (node == null)
                    throw new NullReferenceException("BTNode child can not be null");

                bool isRunning = (i != runningNodeIndex);
                status = node.Update(context, i, isRunning);

                if (status == Status.Ok || status == Status.Running)
                {
                    break;
                }
            }
            return status;
        }

    }
}
