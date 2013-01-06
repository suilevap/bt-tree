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


        protected abstract Status Start(Context<T> context);
        protected abstract Status Tick(Context<T> context);

        protected virtual void Abort(Context<T> context)
        {

        }
        protected virtual Status Complete(Context<T> context)
        {
            return Status.Ok;
        }

        internal Status Update(Context<T> context, bool isAlreadyRunning )
        {
            Status status;

            if (!isAlreadyRunning)
            {
                status = Start(context);
            }
            else
            {
                status = Status.Running;
            }

            if (status == Status.Running)
            {
                status = Tick(context);
            }
            if (status == Status.Ok)
            {
                status = Complete(context);
            }
            if (status == Status.Fail)
            {
                Abort(context);
            }
            return status;
        }

        public override string ToString()
        {
            return string.Format("{0}", Name);
        }

    }
}
