using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BT
{
    public abstract class Node<T>
    {
        internal abstract Status Execute(Context<T> context);
    }
}
