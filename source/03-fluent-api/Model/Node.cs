using System;
using System.Collections.Generic;

namespace _03_fluent_api.Model
{
    public interface INode
    {
        int Index { get; }
        ILocation Location { get; }
    }

    public class Node : INode
    {
        public Node(ILocation location, int index)
        {
            this.Location = location;
            this.Index = index;
        }

        public ILocation Location { get; }
        public int Index { get; }
    }

    public class NodeCollection : SortedSet<INode>
    {
    }
}
