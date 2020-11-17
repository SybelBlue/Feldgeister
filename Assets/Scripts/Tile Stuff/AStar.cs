using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// combined two articles 
// first on AStar which lacked priority queue
// other on priority queue which lacked AStar

// https://github.com/davecusatis/A-Star-Sharp/blob/master/Astar.cs
namespace AStarSharp
{
    public class Node
    {
        public Node Parent;
        public Vector2Int Position;
        public float DistanceToTarget;
        public float Cost;
        public float Weight;
        public float F
        {
            get
            {
                if (DistanceToTarget != -1 && Cost != -1)
                    return DistanceToTarget + Cost;
                else
                    return -1;
            }
        }
        public bool Walkable;

        public Node(Vector2Int pos, bool walkable, float weight = 1)
        {
            Parent = null;
            Position = pos;
            DistanceToTarget = -1;
            Cost = 1;
            Weight = weight;
            Walkable = walkable;
        }
    }

    public class PriorityNode : Node, IPrioritizable
    {
        public float Priority { get; set; }
        public PriorityNode(Vector2Int pos, bool walkable, float priority, float weight = 1) : base(pos, walkable, weight)
        {
            Priority = priority;
        }
        
        public PriorityNode(Node node, float priority) : this(node.Position, node.Walkable, priority, node.Weight)
        { }
    }

    public class Astar
    {
        List<List<Node>> Grid;
        public int GridRows
        {
            get
            {
               return Grid[0].Count;
            }
        }
        public int GridCols
        {
            get
            {
                return Grid.Count;
            }
        }

        public Astar(List<List<Node>> grid)
        {
            Grid = grid;
        }

        public Stack<Node> FindPath(Vector2Int Start, Vector2Int End)
        {
            Node start = new Node(Start, true);
            Node end = new Node(End, true);

            Stack<Node> Path = new Stack<Node>();
            PriorityQueue<PriorityNode> OpenList = new PriorityQueue<PriorityNode>();
            List<Node> ClosedList = new List<Node>();
            List<Node> adjacencies;
            Node current = start;
           
            // add start node to Open List
            OpenList.Enqueue(new PriorityNode(start, 1));

            while(OpenList.Count != 0 && !ClosedList.Exists(x => x.Position == end.Position))
            {
                current = OpenList.Dequeue();
                ClosedList.Add(current);
                adjacencies = GetAdjacentNodes(current);

 
                foreach(Node n in adjacencies)
                {
                    if (!ClosedList.Contains(n) && n.Walkable)
                    {
                        if (!OpenList.Any(pn => pn.Position == n.Position))
                        {
                            n.Parent = current;
                            n.DistanceToTarget = Math.Abs(n.Position.x - end.Position.x) + Math.Abs(n.Position.y - end.Position.y);
                            n.Cost = n.Weight + n.Parent.Cost;
                            OpenList.Enqueue(new PriorityNode(n, n.F));
                        }
                    }
                }
            }
            
            // construct path, if end was not closed return null
            if(!ClosedList.Exists(x => x.Position == end.Position))
            {
                return null;
            }

            // if all good, return path
            Node temp = ClosedList[ClosedList.IndexOf(current)];
            if (temp == null) return null;
            do
            {
                Path.Push(temp);
                temp = temp.Parent;
            } while (temp != start && temp != null) ;
            return Path;
        }
		
        private List<Node> GetAdjacentNodes(Node n)
        {
            List<Node> temp = new List<Node>();

            int row = (int)n.Position.y;
            int col = (int)n.Position.x;

            if(row + 1 < GridRows)
            {
                temp.Add(Grid[col][row + 1]);
            }
            if(row - 1 >= 0)
            {
                temp.Add(Grid[col][row - 1]);
            }
            if(col - 1 >= 0)
            {
                temp.Add(Grid[col - 1][row]);
            }
            if(col + 1 < GridCols)
            {
                temp.Add(Grid[col + 1][row]);
            }

            return temp;
        }
    }
}

// https://medium.com/@basilin/priority-queue-with-c-7089f4898c8d
namespace AStarSharp
{
    public interface IPrioritizable
    {
        /// <summary>
        /// Priority of the item.
        /// </summary>
        float Priority { get; set; }
    }
    
    public sealed class PriorityQueue<TEntry> where TEntry : IPrioritizable
    {
        public LinkedList<TEntry> Entries { get; } = new LinkedList<TEntry>();

        public int Count
            => Entries.Count;

        public TEntry Dequeue()
        {
            if (Entries.Any())
            {
                var itemTobeRemoved = Entries.First.Value;
                Entries.RemoveFirst();
                return itemTobeRemoved;
            }

            return default(TEntry);
        }

        public void Enqueue(TEntry entry)
        {
            var value = new LinkedListNode<TEntry>(entry);
            if (Entries.First == null)
            {
                Entries.AddFirst(value);
            }
            else
            {
                var ptr = Entries.First;
                while (ptr.Next != null && ptr.Value.Priority < entry.Priority)
                {
                    ptr = ptr.Next;
                }

                if (ptr.Value.Priority <= entry.Priority)
                {
                    Entries.AddAfter(ptr, value);
                }
                else
                {
                    Entries.AddBefore(ptr, value);
                }
            }
        }

        public bool Any(Func<TEntry, bool> pred)
            => Entries.Any(pred);
    }
}