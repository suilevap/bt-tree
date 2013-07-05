using System.Collections.Generic;
using System.Linq;

namespace BehaviourTree
{
    public class ExecutionContext<T>
    {
        internal T Data {get; private set;}
        private readonly Node<T> _root;

        private List<Node<T>> _currentPath;
        private List<Node<T>> _lastRunningPath;

        internal ExecutionContext(Node<T> root, T data)
        {
            Data = data;
            _currentPath = new List<Node<T>>();
            _lastRunningPath = new List<Node<T>>();
            _root = root;
        }

        public Status Tick()
        {
            var status = _root.Visit(this, _lastRunningPath.Any());

            if (status == Status.Running)
            {
                _lastRunningPath = _currentPath;
            }
            else
            {
                _lastRunningPath.Clear();
            }
            _currentPath.Clear();
            
            return status;
        }

        internal void PushVisitingNode(Node<T> node)
        {
            _currentPath.Add(node);
        }

        internal void PopVisitingNode()
        {
            _currentPath.RemoveAt(_currentPath.Count - 1);
        }

        internal Node<T> GetCurrentRunningNode()
        {
            return _lastRunningPath.ElementAt(_currentPath.Count);
        }

        public override string ToString()
        {
            return _lastRunningPath.ToString();
        }

    }
}
