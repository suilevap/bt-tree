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

        protected abstract bool Start(Context<T> context);
        protected abstract bool Tick(Context<T> context);

        protected virtual bool Complete(Context<T> context)
        {
            return true;
        }

        protected override Status OnUpdate(Context<T> context, bool isRunning)
        {
            Status status;
            if (!isRunning)
            {
                isRunning = Start(context);
            }


            if (isRunning)
            {
                isRunning = Tick(context);

                if (!isRunning)
                {
                    bool finalTest = Complete(context);
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
