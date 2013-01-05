using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BT
{
    public class SimpleAction<T> : Node<T>
    {
        private readonly Func<T, Status> _actionStart;
        private readonly Func<T, Status> _actionExecute;
        private readonly Func<T, Status> _actionComplete;
        private readonly Action<T> _actionAbort;


        internal SimpleAction(Func<T, Status> actionStart, Func<T, Status> actionExecute, Func<T, Status> actionComplete, Action<T> actionAbort)
        {
            _actionStart = actionStart;
            _actionExecute = actionExecute;
            _actionComplete = actionComplete;
            _actionAbort = actionAbort;
        }

        internal override Status Execute(Context<T> context)
        {
            Status status = Status.Ok;
            if (_actionExecute != null)
            {
                status = _actionExecute(context.ExecutionContext);
            }
            return status;
        }

        internal override Status Start(Context<T> context)
        {
            Status status = Status.Ok;
            if (_actionStart != null)
            {
                status = _actionStart(context.ExecutionContext);
            }
            return status;
        }

        internal override Status Complete(Context<T> context)
        {
            Status status = Status.Ok;
            if (_actionComplete != null)
            {
                status = _actionComplete(context.ExecutionContext);
            }
            return status;
        }
        
        internal override void Abort(Context<T> context)
        {
            if (_actionAbort != null)
            {
                _actionAbort(context.ExecutionContext);
            }
        }


    }
}
