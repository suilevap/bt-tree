using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BT
{
    public abstract class CompositeNode<T> : Node<T>
    {
        public Node<T>[] Childs { get; protected set; }

        protected CompositeNode(string name, params Node<T>[] childs)
            :base(name)
        {
            Childs = childs;
        }


        public override string ToString()
        {
            StringBuilder sb =new StringBuilder();
            sb.Append(base.ToString());
            sb.Append('(');
            foreach(Node<T> node in Childs)
            {
                sb.AppendFormat("{0},", node.ToString());
            }
            sb.Append(')');
            return sb.ToString();
        }
    }
}
