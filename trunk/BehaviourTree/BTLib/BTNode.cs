using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BT
{
    public abstract class BTNode
    {
        public BTNode[] Childs { get; protected set; }
        public string Name { get; protected set; }

        protected BTNode(string name, params BTNode[] childs)
        {
            Name = name;
            Childs = childs;
        }

        internal abstract BTStatus Execute(BTContext context, bool currentPathRunning);

        public override string ToString()
        {
            StringBuilder sb =new StringBuilder();
            sb.Append(Name);
            sb.Append('(');
            foreach(BTNode node in Childs)
            {
                sb.AppendFormat("{0},", node.ToString());
            }
            sb.Append(')');
            return sb.ToString();
        }
    }
}
