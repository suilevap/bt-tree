using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BT
{
    public class Context<T>
    {
        internal T ExecutionContext {get; private set;}
        private readonly Node<T> _root;

        private Path<T> _currentPath;
        private Path<T> _lastRunningPath;

        private readonly Stack<bool> _isNodeRunning;


        internal Context(Node<T> root, T executionContext)
        {
            ExecutionContext = executionContext;
            //IsCurrentPathRunning = false;
            _currentPath = new Path<T>();
            _lastRunningPath = new Path<T>();
            _isNodeRunning = new Stack<bool>(16);
            _root = root;
        }

        public Status Update()
        {
            Status status;
            bool isRunning = (_lastRunningPath.Count != 0);
            status = UpdateNode(0, _root, isRunning);

            if (status == Status.Running)
            {
                //swap
                var tmp = _lastRunningPath;
                _lastRunningPath = _currentPath;
                _currentPath = tmp;
            }
            else
            {
                _lastRunningPath.Clear();
            }
            _currentPath.Clear();
            _isNodeRunning.Clear();

            return status;
        }

        internal void PushVisitingNode(int nodeIndex, Node<T> node, bool isRunningNode)
        {
            _isNodeRunning.Push(isRunningNode);
            _currentPath.Add(nodeIndex, node);
        }

        internal void PopVisitingNode()
        {
            _isNodeRunning.Pop();
            _currentPath.RemoveLast();
        }
        internal int? GetCurrentRunningChildIndex()
        {
            int? result = null;
            if ((_isNodeRunning.Count == 0 || _isNodeRunning.Peek()) && _lastRunningPath.Count > _currentPath.Count)
            {
                result = _lastRunningPath.GetNodeIndex(_currentPath.Count);
            }
            return result;
        }
        internal bool TryGetCurrentRunningChildIndex(ref int index)
        {
            bool result = false;
            if ((_isNodeRunning.Count == 0 || _isNodeRunning.Peek()) && _lastRunningPath.Count > _currentPath.Count)
            {
                index = _lastRunningPath.GetNodeIndex(_currentPath.Count);
                result = true;
            }
            else
            {
                result = false;
            }
            return result;
        }

        internal Status UpdateNode(int index, Node<T> node, bool isRunning)
        {
            Status status;
            PushVisitingNode(index, node, isRunning);
            status = node.Update(this, isRunning);
            if (status != Status.Running)
            {
                PopVisitingNode();
            }
            return status;
        }


        public override string ToString()
        {
            return _lastRunningPath.ToString();
        }

        //internal int GetCurrentRunningNodeIndex()
        //{
        //    int currentRunningNodeIndex = -1;
        //    if (_isNodeRunning.Count != 0 && _isNodeRunning.Peek())
        //    {
        //        currentRunningNodeIndex = _lastRunningPath.GetNodeIndex(_currentPath.Count);
        //    }
        //    return currentRunningNodeIndex;
        //}
    }
}
