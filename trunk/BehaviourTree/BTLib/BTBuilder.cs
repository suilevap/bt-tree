using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BT
{
    public class BTBuilder<T>
    {
        /// <summary>
        /// Get builder instance
        /// </summary>
        public static BTBuilder<T> Instance = new BTBuilder<T>();

        /// <summary>
        /// Create BT context, that should be used per agent
        /// </summary>
        /// <param name="root">BT root node</param>
        /// <param name="executionContext">Action and condtion context</param>
        /// <returns>Context</returns>
        public Context<T> CreateContext(Node<T> root, T executionContext)
        {
            Context<T> result = new Context<T>(root, executionContext);
            return result;
        }
        /// <summary>
        /// Create BT Selector node
        /// </summary>
        /// <param name="name">Node name</param>
        /// <param name="childs">Variants</param>
        /// <returns>Node</returns>
        public Selector<T> Selector(string name, params Node<T>[] childs)
        {
            Selector<T> node = new Selector<T>(name, childs);
            return node;
        }

        /// <summary>
        /// Create BT Sequence node
        /// </summary>
        /// <param name="name">Node name</param>
        /// <param name="childs">Sequence</param>
        /// <returns>Node</returns>
        public Sequence<T> Sequence(string name, params Node<T>[] childs)
        {
            Sequence<T> node = new Sequence<T>(name, childs);
            return node;
        }

        /// <summary>
        /// Crate BT Condition
        /// </summary>
        /// <param name="name">Node name</param>
        /// <param name="condition">Functio that should return True in case of success</param>
        /// <returns>Node</returns>
        public Condition<T> Condition(string name, Func<T, bool> condition)
        {
            Condition<T> node = new Condition<T>(name, condition);
            return node;
        }

        /// <summary>
        /// Create simple action
        /// </summary>
        /// <param name="name">Node ame</param>
        /// <param name="startAction">Run on node start, should return True if success and node moves to Status.Running state</param>
        /// <returns>Node</returns>
        public SimpleAction<T> Action(string name, Func<T, bool> startAction)
        {
            SimpleAction<T> node = new SimpleAction<T>(name, startAction);
            return node;
        }

        /// <summary>
        /// Create simple action
        /// </summary>
        /// <param name="name">Node ame</param>
        /// <param name="startAction">Run on node start, should return True if success and node moves to Status.Running state</param>
        /// <param name="executionAction">Run every node tick, should return True if node is still in Status.Running state, false - node execution is complete</param>
        /// <returns>Node</returns>
        public SimpleAction<T> Action(string name, Func<T, bool> startAction, Func<T, bool> executionAction)
        {
            SimpleAction<T> node = new SimpleAction<T>(name, startAction, executionAction);
            return node;
        }

        /// <summary>
        /// Create simple action
        /// </summary>
        /// <param name="name">Node ame</param>
        /// <param name="startAction">Run on node start, should return True if success and node moves to Status.Running state</param>
        /// <param name="executionAction">Run every node tick, should return True if node is still in Status.Running state, false - node execution is complete</param>
        /// <param name="completeAction">Run on node complete. Should return True in case of Statu</param>
        /// <returns>Node</returns>
        public SimpleAction<T> Action(string name, Func<T, bool> startAction, Func<T, bool> executionAction, Func<T, bool> completeAction)
        {
            SimpleAction<T> node = new SimpleAction<T>(name, startAction, executionAction, completeAction);
            return node;
        }
 
    }
}
