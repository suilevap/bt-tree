using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BT
{
    public abstract class Node<T>
    {
        public string Name { get; private set; }
        
        internal Node(string name)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                Name = name;
            }
            else
            {
                Name = GetType().Name;
            }
        }

        protected abstract Status OnUpdate(Context<T> context, bool isAlreadyRunning);

        internal Status Update(Context<T> context, int index, bool isAlreadyRunning )
        {
            Status status;
            //store path to this node
            context.PushVisitingNode(index, this, isAlreadyRunning);

            status = OnUpdate(context, isAlreadyRunning);

            if (status != Status.Running)
            {
                //clear running path
                context.PopVisitingNode();
            }
            return status;
        }

        public override string ToString()
        {
            return string.Format("{0}", Name);
        }

    }
}
