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

        public Status Tick()
        {
            Status status;
            if (_lastRunningPath.Count != 0)
            {
                //continue
                status = _root.Execute(this);
            }
            else
            {
                //start
                status = _root.Start(this);
            }

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

        internal void PushVisitingNode(int nodeIndex, Node<T> node)
        {
            int runningIndex = -1;
            bool isCurrentPathRuning = TryGetCurrentRunningChildIndex(ref runningIndex);
            bool nodeRunning = isCurrentPathRuning && (nodeIndex == runningIndex);
            _isNodeRunning.Push(nodeRunning);
            _currentPath.Add(nodeIndex, node);
        }

        internal void PopVisitingNode()
        {
            _isNodeRunning.Pop();
            _currentPath.RemoveLast();
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

        internal Status StartNode(int index, Node<T> node)
        {
            Status status;
            PushVisitingNode(index, node);
            status = node.Start(this);
            if (status == Status.Running)
            {
                status = node.Tick(this);
            }
            if (status != Status.Running)
            {
                PopVisitingNode();
            }
            return status;
        }
        internal Status ExecuteRunningNode(int index, Node<T> node)
        {
            Status status;
            PushVisitingNode(index, node);

            status = node.Tick(this);
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
