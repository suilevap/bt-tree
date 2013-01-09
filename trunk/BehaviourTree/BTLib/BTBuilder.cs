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
    public class BTBuilder<TBlackboard>
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
        public Selector<TBlackboard> Selector(string name, params Node<TBlackboard>[] childs)
        {
            Selector<TBlackboard> node = new Selector<TBlackboard>(name, childs);
            return node;
        }

        /// <summary>
        /// Create BT Sequence node
        /// </summary>
        /// <param name="name">Node name</param>
        /// <param name="childs">Sequence</param>
        /// <returns>Node</returns>
        public Sequence<TBlackboard> Sequence(string name, params Node<TBlackboard>[] childs)
        {
            Sequence<TBlackboard> node = new Sequence<TBlackboard>(name, childs);
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
        public SimpleAction<TBlackboard> Action(string name, Func<TBlackboard, bool> startAction)
        {
            SimpleAction<TBlackboard> node = new SimpleAction<TBlackboard>(name, startAction);
            return node;
        }

        /// <summary>
        /// Create simple action
        /// </summary>
        /// <param name="name">Node ame</param>
        /// <param name="startAction">Run on node start, should return True if success and node moves to Status.Running state</param>
        /// <param name="executionAction">Run every node tick, should return True if node is still in Status.Running state, false - node execution is complete</param>
        /// <returns>Node</returns>
        public SimpleAction<TBlackboard> Action(string name, Func<TBlackboard, bool> startAction, Func<TBlackboard, bool> executionAction)
        {
            SimpleAction<TBlackboard> node = new SimpleAction<TBlackboard>(name, startAction, executionAction);
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
        public SimpleAction<TBlackboard> Action(string name, Func<TBlackboard, bool> startAction, Func<TBlackboard, bool> executionAction, Func<TBlackboard, bool> completeAction)
        {
            SimpleAction<TBlackboard> node = new SimpleAction<TBlackboard>(name, startAction, executionAction, completeAction);
            return node;
        }
 
    }
}
