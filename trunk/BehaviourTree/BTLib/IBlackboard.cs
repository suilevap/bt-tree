using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BT
{
    public interface IBlackboard
    {
        void Update(TimeSpan time);
        void Reset();

        void RunningActionChanged<T>(Node<T> runningNode, Path<T> path) where T : IBlackboard;
    }
}
