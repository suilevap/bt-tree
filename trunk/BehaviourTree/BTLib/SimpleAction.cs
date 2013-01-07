﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BT
{
    public class SimpleAction<T> : ActionNode<T>
    {
        private readonly Func<T, bool> _actionStart;
        private readonly Func<T, bool> _actionExecute;
        private readonly Func<T, bool> _actionComplete;


        internal SimpleAction(string name, Func<T, bool> actionStart)
            : this(name, actionStart, null, null)
        {}

        internal SimpleAction(string name, Func<T, bool> actionStart, Func<T, bool> actionExecute)
            : this(name, actionStart, actionExecute, null)
        {}

        internal SimpleAction(string name, Func<T, bool> actionStart, Func<T, bool> actionExecute, Func<T, bool> actionComplete)
            :base(name)
        {
            _actionStart = actionStart;
            _actionExecute = actionExecute;
            _actionComplete = actionComplete;
        }


        protected override bool Start(T executionContext)
        {
            bool status = false;//Fail by default
            if (_actionStart != null)
            {
                status = _actionStart(executionContext);
            }
            return status;
        }

        protected override bool Tick(T executionContext)
        {
            bool status = true;//immediate complete by default
            if (_actionExecute != null)
            {
                status = _actionExecute(executionContext);
            }
            return status;
        }

        protected override bool Complete(T executionContext)
        {
            bool status = true; //Success complete by default
            if (_actionComplete != null)
            {
                status = _actionComplete(executionContext);
            }
            return status;
        }

    }
}
