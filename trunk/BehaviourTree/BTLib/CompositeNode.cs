using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BT
{
    public abstract class CompositeNode<T> : Node<T>
    {
        public CompositeNode<T>[] Childs { get; protected set; }
        public string Name { get; protected set; }

        protected CompositeNode(string name, params CompositeNode<T>[] childs)
        {
            Name = name;
            Childs = childs;
        }

        public override string ToString()
        {
            StringBuilder sb =new StringBuilder();
            sb.Append(Name);
            sb.Append('(');
            foreach(CompositeNode<T> node in Childs)
            {
                sb.AppendFormat("{0},", node.ToString());
            }
            sb.Append(')');
            return sb.ToString();
        }
    }
}
