using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BT
{
    /// <summary>
    /// Condtion node, return Ok if condtion function is true, else Fail status
    /// </summary>
    /// <typeparam name="TBlackboard"></typeparam>
    public class Condition<TBlackboard> : Node<TBlackboard> where TBlackboard : IBlackboard
    {
        private readonly Func<TBlackboard, bool> _condition;

        internal Condition(string name, Func<TBlackboard, bool> condition)
            :base(name)
        {
            _condition = condition;
        }

        protected virtual bool CheckCondtion(TBlackboard blackboard)
        {
            bool result;
            if (_condition != null)
            {
                result = _condition(blackboard);
            }
            else
            {
                result = false;
            }
            return result;
        }

        protected override Status OnUpdate(Context<TBlackboard> context, NodeContext<TBlackboard> nodeContext)
        {
            bool check = CheckCondtion(context.Blackboard);
            Status status;
            if (check)
            {
                status = Status.Ok;
            }
            else
            {
                status = Status.Fail;
            }
            return status;
        }

    }
}
