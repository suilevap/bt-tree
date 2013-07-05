using System;

namespace BehaviourTree
{
    public class ConditionDecorator<T> : CompositeNode<T>
    {
        private readonly Func<T, bool> _condition;
        private readonly bool _checkConditionOnResume;

        public ConditionDecorator(string name, Func<T, bool> condition, Node<T> decoratedNode, bool checkConditionOnResume)
            :base(name, decoratedNode)
        {
            _condition = condition;
            _checkConditionOnResume = checkConditionOnResume;
        }

        protected override Status Start(ExecutionContext<T> executionContext)
        {
            var condition = _condition(executionContext.Data);
            if (!condition) return Status.Fail;

            return Children[0].Visit(executionContext, false) == Status.Running ? Status.Running : Status.Done;
        }

        protected override Status Resume(ExecutionContext<T> executionContext)
        {
            if (_checkConditionOnResume)
            {
                var condition = _condition(executionContext.Data);
                if (!condition) return Status.Fail;
            }

            return Children[0].Visit(executionContext, true) == Status.Running ? Status.Running : Status.Done;
        }
    }
}
