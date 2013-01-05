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


        internal abstract Status Start(Context<T> context);
        internal abstract Status Execute(Context<T> context);

        internal virtual void Abort(Context<T> context)
        {

        }
        internal virtual Status Complete(Context<T> context)
        {
            return Status.Ok;
        }

        internal Status Tick(Context<T> context )
        {
            Status status = Execute(context);

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
