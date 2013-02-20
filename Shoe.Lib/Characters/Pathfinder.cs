#region comments

/******************************************************************************************************
 * Original By: Anthony Cecchi
 * 
 * Modified By: Anthony Cecchi
 * 
 * Original Date:   21/01/2012 
 * 
 * Modified Date:   22/01/2012
 * 
 * Purpose: A* algorithm to control NPCs tracking player
 * 
 * Dependencies: 
 *      Map.cs
 *      
 * Variables:
 *      SearchNode Class:
 *          public Point Position
 *          public SearchNode[] Neighbors
 *          public SearchNode Parent
 *          public bool Walkable
 *          public bool InOpenList
 *          public bool InClosedList
 *          public float DistanceToGoal
 *          public float DistanceTraveled
 *      Pathfinder Class:
 *          private SearchNode[,] searchNodes
 *          private int levelWidth
 *          private int levelHeight
 *          private List<SearchNode> openList = new List<SearchNode>()
 *          private List<SearchNode> closedList = new List<SearchNode>()
 * Functions:
 *      Pathfinder Class:
 *          private float Heuristic(Point point1, Point point2)
 *          public Pathfinder(Map map)
 *          private void InitializeSearchNodes(Map map)
 *          private void ResetSearchNodes()
 *          private SearchNode FindBestNode()
 *          private List<Vector2> FindFinalPath(SearchNode startNode, SearchNode endNode)
 *          public List<Vector2> FindPath(Point startPoint, Point endPoint)
 * Comments:
 * ***************************************************************************************************/

#endregion

#region     include
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TiledLib;

#endregion

#region     Pathfinder

namespace Shoe.Lib.Characters
{

    #region     classes

    #region     SearchNode Class

    public class SearchNode
    {

        #region     variables

        public Point Position;

        public SearchNode[] Neighbors;

        public SearchNode Parent;

        public bool Walkable;
        public bool InOpenList;
        public bool InClosedList;

        public float DistanceToGoal;
        public float DistanceTraveled;

        #endregion

    }

    #endregion

    #region     Pathfinder Class

    public class Pathfinder
    {

        #region     variables

        private SearchNode[,] searchNodes;

        private int levelWidth;
        private int levelHeight;
        public  Vector2 remainder = Vector2.Zero;
          
        private List<SearchNode> openList = new List<SearchNode>();
        private List<SearchNode> closedList = new List<SearchNode>();

        #endregion

        #region     functions

        #region     Heuristic

        private float Heuristic(Point point1, Point point2)
        {

            return Math.Abs(point1.X - point2.X) +
                   Math.Abs(point1.Y - point2.Y);

        }

        #endregion

        #region     Pathfinder

        public Pathfinder(Map map)
        {

            levelWidth = map.Width;
            levelHeight = map.Height;
            InitializeSearchNodes(map);

        }

        #endregion

        #region     InitializeSearchNodes

        private void InitializeSearchNodes(Map map)
        {
            searchNodes = new SearchNode[levelWidth, levelHeight];

            #region     InitializeNodesArray
        TileLayer clipLayer = map.GetLayer("Clip") as TileLayer;
                   
            for (int x = 0; x < levelWidth; x++)
            {

                for (int y = 0; y < levelHeight; y++)
                {

                    SearchNode node = new SearchNode();

                    node.Position = new Point(x, y);
                     Tile currentNode = clipLayer.Tiles[x, y];
                    if (currentNode != null)
                        node.Walkable = false;
                    else
                        node.Walkable = true;

                    if (node.Walkable == true)
                    {

                        node.Neighbors = new SearchNode[4];
                        searchNodes[x, y] = node;

                    }

                }

            }

            #endregion

            #region     ConnectNeighbors

            for (int x = 0; x < levelWidth; x++)
            {

                for (int y = 0; y < levelHeight; y++)
                {

                    SearchNode node = searchNodes[x, y];

                    if (node == null || node.Walkable == false)
                    {

                        continue;

                    }

                    Point[] neighbors = new Point[]
                    {

                        new     Point   (x, y   -   1),
                        new     Point   (x, y   +   1),
                        new     Point   (x  -   1,  y),
                        new     Point   (x  +   1,  y),
                   

                    };

                    for (int i = 0; i < neighbors.Length; i++)
                    {

                        Point position = neighbors[i];

                        if (position.X < 0 || position.X > levelWidth - 1 ||
                            position.Y < 0 || position.Y > levelHeight - 1)
                        {

                            continue;

                        }

                        SearchNode neighbor = searchNodes[position.X, position.Y];

                        if (neighbor == null || neighbor.Walkable == false)
                        {

                            continue;

                        }

                        node.Neighbors[i] = neighbor;

                    }

                }

            }

            #endregion

        }

