using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BT
{
    public class Condition<T> : Node<T>
    {
        private readonly Func<T, bool> _condition;

        internal Condition(string name, Func<T, bool> condition)
            :base(name)
        {
            _condition = condition;
        }

        internal override Status Start(Context<T> context)
        {
            bool check = _condition(context.ExecutionContext);
            Status status;
            if (check)
            {
                status = Status.Ok;
            }
            else
            {
                status = Status.Fail;
            }
            return status;
        }

        internal override Status Execute(Context<T> context)
        {
            throw new InvalidOperationException("Condition node can not be executed");
        }
    }
}
