using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BT
{
    /// <summary>
    /// While node
    /// </summary>
    /// <typeparam name="TBlackboard"></typeparam>
    public class DecoratorCondtion<TBlackboard> : Node<TBlackboard> where TBlackboard : IBlackboard
    {
        private readonly Action<TBlackboard, NodeContext<TBlackboard>> _initFunc;
        private readonly Func<TBlackboard, NodeContext<TBlackboard>, Status> _conditionFunc;


        private readonly Node<TBlackboard> _child;

        public DecoratorCondtion(string name, Action<TBlackboard, NodeContext<TBlackboard>> initFunc, Func<TBlackboard, NodeContext<TBlackboard>, Status> checkFunc, Node<TBlackboard> child)
            : base(name)
        {
            _child = child;
            _initFunc = initFunc;
            _conditionFunc = checkFunc;
        }

        protected override Status OnUpdate(Context<TBlackboard> context, NodeContext<TBlackboard> nodeContext)
        {
            Status result;
            if (!nodeContext.IsRunning)
            {
                if (_initFunc != null)
                {
                    _initFunc(context.Blackboard, nodeContext);
                }
            }
            result = _conditionFunc(context.Blackboard, nodeContext);
            if (result == Status.Running)
            {
                result = _child.Update(context);
            }
            
            return result;

        }


    }

    public class RepeatUntilSuccessDecorator<TBlackboard> : Node<TBlackboard> where TBlackboard : IBlackboard
    {
        private readonly Node<TBlackboard> _child;

        public RepeatUntilSuccessDecorator(Node<TBlackboard> child)
            : base("RepeatUntilSuccess")
        {
            _child = child;
        }

        protected override Status OnUpdate(Context<TBlackboard> context, NodeContext<TBlackboard> nodeContext)
        {
            Status result;
            result = _child.Update(context);
            if (result == Status.Fail )//&& isAlreadyRunning)
            {
                result = _child.Update(context);
            }
            return result;

        }
    }
}
