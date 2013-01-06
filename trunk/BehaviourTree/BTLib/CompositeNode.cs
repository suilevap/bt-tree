﻿using System;
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

        protected abstract Status UpdateChilds(Context<T> context, int? runningNodeIndex);

        protected override Status Start(Context<T> context)
        {
            //Status status = UpdateChilds(context, null);
            //return status;
            return Status.Running;
        }

        protected override Status Tick(Context<T> context)
        {
            int? runningIndex = context.GetCurrentRunningChildIndex();
            //bool runningNodeExsists = context.TryGetCurrentRunningChildIndex(ref runningIndex);
            //if (!runningNodeExsists)
            //    throw new InvalidOperationException("Call Execute for not running node");

            Status status = UpdateChilds(context, runningIndex);
            return status;
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
