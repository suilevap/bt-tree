using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BT
{
    public class BTSelector : BTNode
    {
        public BTSelector(string name, params BTNode[] childs)
            :base(name, childs)
        {
            
        }

        internal override BTStatus Execute(BTContext context, bool currentPathRunning)
        {
            BTNode lastRunningNode = null;
            bool currentPathRunning = context.IsCurrentPathRunning;
            if (context.IsCurrentPathRunning)
            {
                lastRunningNode = context.LastPath[context.CurrentPath.Count];
            }

            BTStatus status = BTStatus.Fail;
            BTNode runningNode = null;
            foreach (BTNode node in Childs)
            {
                if (node == null)
                    throw new NullReferenceException("BTNode child can not be null");

                context.CurrentPath.Add(node);
                context.IsCurrentPathRunning = currentPathRunning && (node == lastRunningNode);

                status = node.Execute(context);

                context.CurrentPath.RemoveLast();

                if (status == BTStatus.Ok || status == BTStatus.Running)
                {
                    runningNode = node;
                    break;
                }
            }
            context.IsCurrentPathRunning = currentPathRunning;
            return status;
        }
    }
}
