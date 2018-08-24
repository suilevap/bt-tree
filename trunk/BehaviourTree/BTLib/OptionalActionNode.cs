using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BT
{
    /// <summary>
    /// Optionla action. always in progress
    /// </summary>
    /// <typeparam name="TBlackboard">Type of using Blackboard</typeparam>
    public class OptionalActionNode<TBlackboard> : ActionNode<TBlackboard> where TBlackboard : IBlackboard
    {
        private ActionNode<TBlackboard> _child;
        private bool _startSuccessRequired;

        public OptionalActionNode(ActionNode<TBlackboard> child, bool startSuccessRequired)
            :base("Optional_"+child.Name)
        {
            _child = child;
            _startSuccessRequired = startSuccessRequired;
        }

        protected internal override bool Start(TBlackboard blackboard)
        {
            
            bool result;
            bool startResult = _child.Start(blackboard);

            if (_startSuccessRequired)
            {
                result = startResult;
            }
            else
            {
                result = true;
            }
            return result;
        }

        protected internal override bool IsInProgress(TBlackboard blackboard)
        {
            return true;
        }

        protected internal override bool Complete(TBlackboard blackboard)
        {
            return true;
        }

        protected internal override void Tick(TBlackboard blackboard)
        {
            if (_child.IsInProgress(blackboard))
            {
                _child.Tick(blackboard);
            }
        }
    }
}
