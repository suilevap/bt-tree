using System;

namespace BehaviourTree
{
    public class SpinWhile<T> : Node<T>
    {
        private readonly Func<T, bool> _keepSpinningFunc;

        public SpinWhile(string name, Func<T, bool> keepSpinningFunc) : base(name)
        {
            _keepSpinningFunc = keepSpinningFunc;
        }

        protected override Status Start(ExecutionContext<T> executionContext)
        {
            return Resume(executionContext);
        }

        protected override Status Resume(ExecutionContext<T> executionContext)
        {
            return _keepSpinningFunc(executionContext.Data) ? Status.Running : Status.Done;
        }
    }
}