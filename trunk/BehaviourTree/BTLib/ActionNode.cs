using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BT
{
    public abstract class ActionNode<T> : Node<T>
    {

        protected ActionNode(string name)
            : base(name)
        { }

        protected abstract bool Start(T executionContext);
        protected abstract bool Tick(T executionContext);

        protected virtual bool Complete(T executionContext)
        {
            return true;
        }

        protected override Status OnUpdate(Context<T> context, bool isRunning)
        {
            Status status;
            if (!isRunning)
            {
                isRunning = Start(context.ExecutionContext);
            }


            if (isRunning)
            {
                isRunning = Tick(context.ExecutionContext);

                if (!isRunning)
                {
                    bool finalTest = Complete(context.ExecutionContext);
                    status = finalTest ? Status.Ok : Status.Fail;
                }
                else
                {
                    status = Status.Running;
                }
            }
            else
            {
                status = Status.Fail;
            }
            return status;
        }

    }
}
