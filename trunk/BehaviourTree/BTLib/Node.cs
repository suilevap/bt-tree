using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BT
{
    public abstract class Node<T>
    {
        internal abstract Status Start(Context<T> context);
        internal abstract Status Execute(Context<T> context);


        internal virtual void Abort(Context<T> context)
        {

        }
        internal virtual Status Complete(Context<T> context)
        {
            return Status.Ok;
        }


    }
}
