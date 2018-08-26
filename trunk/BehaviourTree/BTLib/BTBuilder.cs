using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BT
{
    /// <summary>
    /// Helper class to build BT tree
    /// </summary>
    /// <typeparam name="TBlackboard">Type of Blackboard</typeparam>
    public class BTBuilder<TBlackboard> where TBlackboard : IBlackboard
    {
        /// <summary>
        /// Get builder instance
        /// </summary>
        public static BTBuilder<TBlackboard> Instance = new BTBuilder<TBlackboard>();

        /// <summary>
        /// Create BT context, that should be used per agent
        /// </summary>
        /// <param name="root">BT root node</param>
        /// <param name="blackboard">Reference to Blackboard</param>
        /// <returns>Context</returns>
        public Context<TBlackboard> CreateContext(Node<TBlackboard> root, TBlackboard blackboard)
        {
            Context<TBlackboard> result = new Context<TBlackboard>(root, blackboard);
            return result;
        }
        /// <summary>
        /// Create BT Selector node
        /// </summary>
        /// <param name="name">Node name</param>
        /// <param name="childs">Variants</param>
        /// <returns>Node</returns>
        public Node<TBlackboard> Selector(string name, params Node<TBlackboard>[] childs)
        {
            //Selector<TBlackboard> node = new Selector<TBlackboard>(name, childs);
            Node<TBlackboard> node;
            if (childs.Length > 1)
            {
                node = new Selector<TBlackboard>(name, childs);
            }
            else
            {
                node = childs[0];
                //node.Name = string.Format("{0}-{1}",name, node.Name);
            }
            return node;
        }

        /// <summary>
        /// Create BT Sequence node
        /// </summary>
        /// <param name="name">Node name</param>
        /// <param name="childs">Sequence</param>
        /// <returns>Node</returns>
        public Node<TBlackboard> Sequence(string name, params Node<TBlackboard>[] childs)
        {
            
            Node<TBlackboard> node;
            if (childs.Length > 1)
            {
                node = new Sequence<TBlackboard>(name, childs);
            }
            else
            {
                node = childs[0];
                //node.Name = string.Format("{0}-{1}",name, node.Name);
            }
            return node;
        }

        /// <summary>
        /// Crate BT Condition
        /// </summary>
        /// <param name="name">Node name</param>
        /// <param name="condition">Functio that should return True in case of success</param>
        /// <returns>Node</returns>
        public Condition<TBlackboard> Condition(string name, Func<TBlackboard, bool> condition)
        {
            Condition<TBlackboard> node = new Condition<TBlackboard>(name, condition);
            return node;
        }

        /// <summary>
        /// Create simple action
        /// </summary>
        /// <param name="name">Node ame</param>
        /// <param name="startAction">Run on node start, should return True if success and node moves to Status.Running state</param>
        /// <returns>Node</returns>
        public SimpleAction<TBlackboard> Action(string name, Func<TBlackboard, NodeContext<TBlackboard>, bool> startAction)
        {
            SimpleAction<TBlackboard> node = new SimpleAction<TBlackboard>(name, startAction);
            return node;
        }

        /// <summary>
        /// Create simple action
        /// </summary>
        /// <param name="name">Node ame</param>
        /// <param name="actionStart">Run on node start, should return True if success and node moves to Status.Running state</param>
        /// <param name="checkInProgress">Check every node tick, should return True if node is still in Status.Running state, false - node execution is complete</param>
        /// <param name="actionExecute">Run every node tick</param>
        /// <param name="actionComplete">Run on node complete. Should return True in case of Ok, false in case of Fail</param>        
        /// <returns>Node</returns>
        public SimpleAction<TBlackboard> Action(string name, 
            Func<TBlackboard, NodeContext<TBlackboard>, bool> actionStart,
            Func<TBlackboard, NodeContext<TBlackboard>, bool> checkInProgress,
            Action<TBlackboard, NodeContext<TBlackboard>> executionAction)
        {
            SimpleAction<TBlackboard> node = new SimpleAction<TBlackboard>(name, actionStart, checkInProgress, executionAction);
            return node;
        }

        /// <summary>
        /// Create simple action
        /// </summary>
        /// <param name="name">Node ame</param>
        /// <param name="startAction">Run on node start, should return True if success and node moves to Status.Running state</param>
        /// <param name="executionAction">Run every node tick, should return True if node is still in Status.Running state, false - node execution is complete</param>
        /// <param name="completeAction">Run on node complete. Should return True in case of Ok, false in case of Fail</param>
        /// <returns>Node</returns>
        public SimpleAction<TBlackboard> Action(string name,
            Func<TBlackboard, NodeContext<TBlackboard>, bool> actionStart, 
            Func<TBlackboard, NodeContext<TBlackboard>, bool> checkInProgress, 
            Action<TBlackboard, NodeContext<TBlackboard>> executionAction, 
            Func<TBlackboard, NodeContext<TBlackboard>, bool> completeAction)
        {
            SimpleAction<TBlackboard> node = new SimpleAction<TBlackboard>(name, actionStart, checkInProgress, executionAction, completeAction);
            return node;
        }

        /// <summary>
        /// Create simple action
        /// </summary>
        /// <param name="name">Node ame</param>
        /// <param name="startAction">Run on node start, should return True if success and node moves to Status.Running state</param>
        /// <param name="executionAction">Run every node tick, should return True if node is still in Status.Running state, false - node execution is complete</param>
        /// <param name="completeAction">Run on node complete. </param>
        /// <returns>Node</returns>
        public SimpleAction<TBlackboard> Action(string name, 
            Func<TBlackboard, NodeContext<TBlackboard>, bool> actionStart,
            Func<TBlackboard, NodeContext<TBlackboard>, bool> checkInProgress, 
            Action<TBlackboard, NodeContext<TBlackboard>> executionAction, 
            Action<TBlackboard, NodeContext<TBlackboard>> completeAction)
        {
            SimpleAction<TBlackboard> node = new SimpleAction<TBlackboard>(name, actionStart, checkInProgress, executionAction,
                (x,c) =>
                {
                    completeAction(x,c);
                    return true;
                });
            return node;
        }


        /// <summary>
        /// Create Parallel All node
        /// </summary>
        /// <param name="name">Node name</param>
        /// <param name="childs">Variants</param>
        /// <returns>Node</returns>
        public ParallelAllActionNode<TBlackboard> All(string name, params ActionNode<TBlackboard>[] childs)
        {
            ParallelAllActionNode<TBlackboard> node = new ParallelAllActionNode<TBlackboard>(name, childs);
            return node;
        }

        /// <summary>
        /// Create While Node
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="checkFunc">Run child node if function return Status.Running</param>
        /// <param name="node">Child Node</param>
        /// <returns>Node</returns>
        public DecoratorCondtion<TBlackboard> While(string name, Action<TBlackboard, NodeContext<TBlackboard>> initFunc, Func<TBlackboard, NodeContext<TBlackboard>, Status> checkFunc, Node<TBlackboard> node)
        {
            return new DecoratorCondtion<TBlackboard>(name, initFunc, checkFunc, node);
        }

        /// <summary>
        /// Create node which repeat child until success of child node.
        /// </summary>
        /// <param name="node">Child node</param>
        /// <returns>Node</returns>
        public RepeatUntilSuccessDecorator<TBlackboard> UntilSuccess(Node<TBlackboard> node)
        {
            return new RepeatUntilSuccessDecorator<TBlackboard>(node);
        }

        /// <summary>
        /// Create Optional action node
        /// </summary>
        /// <param name="child">Optional action</param>
        /// <param name="startSuccessRequired">If False action will starts even if child.Start failed</param>
        /// <returns>ActionNode</returns>
        public ActionNode<TBlackboard> Optional(ActionNode<TBlackboard> child, bool startSuccessRequired)
        {
            ActionNode<TBlackboard> node = new OptionalActionNode<TBlackboard>(child, startSuccessRequired);
            return node;
        }

        /// <summary>
        /// Create Optional action node
        /// </summary>
        /// <param name="child">Optional action</param>
        /// <returns>ActionNode</returns>
        public ActionNode<TBlackboard> Optional(ActionNode<TBlackboard> child)
        {
            ActionNode<TBlackboard> node = new OptionalActionNode<TBlackboard>(child, false);
            return node;
        }


        
        /// <summary>
        /// Infinite empty action
        /// </summary>
        public ActionNode<TBlackboard> Idle = new SimpleAction<TBlackboard>("Idle", (x,c) => true, (x, c) => true, (x, c) => { });

    }
}
