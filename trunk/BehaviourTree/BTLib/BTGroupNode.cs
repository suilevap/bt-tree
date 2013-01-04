using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BT
{
    public abstract class BTGroupNode<T> : BTNode<T>
    {
        public BTGroupNode<T>[] Childs { get; protected set; }
        public string Name { get; protected set; }

        protected BTGroupNode(string name, params BTGroupNode<T>[] childs)
        {
            Name = name;
            Childs = childs;
        }

        public override string ToString()
        {
            StringBuilder sb =new StringBuilder();
            sb.Append(Name);
            sb.Append('(');
            foreach(BTGroupNode<T> node in Childs)
            {
                sb.AppendFormat("{0},", node.ToString());
            }
            sb.Append(')');
            return sb.ToString();
        }
    }
}
