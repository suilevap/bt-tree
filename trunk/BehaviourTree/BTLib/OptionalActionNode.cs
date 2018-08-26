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

        protected internal override bool Start(TBlackboard blackboard, NodeContext<TBlackboard> nodeContext)
        {
            
            bool result;
            bool startResult = _child.Start(blackboard, nodeContext);

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

        protected internal override bool IsInProgress(TBlackboard blackboard, NodeContext<TBlackboard> nodeContext)
        {
            return true;
        }

        protected internal override bool Complete(TBlackboard blackboard, NodeContext<TBlackboard> nodeContext)
        {
            return true;
        }

        protected internal override void Tick(TBlackboard blackboard, NodeContext<TBlackboard> nodeContext)
        {
            if (_child.IsInProgress(blackboard, nodeContext))
            {
                _child.Tick(blackboard, nodeContext);
            }
        }
    }
}