        #endregion

        #region     ResetSearchNodes

        private void ResetSearchNodes()
        {

            openList.Clear();
            closedList.Clear();

            for (int x = 0; x < levelWidth; x++)
            {

                for (int y = 0; y < levelHeight; y++)
                {

                    SearchNode node = searchNodes[x, y];

                    if (node == null)
                    {

                        continue;

                    }

                    node.InOpenList = false;
                    node.InClosedList = false;

                    node.DistanceTraveled = float.MaxValue;
                    node.DistanceToGoal = float.MaxValue;

                }

            }

        }

        #endregion

        #region     FindBestNode

        private SearchNode FindBestNode()
        {

            SearchNode currentTile = openList[0];

            float smallestDistanceToGoal = float.MaxValue;

            for (int i = 0; i < openList.Count; i++)
            {

                if (openList[i].DistanceToGoal < smallestDistanceToGoal)
                {

                    currentTile = openList[i];
                    smallestDistanceToGoal = currentTile.DistanceToGoal;

                }

            }

            return currentTile;

        }

        #endregion

        #region     FindFinalPath

        private List<Vector2> FindFinalPath(SearchNode startNode, SearchNode endNode)
        {

            closedList.Add(endNode);

            SearchNode parentTile = endNode.Parent;

            while (parentTile != startNode)
            {

                closedList.Add(parentTile);
                parentTile = parentTile.Parent;

            }

            List<Vector2> finalPath = new List<Vector2>();

            for (int i = closedList.Count - 1; i >= 0; i--)
            {

                finalPath.Add(new Vector2(closedList[i].Position.X * 64 +remainder .X ,
                                                closedList[i].Position.Y * 64 +remainder .Y  ));

            }

            return finalPath;

        }

        #endregion

        #region     FindPath

        public List<Vector2> FindPath(Point startPoint, Point endPoint)
        {
             remainder.X = startPoint.X % 64;
            remainder.Y = startPoint.Y % 64;
            startPoint.X = startPoint.X / 64;
            startPoint.Y = startPoint.Y / 64;
            endPoint.X = endPoint.X / 64;
            endPoint.Y = endPoint.Y / 64;

            if (startPoint == endPoint)
            {

                return new List<Vector2>();

            }

            ResetSearchNodes();

            SearchNode startNode = searchNodes[startPoint.X, startPoint.Y];
            SearchNode endNode = searchNodes[endPoint.X, endPoint.Y];
            

            
            startNode.InOpenList = true;

            startNode.DistanceToGoal = Heuristic(startPoint, endPoint);
            startNode.DistanceTraveled = 0;

            openList.Add(startNode);

            while (openList.Count > 0)
            {

                SearchNode currentNode = FindBestNode();

                if (currentNode == null)
                    break;

                if (currentNode == endNode)
                    return FindFinalPath(startNode, endNode);

                for (int i = 0; i < currentNode.Neighbors.Length; i++)
                {

                    SearchNode neighbor = currentNode.Neighbors[i];

                    if (neighbor == null || neighbor.Walkable == false)
                    {

                        continue;

                    }

                    float distanceTraveled = currentNode.DistanceTraveled + 1;

                    float heuristic = Heuristic(neighbor.Position, endPoint);

                    if (neighbor.InOpenList == false && neighbor.InClosedList == false)
                    {

                        neighbor.DistanceTraveled = distanceTraveled;
                        neighbor.DistanceToGoal = distanceTraveled + heuristic;
                        neighbor.Parent = currentNode;
                        neighbor.InOpenList = true;
                        openList.Add(neighbor);

                    }

                    else if (neighbor.InOpenList || neighbor.InClosedList)
                    {

                        if (neighbor.DistanceTraveled > distanceTraveled)
                        {

                            neighbor.DistanceTraveled = distanceTraveled;
                            neighbor.DistanceToGoal = distanceTraveled + heuristic;

                            neighbor.Parent = currentNode;

                        }

                    }

                }

                openList.Remove(currentNode);
                currentNode.InClosedList = true;

            }
            return new List<Vector2>();

        }

        #endregion

        #endregion

    }

    #endregion

    #endregion

}

#endregion