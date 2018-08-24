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
        private readonly Action<TBlackboard> _initFunc;
        private readonly Func<TBlackboard, Status> _conditionFunc;


        private readonly Node<TBlackboard> _child;

        public DecoratorCondtion(string name, Action<TBlackboard> initFunc, Func<TBlackboard, Status> checkFunc, Node<TBlackboard> child)
            : base(name)
        {
            _child = child;
            _initFunc = initFunc;
            _conditionFunc = checkFunc;
        }

        protected override Status OnUpdate(Context<TBlackboard> context, bool isAlreadyRunning)
        {
            Status result;
            if (!isAlreadyRunning)
            {
                if (_initFunc != null)
                {
                    _initFunc(context.Blackboard);
                }
            }
            result = _conditionFunc(context.Blackboard);
            if (result == Status.Running)
            {
                result = _child.Update(context, 0, isAlreadyRunning);
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

        protected override Status OnUpdate(Context<TBlackboard> context, bool isAlreadyRunning)
        {
            Status result;
            result = _child.Update(context, 0, isAlreadyRunning);
            if (result == Status.Fail )//&& isAlreadyRunning)
            {
                result = _child.Update(context, 0, false);
            }
            return result;

        }
    }
}
