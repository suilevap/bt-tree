using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BT
{
    public abstract class BTNode<T>
    {
        internal abstract BTStatus Execute(BTContext<T> context);
    }
}
