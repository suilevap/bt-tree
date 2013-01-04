using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BT
{
    public class BTSelector<T> : BTGroupNode<T>
    {
        internal BTSelector(string name, params BTGroupNode<T>[] childs)
            : base(name, childs)
        {

        }

        internal override BTStatus Execute(BTContext<T> context)
        {

            BTStatus status = BTStatus.Fail;
            BTGroupNode<T> runningNode = null;
            for (int i = 0; i < Childs.Length; i++)
            {
                BTGroupNode<T> node = Childs[i];
                if (node == null)
                    throw new NullReferenceException("BTNode child can not be null");

                context.PushVisitingNode(i, node);
                status = node.Execute(context);
                context.PopVisitingNode();

                if (status == BTStatus.Ok || status == BTStatus.Running)
                {
                    runningNode = node;
                    break;
                }
            }
            return status;
        }

    }
}
