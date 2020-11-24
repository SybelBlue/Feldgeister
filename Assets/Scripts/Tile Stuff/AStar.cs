using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace AStarSharp
{
    class AStarRiver
    {
        private Vector2Int bottomLeftCorner, upperRightCorner, usableMapDimensions;
        private Template castleTemplate;
        private RegionManager<Building> usedSpaces;
        private GatedPerlinTile groundTile;
        private TileBase riverTile;

        public AStarRiver(
            Template castleTemplate, 
            Vector2Int usableMapDimensions,
            Vector2Int bottomLeftCorner, 
            Vector2Int upperRightCorner,
            RegionManager<Building> usedSpaces,
            GatedPerlinTile groundTile,
            TileBase riverTile)
        {
            this.castleTemplate = castleTemplate;
            this.bottomLeftCorner = bottomLeftCorner;
            this.upperRightCorner = upperRightCorner;
            this.usedSpaces = usedSpaces;
            this.groundTile = groundTile;
            this.riverTile = riverTile;
            this.usableMapDimensions = usableMapDimensions;
        }

        public void LoadRiver(Tilemap mainTilemap)
        {
            var grid = new List<List<Node>>();
            for (int i = bottomLeftCorner.x - 5; i < upperRightCorner.x + 5; i++)
            {
                var row = new List<Node>();
                for (int j = bottomLeftCorner.y - 5; j < upperRightCorner.y + 5; j++)
                {
                    row.Add(MakeMapNode(new Vector2Int(i, j)));
                }
                grid.Add(row);
            }
            var astar = new Astar(grid);

            var start = -castleTemplate.boundingBox.size / 2 - bottomLeftCorner + new Vector2Int(5, 5);
            var end = new Vector2Int(1, 1);

            var pathStack = astar.FindPath(start, end);

            if (pathStack != null) 
            {
                foreach (var item in pathStack)
                {
                    var pos = item.Position + bottomLeftCorner - new Vector2Int(5, 5);
                    mainTilemap.SetTile(pos.To3D(), riverTile);
                }
            }

            start = castleTemplate.boundingBox.size / 2 - bottomLeftCorner + new Vector2Int(4, 4);
            end = usableMapDimensions + new Vector2Int(8, 8);

            pathStack = astar.FindPath(start, end);
            if (pathStack != null) 
            {
                foreach (var item in pathStack)
                {
                    var pos = item.Position + bottomLeftCorner - new Vector2Int(5, 5);
                    mainTilemap.SetTile(pos.To3D(), riverTile);
                }
            }
        }

        private Node MakeMapNode(Vector2Int vec)
            => new Node(vec - bottomLeftCorner + new Vector2Int(5, 5), !usedSpaces.AnyContain(vec), 10 * groundTile.RawPerlinValue(vec.x, vec.y));
    }

    // https://github.com/davecusatis/A-Star-Sharp/blob/master/Astar.cs
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
            List<Node> OpenList = new List<Node>();
            List<Node> ClosedList = new List<Node>();
            List<Node> adjacencies;
            Node current = start;
            
            // add start node to Open List
            OpenList.Add(start);

            while(OpenList.Count != 0 && !ClosedList.Exists(x => x.Position == end.Position))
            {
                current = OpenList[0];
                OpenList.Remove(current);
                ClosedList.Add(current);
                adjacencies = GetAdjacentNodes(current);

                foreach(Node n in adjacencies)
                {
                    if (!ClosedList.Contains(n) && n.Walkable)
                    {
                        if (!OpenList.Contains(n))
                        {
                            n.Parent = current;
                            n.DistanceToTarget = Math.Abs(n.Position.x - end.Position.x) + Math.Abs(n.Position.y - end.Position.y);
                            n.Cost = n.Weight + n.Parent.Cost;
                            OpenList.Add(n);
                            OpenList = OpenList.OrderBy(node => node.F).ToList<Node>();
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

            int row = n.Position.y;
            int col = n.Position.x;

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