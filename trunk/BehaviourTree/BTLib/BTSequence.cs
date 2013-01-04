using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BT
{
    public class BTSequence<T> : BTGroupNode<T>
    {
        internal BTSequence(string name, params BTGroupNode<T>[] childs)
            : base(name, childs)
        {

        }

        internal override BTStatus Execute(BTContext<T> context)
        {

            BTStatus status = BTStatus.Fail;

            int startIndex = 0;
            context.TryGetCurrentRunningChildIndex(ref startIndex);

            for (int i = startIndex; i < Childs.Length; i++)
            {
                BTGroupNode<T> node = Childs[i];
                if (node == null)
                    throw new NullReferenceException("BTNode child can not be null");

                context.PushVisitingNode(i, node);
                status = node.Execute(context);
                context.PopVisitingNode();

                if (status == BTStatus.Fail || status == BTStatus.Running)
                {
                    break;
                }
            }
            return status;
        }
    }
}
