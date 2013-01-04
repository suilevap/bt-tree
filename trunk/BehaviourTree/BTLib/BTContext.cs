using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BT
{
    internal class BTContext
    {
        private BTNode _root;

        internal BTPath CurrentPath;
        internal BTPath LastPath;
        internal bool IsCurrentPathRunning;

        internal BTContext(BTNode root)
        {
            _root = root;
        }

        internal BTStatus Run()
        {
            return _root.Execute(this);
        }
    }
}
