using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BT
{
    public class BTBuilder<T>
    {
        public static BTBuilder<T> Instance = new BTBuilder<T>();

        public Context<T> CreateContext(Node<T> root, T executionContext)
        {
            Context<T> result = new Context<T>(root, executionContext);
            return result;
        }

        public Selector<T> Selector(string name, params Node<T>[] childs)
        {
            Selector<T> node = new Selector<T>(name, childs);
            return node;
        }

        public Sequence<T> Sequence(string name, params Node<T>[] childs)
        {
            Sequence<T> node = new Sequence<T>(name, childs);
            return node;
        }

        public Condition<T> Condition(string name, Func<T, bool> condition)
        {
            Condition<T> node = new Condition<T>(name, condition);
            return node;
        }

        public SimpleAction<T> Action(string name, Func<T, Status> startAction)
        {
            SimpleAction<T> node = new SimpleAction<T>(name, startAction);
            return node;
        }

        public SimpleAction<T> Action(string name, Func<T, Status> startAction, Func<T, Status> executionAction)
        {
            SimpleAction<T> node = new SimpleAction<T>(name, startAction, executionAction);
            return node;
        }
        public SimpleAction<T> Action(string name, Func<T, Status> startAction, Func<T, Status> executionAction, Func<T, Status> completeAction)
        {
            SimpleAction<T> node = new SimpleAction<T>(name, startAction, executionAction, completeAction);
            return node;
        }
        public SimpleAction<T> Action(string name, Func<T, Status> startAction, Func<T, Status> executionAction, Func<T, Status> completeAction, Action<T> abortAction)
        {
            SimpleAction<T> node = new SimpleAction<T>(name, startAction, executionAction, completeAction, abortAction);
            return node;
        }
    }
}
