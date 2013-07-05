namespace BehaviourTree
{
    public class PrioritySelector<T> : CompositeNode<T>
    {
        internal PrioritySelector(string name, params Node<T>[] children)
            : base(name, children)
        {

        }

        protected override Status Start(ExecutionContext<T> executionContext)
        {
            return ExecuteStepsButBreakOnFirstDone(executionContext, null);
        }

        protected override Status Resume(ExecutionContext<T> executionContext)
        {
            var node = executionContext.GetCurrentRunningNode();
            return ExecuteStepsButBreakOnFirstDone(executionContext, node);
        }
        
        private Status ExecuteStepsButBreakOnFirstDone(ExecutionContext<T> executionContext, Node<T> nodeToResumeAt)
        {
            foreach (var child in Children)
            {
                var status = child.Visit(executionContext, child == nodeToResumeAt);
                if (status == Status.Done) return Status.Done;
                if (status == Status.Running) return Status.Running;
            }
            return Status.Fail;
        }
    }
}
