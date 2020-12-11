using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace _03_fluent_api.Model
{
    public interface INode
    {
        int Index { get; }
        ILocation Location { get; }
    }

    public class Node : INode, IComparable<INode>, IComparable
    {
        public Node(ILocation location, int index)
        {
            this.Location = location;
            this.Index = index;
        }

        public ILocation Location { get; }
        public int Index { get; }

        #region IComparable / IComparable<INode> implementation

        public int CompareTo([AllowNull] INode other)
        {
            if (other == null)
            {
                return 1;
            }

            return (other.Index - this.Index); 
        }

        public int CompareTo(object obj)
        {
            var other = obj as INode;
            if (other == null)
                return 1;

            return this.CompareTo(other);
        }

        // Define the is greater than operator.
        public static bool operator >(Node operand1, INode operand2)
        {
            return operand1.CompareTo(operand2) == 1;
        }

        // Define the is less than operator.
        public static bool operator <(Node operand1, INode operand2)
        {
            return operand1.CompareTo(operand2) == -1;
        }

        // Define the is greater than or equal to operator.
        public static bool operator >=(Node operand1, INode operand2)
        {
            return operand1.CompareTo(operand2) >= 0;
        }

        // Define the is less than or equal to operator.
        public static bool operator <=(Node operand1, INode operand2)
        {
            return operand1.CompareTo(operand2) <= 0;
        }

        #endregion
    }

    public class NodeCollection : SortedSet<INode>
    {
        public INode this[int index] => this.ToList()[index];
    }
}
