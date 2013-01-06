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
        {}

        protected abstract Status Start(Context<T> context);
        protected abstract Status Tick(Context<T> context);

        protected virtual void Abort(Context<T> context)
        {

        }
        protected virtual Status Complete(Context<T> context)
        {
            return Status.Ok;
        }

        protected override Status OnUpdate(Context<T> context, bool isAlreadyRunning)
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

    }
}
