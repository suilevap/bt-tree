using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BT
{
    internal class Context<T>
    {
        internal T ExecutionContext {get; private set;}
        private CompositeNode<T> _root;

        private Path<T> _currentPath;
        private Path<T> _lastRunningPath;

        private Stack<bool> _isNodeRunning;
        ///// <summary>
        ///// True if _lastPath.StartsWith(_currentPath)
        ///// </summary>
        //internal bool IsCurrentPathRunning {get; private set;}



        internal Context(CompositeNode<T> root, T executionContext)
        {
            ExecutionContext = executionContext;
            //IsCurrentPathRunning = false;
            _currentPath = new Path<T>();
            _lastRunningPath = new Path<T>();
            _isNodeRunning = new Stack<bool>(16);
            _root = root;
        }

        internal Status Run()
        {
            return _root.Execute(this);
        }

        internal void PushVisitingNode(int nodeIndex, CompositeNode<T> node)
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
            if (_isNodeRunning.Count != 0 && _isNodeRunning.Peek())
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
