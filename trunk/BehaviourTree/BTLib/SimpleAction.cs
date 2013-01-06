﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BT
{
    public class SimpleAction<T> : ActionNode<T>
    {
        private readonly Func<T, Status> _actionStart;
        private readonly Func<T, Status> _actionExecute;
        private readonly Func<T, Status> _actionComplete;
        private readonly Action<T> _actionAbort;


        internal SimpleAction(string name, Func<T, Status> actionStart)
            : this(name, actionStart, null, null, null)
        {}

        internal SimpleAction(string name, Func<T, Status> actionStart, Func<T, Status> actionExecute)
            : this(name, actionStart, actionExecute, null, null)
        {}

        internal SimpleAction(string name, Func<T, Status> actionStart, Func<T, Status> actionExecute, Func<T, Status> actionComplete)
            : this(name, actionStart, actionExecute, actionComplete, null)
        {}

        internal SimpleAction(string name, Func<T, Status> actionStart, Func<T, Status> actionExecute, Func<T, Status> actionComplete, Action<T> actionAbort)
            :base(name)
        {
            _actionStart = actionStart;
            _actionExecute = actionExecute;
            _actionComplete = actionComplete;
            _actionAbort = actionAbort;
        }

        protected override Status Tick(Context<T> context)
        {
            Status status = Status.Ok;
            if (_actionExecute != null)
            {
                status = _actionExecute(context.ExecutionContext);
            }
            return status;
        }

        protected override Status Start(Context<T> context)
        {
            Status status = Status.Ok;
            if (_actionStart != null)
            {
                status = _actionStart(context.ExecutionContext);
            }
            return status;
        }

        protected override Status Complete(Context<T> context)
        {
            Status status = Status.Ok;
            if (_actionComplete != null)
            {
                status = _actionComplete(context.ExecutionContext);
            }
            return status;
        }

        protected override void Abort(Context<T> context)
        {
            if (_actionAbort != null)
            {
                _actionAbort(context.ExecutionContext);
            }
        }

    }
}
