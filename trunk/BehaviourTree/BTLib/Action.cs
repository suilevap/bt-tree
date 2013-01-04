using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BT
{
    public class Action<T> : Node<T>
    {
        private Func<T, Status> _action;

        internal Action(Func<T, Status> action)
        {
            _action = action;
        }

        internal override Status Execute(Context<T> context)
        {
            Status status = _action(context.ExecutionContext);
            return status;
        }
    }
}
