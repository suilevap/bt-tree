using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BT
{
    public class BTAction<T> : BTNode<T>
    {
        private Func<T, BTStatus> _action;

        internal BTAction(Func<T, BTStatus> action)
        {
            _action = action;
        }

        internal override BTStatus Execute(BTContext<T> context)
        {
            BTStatus status = _action(context.ExecutionContext);
            return status;
        }
    }
}
