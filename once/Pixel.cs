using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Once
{
    class Pixelclass
    {
        public static Texture2D Pixel;
        public static SpriteFont Font;
        public static SpriteFont Tfont;
        public static ContentManager Content;
    }

    //foreach (Node neighbour in neighbournodes)
    //            {
    //                if (closedset.Contains(neighbour))// || openset.Contains(neighbour))
    //                    continue;
    //
    //                double tentativeGscore = current.G_Score + Heuristic(current, neighbour);//current.G_Score + 1;
    //
    //
    //                if (!openset.Contains(neighbour))
    //                {
    //                    AddNode(neighbour, openset);
    //neighbour.Floor = FloorType.Open;
    //                }
    //                else if (tentativeGscore >= neighbour.G_Score)
    //                    continue;
    //
    //                neighbour.CameFrom = current;
    //                neighbour.G_Score = tentativeGscore;
    //                neighbour.H_Score = Heuristic(neighbour, goalN);
    //            }

}

//using System.Collections.Generic;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using Microsoft.Xna.Framework.Input;
//using System;
//using System.Diagnostics;
//
//namespace Astar
//{
//    class Map
//    {
//        public struct Settings
//        {
//            public enum Algorithm
//            {
//                Lee = 1,
//                Dijkstra = 2,
//                Astar = 3
//            }
//            public enum Heuristic
//            {
//                Manhattan = 1,
//                Euclidean = 2,
//                Refined = 3,
//                Greedy = 4
//            }
//            public enum DataStructure
//            {
//                InsertionSort = 1,
//                LinearSearch = 2
//            }
//        }
//
//
//        private Settings.Algorithm s_algorithm = Settings.Algorithm.Astar;
//        private Settings.Heuristic s_heuristic = Settings.Heuristic.Manhattan;
//        private Settings.DataStructure s_dataStructure = Settings.DataStructure.LinearSearch;
//
//        private Node[,] nodes;
//        private int nodesX, nodesY;
//
//        private Node start;
//        private Node goal;
//
//        private List<Texture2D> txrList;
//
//        // Astar Variables
//        private List<Node> closedset = new List<Node>();        // The set of nodes already evaluated.
//        private List<Node> openset = new List<Node>();          // The set of tentative nodes to be evaluated, initially containing the start node
//        private List<Node> finalPath = new List<Node>();        // The list which holds the final path of tiles
//
//        private List<Node> camefrom = new List<Node>();         // The map of navigated nodes.
//        private List<Node> neighbournodes = new List<Node>();   // The list which holds the neighbours of the current tile
//
//        public bool MapChanged { get; private set; }
//
//        private Stopwatch stopWatch;
//        private long time;
//        public long Time { get { return time; } }
//
//        private bool inProgress = false;
//
//        public string[] GetSettings
//        {
//            get
//            {
//                string[] s = new string[3];
//
//                s[0] = s_algorithm.ToString();
//                s[1] = s_heuristic.ToString();
//                s[2] = s_dataStructure.ToString();
//
//                return s;
//            }
//        }
//
//        public int[] PathSize
//        {
//            get
//            {
//                int[] paths = new int[3];
//
//                paths[0] = openset.Count;
//                paths[1] = closedset.Count;
//                paths[2] = finalPath.Count;
//
//                return paths;
//            }
//        }
//
//        /// <summary>
//        /// Calls the nodes
//        /// </summary>
//        /// <param name="TxrList">The list of textures</param>
//        /// <param name="gridsizeX">The amount of Nodes in the X-axis</param>
//        /// <param name="gridsizeY">The amount of Nodes in the Y-axis</param>
//        public Map(List<Texture2D> TxrList, int gridsizeX, int gridsizeY, int size)
//        {
//            nodesX = gridsizeX;
//            nodesY = gridsizeY;
//
//            nodes = new Node[nodesX, nodesY];
//
//            for (int x = 0; x < nodesX; x++)
//            {
//                for (int y = 0; y < nodesY; y++)
//                {
//                    nodes[x, y] = new Node(x, y, size);
//                }
//            }
//
//            start = nodes[1, 1]; // Start node
//            goal = nodes[nodesX - 2, nodesY - 2]; // Goal node
//            goal.Floor = FloorType.Goal;
//
//            txrList = TxrList;
//
//            MapChanged = true;
//
//            stopWatch = new Stopwatch();
//        }
//
//        /// <summary>
//        /// Updates the nodes
//        /// </summary>
//        /// <param name="currMouse">The current state of the mouse</param>
//        /// <param name="oldMouse">The old state of the mouse</param>
//        public void Update(MouseState currMouse, MouseState oldMouse, KeyboardState key, KeyboardState oldKey)
//        {
//            ModifyWall(currMouse);
//
//            if ((currMouse.MiddleButton == ButtonState.Pressed && oldMouse.MiddleButton == ButtonState.Released) ||
//                (key.IsKeyDown(Keys.Enter) && oldKey.IsKeyUp(Keys.Enter)))
//            {
//                if (!inProgress)
//                {
//                    ClearMap();
//
//                    stopWatch.Reset();
//
//                    stopWatch.Start();
//                    FindPath(start, goal);
//                    stopWatch.Stop();
//
//                    time = stopWatch.ElapsedMilliseconds;
//
//                    MapChanged = false;
//                }
//            }
//
//            if (key.IsKeyDown(Keys.Space) && oldKey.IsKeyUp(Keys.Space))
//            {
//                StepPath(start, goal);
//            }
//            if (key.IsKeyDown(Keys.Back) && oldKey.IsKeyUp(Keys.Back))
//            {
//                ClearMap();
//                inProgress = false;
//            }
//
//            ChangeSetting<Settings.Heuristic>(key, oldKey, Settings.Heuristic.Manhattan, Keys.NumPad1, Keys.D1);
//            ChangeSetting<Settings.Heuristic>(key, oldKey, Settings.Heuristic.Euclidean, Keys.NumPad2, Keys.D2);
//            ChangeSetting<Settings.Heuristic>(key, oldKey, Settings.Heuristic.Refined, Keys.NumPad3, Keys.D3);
//            ChangeSetting<Settings.Heuristic>(key, oldKey, Settings.Heuristic.Greedy, Keys.NumPad4, Keys.D4);
//            ChangeSetting<Settings.DataStructure>(key, oldKey, Settings.DataStructure.LinearSearch, Keys.NumPad7, Keys.D7);
//            ChangeSetting<Settings.DataStructure>(key, oldKey, Settings.DataStructure.InsertionSort, Keys.NumPad8, Keys.D8);
//        }
//
//        #region ModifyWall
//        /// <summary>
//        /// Creates Wall or Floor tiles
//        /// </summary>
//        /// <param name="currMouse">The current state of the mouse</param>
//        /// <param name="oldMouse">The old state of the mouse</param>
//        private void ModifyWall(MouseState currMouse)
//        {
//            /* Place a wall */
//            if (currMouse.LeftButton == ButtonState.Pressed)
//            {
//                for (int x = 0; x < nodesX; x++)
//                {
//                    for (int y = 0; y < nodesY; y++)
//                    {
//                        if (nodes[x, y].Rect.Contains(new Point(currMouse.X, currMouse.Y)))
//                        {
//                            if (nodes[x, y].Walkable == true)
//                            {
//                                nodes[x, y].Walkable = false;
//                                nodes[x, y].Floor = FloorType.Wall;
//                                MapChanged = true;
//                            }
//                        }
//                    }
//                }
//            }
//            /* Remove the wall */
//            if (currMouse.RightButton == ButtonState.Pressed)
//            {
//                for (int x = 0; x < nodesX; x++)
//                {
//                    for (int y = 0; y < nodesY; y++)
//                    {
//                        if (nodes[x, y].Rect.Contains(new Point(currMouse.X, currMouse.Y)))
//                        {
//                            if (nodes[x, y].Walkable == false)
//                            {
//                                nodes[x, y].Walkable = true;
//                                nodes[x, y].Floor = FloorType.Floor;
//                                MapChanged = true;
//                            }
//                        }
//                    }
//                }
//            }
//        }
//        #endregion
//
//        /// <summary>
//        /// The algorithm to find the shortest path from the start to the goal
//        /// </summary>
//        /// <param name="start">The starting Node</param>
//        /// <param name="goalN">The goal Node</param>
//        /// <param name="grid">The Map grid</param>
//        private void FindPath(Node startN, Node goalN, Settings.Algorithm a = 0, Settings.Heuristic h = 0, Settings.DataStructure d = 0)
//        {
//            Node current;
//
//            if (inProgress == false)
//            {
//                if (a != 0)
//                    s_algorithm = a;
//                if (h != 0)
//                    s_heuristic = h;
//                if (d != 0)
//                    s_dataStructure = d;
//
//                current = startN; // Start off by making the current node the starting node
//
//                if (current.Walkable == true)
//                {
//                    AddNode(current, openset);
//                    current.Floor = FloorType.Open;
//                    current.G_Score = 0;
//                    current.H_Score = Heuristic(current, goalN);
//                }
//                inProgress = true;
//            }
//
//            while (openset.Count != 0)
//            {
//                current = ReturnLowest(openset); // The node in openset having the lowest F_Score value becomes the current
//
//                if (current.PointLocation == goalN.PointLocation)
//                {
//                    // The search is complete. The path has been found.
//                    finalPath = ReconstructPath(current);
//                    inProgress = false;
//                    break;
//                }
//
//                openset.Remove(current); // Remove current from openset
//                AddNode(current, closedset);
//                current.Floor = FloorType.Closed;
//
//                OrganiseNeighbours(current, neighbournodes);
//
//                foreach (Node neighbour in neighbournodes)
//                {
//                    if (closedset.Contains(neighbour))// || openset.Contains(neighbour))
//                        continue;
//
//                    double tentativeGscore = current.G_Score + Heuristic(current, neighbour);//current.G_Score + 1;
//
//
//                    if (!openset.Contains(neighbour))
//                    {
//                        AddNode(neighbour, openset);
//                        neighbour.Floor = FloorType.Open;
//                    }
//                    else if (tentativeGscore >= neighbour.G_Score)
//                        continue;
//
//                    neighbour.CameFrom = current;
//                    neighbour.G_Score = tentativeGscore;
//                    neighbour.H_Score = Heuristic(neighbour, goalN);
//                }
//                neighbournodes.Clear();
//            }
//            if (openset.Count == 0)
//            {
//                // The search is complete. The path has not been found.
//                inProgress = false;
//            }
//
//            inProgress = false;
//        }
//
//        private void StepPath(Node startN, Node goalN, Settings.Algorithm a = 0, Settings.Heuristic h = 0, Settings.DataStructure d = 0)
//        {
//            Node current;
//
//            if (inProgress == false)
//            {
//                if (a != 0)
//                    s_algorithm = a;
//                if (h != 0)
//                    s_heuristic = h;
//                if (d != 0)
//                    s_dataStructure = d;
//
//                current = startN; // Start off by making the current node the starting node
//
//                if (current.Walkable == true)
//                {
//                    AddNode(current, openset);
//                    current.Floor = FloorType.Open;
//                    current.G_Score = 0;
//                    current.H_Score = Heuristic(current, goalN);
//                }
//                inProgress = true;
//            }
//
//            if (openset.Count != 0)
//            {
//                current = ReturnLowest(openset); // The node in openset having the lowest F_Score value becomes the current
//
//                if (current.PointLocation == goalN.PointLocation)
//                {
//                    // The search is complete. The path has been found.
//                    finalPath = ReconstructPath(current);
//                    inProgress = false;
//                    return;
//                }
//
//                openset.Remove(current); // Remove current from openset
//                AddNode(current, closedset);
//                current.Floor = FloorType.Closed;
//
//                OrganiseNeighbours(current, neighbournodes);
//
//                foreach (Node neighbour in neighbournodes)
//                {
//                    if (closedset.Contains(neighbour))// || openset.Contains(neighbour))
//                        continue;
//
//                    if (!openset.Contains(neighbour))
//                    {
//                        neighbour.CameFrom = current;
//                        neighbour.G_Score = current.G_Score + 1;
//                        neighbour.H_Score = Heuristic(neighbour, goalN);
//
//                        AddNode(neighbour, openset);
//                        neighbour.Floor = FloorType.Open;
//                    }
//                }
//                neighbournodes.Clear();
//            }
//            if (openset.Count == 0)
//            {
//                // The search is complete. The path has not been found.
//                inProgress = false;
//            }
//        }
//
//        #region ClearMap
//        /// <summary>
//        /// Clears the nodes
//        /// </summary>
//        private void ClearMap()
//        {
//            for (int x = 0; x < nodesX; x++)
//                for (int y = 0; y < nodesY; y++)
//                    nodes[x, y].Reset();
//
//            closedset.Clear();
//            openset.Clear();
//            camefrom.Clear();
//            neighbournodes.Clear();
//            finalPath.Clear();
//        }
//        #endregion
//
//        #region AddNode
//        private void AddNode(Node node, List<Node> set, Settings.DataStructure d = 0)
//        {
//            switch (d)
//            {
//                case 0:
//                    AddNode(node, set, s_dataStructure);
//                    break;
//                case Settings.DataStructure.LinearSearch:
//                    set.Add(node);
//                    break;
//                case Settings.DataStructure.InsertionSort:
//                    set.Add(node);
//                    InsertionSort(set);
//                    break;
//            }
//        }
//        private void InsertionSort(List<Node> set)
//        {
//            for (int i = 0; i < set.Count - 1; i++)
//            {
//                int current = i;
//                Node temp = set[current + 1];
//
//                while (current >= 0)
//                {
//                    if (set[current].F_Score > temp.F_Score)
//                    {
//                        set[current + 1] = set[current];
//                        current--;
//                    }
//                    else if (set[current].F_Score == temp.F_Score)
//                    {
//                        if (set[current].H_Score == temp.H_Score)
//                        {
//                            set[current + 1] = set[current];
//                            current--;
//                        }
//                    }
//                }
//                set[current + 1] = temp;
//            }
//        }
//        #endregion
//
//        #region ReturnLowest
//        /// <summary>
//        /// Returns the Node with the lowest F value in a list
//        /// </summary>
//        /// <param name="set">The list of Nodes to search through</param>
//        /// <returns></returns>
//        private Node ReturnLowest(List<Node> set, Settings.Algorithm p = 0, Settings.DataStructure d = 0)
//        {
//            switch (p)
//            {
//                case 0:
//                default:
//                    return ReturnLowest(set, s_algorithm, s_dataStructure);
//                case Settings.Algorithm.Lee:
//                    return set[set.Count - 1];
//                case Settings.Algorithm.Astar:
//                    switch (d)
//                    {
//                        case 0:
//                        default:
//                            return ReturnLowest(set, s_algorithm, s_dataStructure);
//                        case Settings.DataStructure.InsertionSort:
//                            return set[0];
//                        case Settings.DataStructure.LinearSearch:
//                            {
//                                Node small = set[0];
//                                for (int i = 1; i < set.Count; i++)
//                                {
//                                    if (set[i].F_Score < small.F_Score)
//                                        small = set[i];
//                                    else if (set[i].F_Score == small.F_Score)
//                                        if (set[i].H_Score < small.H_Score)
//                                            small = set[i];
//                                }
//                                return small;
//                            }
//                    }
//            }
//        }
//        #endregion
//
//        #region Heuristic
//        private double Heuristic(Node current, Node goal, Settings.Algorithm p = 0, Settings.Heuristic h = 0)
//        {
//            switch (p)
//            {
//                default:
//                case 0:
//                    return Heuristic(current, goal, s_algorithm, s_heuristic);
//                case Settings.Algorithm.Lee:
//                    return 0;
//                case Settings.Algorithm.Dijkstra:
//                    return 0;
//                case Settings.Algorithm.Astar:
//                    switch (h)
//                    {
//                        default:
//                        case 0:
//                            return Heuristic(current, goal, p, s_heuristic);
//                        case Settings.Heuristic.Manhattan:
//                            return (Math.Abs(goal.PointLocation.X - current.PointLocation.X) +
//                                    Math.Abs(goal.PointLocation.Y - current.PointLocation.Y));
//                        case Settings.Heuristic.Euclidean:
//                            double dx1 = Math.Pow((double)Math.Abs(goal.PointLocation.X - current.PointLocation.X), 2);
//                            double dy1 = Math.Pow((double)Math.Abs(goal.PointLocation.Y - current.PointLocation.Y), 2);
//                            return Math.Sqrt(dx1 + dy1);
//                        case Settings.Heuristic.Refined:
//                            //double dx = Math.Abs(goal.PointLocation.X - current.PointLocation.X);
//                            //double dy = Math.Abs(goal.PointLocation.Y - current.PointLocation.Y);
//                            //return (dx + dy) * 1.001;
//                            //return 0;
//                            double dx;
//                            double dy;
//                            double D = 1;
//                            double D2 = 1;
//
//                            dx = Math.Abs(current.PointLocation.X - goal.PointLocation.X);
//                            dy = Math.Abs(current.PointLocation.Y - goal.PointLocation.Y);
//
//                            double dxy = (dx + dy) - (1 - (1 / (current.G_Score + 1)));
//
//                            //return dxy + (1 / (Math.Max(dx, dy) + 1));
//                            return D * (dx + dy) + (D2 - 2 * D) * Math.Min(dx, dy);
//                        case Settings.Heuristic.Greedy:
//                            double g_dx = Math.Abs(current.PointLocation.X - goal.PointLocation.X);
//                            double g_dy = Math.Abs(current.PointLocation.Y - goal.PointLocation.Y);
//                            return (g_dx * g_dx + g_dy * g_dy);
//                    }
//            }
//        }
//        #endregion
//
//        #region OrganiseNeighbours - Change For Diagonals
//        /// <summary>
//        /// Checks to see if the neighbours aren't in the openlist and adds them to the list of neighbours if true
//        /// </summary>
//        /// <param name="current">The current node</param>
//        /// <param name="neighbourNodes">The neighbour list</param>
//        private void OrganiseNeighbours(Node current, List<Node> neighbourNodes)
//        {
//            if (current.PointLocation.X < nodes.GetLength(0) - 1) // X Pos
//                CheckNeighbor(nodes[current.PointLocation.X + 1, current.PointLocation.Y], neighbournodes);
//            if (current.PointLocation.X > 0) // X Neg
//                CheckNeighbor(nodes[current.PointLocation.X - 1, current.PointLocation.Y], neighbournodes);
//            if (current.PointLocation.Y < nodes.GetLength(1) - 1) // Y Pos
//                CheckNeighbor(nodes[current.PointLocation.X, current.PointLocation.Y + 1], neighbournodes);
//            if (current.PointLocation.Y > 0) // Y neg
//                CheckNeighbor(nodes[current.PointLocation.X, current.PointLocation.Y - 1], neighbournodes);
//
//            if ((current.PointLocation.X < nodes.GetLength(0) - 1) && (current.PointLocation.Y < nodes.GetLength(1) - 1))// X Pos // Y Pos
//                if (nodes[current.PointLocation.X + 1, current.PointLocation.Y].Floor == FloorType.Wall ||  // Xpos for wall
//                    nodes[current.PointLocation.X, current.PointLocation.Y + 1].Floor == FloorType.Wall)    // Ypos for wall
//                { }
//                else
//                    CheckNeighbor(nodes[current.PointLocation.X + 1, current.PointLocation.Y + 1], neighbournodes);
//
//            if ((current.PointLocation.X > 0) && (current.PointLocation.Y < nodes.GetLength(1) - 1)) // X Neg // Y Pos
//                if (nodes[current.PointLocation.X - 1, current.PointLocation.Y].Floor == FloorType.Wall ||  // Xneg for wall
//                    nodes[current.PointLocation.X, current.PointLocation.Y + 1].Floor == FloorType.Wall)    // Ypos for wall
//                { }
//                else
//                    CheckNeighbor(nodes[current.PointLocation.X - 1, current.PointLocation.Y + 1], neighbournodes);
//
//            if ((current.PointLocation.Y > 0) && (current.PointLocation.X > 0)) // Y Neg // X Neg
//                if (nodes[current.PointLocation.X, current.PointLocation.Y - 1].Floor == FloorType.Wall ||  // Yneg for wall
//                    nodes[current.PointLocation.X - 1, current.PointLocation.Y].Floor == FloorType.Wall)    // Xneg for wall
//                { }
//                else
//                    CheckNeighbor(nodes[current.PointLocation.X - 1, current.PointLocation.Y - 1], neighbournodes);
//
//            if ((current.PointLocation.Y > 0) && (current.PointLocation.X < nodes.GetLength(0) - 1)) // Y neg // X Pos
//                if (nodes[current.PointLocation.X, current.PointLocation.Y - 1].Floor == FloorType.Wall ||  // Yneg for wall
//                    nodes[current.PointLocation.X + 1, current.PointLocation.Y].Floor == FloorType.Wall)    // Xpos for wall
//                { }
//                else
//                    CheckNeighbor(nodes[current.PointLocation.X + 1, current.PointLocation.Y - 1], neighbournodes);
//        }
//        private void CheckNeighbor(Node neighbour, List<Node> neighbourNodes)
//        {
//            if (/*(!openset.Contains(neighbour)) &&*/
//                (!closedset.Contains(neighbour)) &&
//                (neighbour.Walkable == true))
//                neighbourNodes.Add(neighbour);
//        }
//        #endregion
//
//        #region ReconstructPath
//        /// <summary>
//        /// Constructs the shortest path back to the start
//        /// </summary>
//        /// <param name="current">The current node</param>
//        /// <returns></returns>
//        private List<Node> ReconstructPath(Node current)
//        {
//            List<Node> totalpath = new List<Node>();
//
//            totalpath.Add(current);
//            current.Floor = FloorType.Goal;
//            while (current.CameFrom != null)
//            {
//                current = current.CameFrom;
//                current.Floor = FloorType.Path;
//                totalpath.Add(current);
//            }
//            return totalpath;
//        }
//        #endregion
//
//
//        /// <summary>
//        /// Draws the nodes on the nodes
//        /// </summary>
//        /// <param name="sb">Spritebatch used for drawing</param>
//        public void Draw(SpriteBatch sb)
//        {
//            for (int x = 0; x < nodesX; x++)
//            {
//                for (int y = 0; y < nodesY; y++)
//                {
//                    if (nodes[x, y].Floor == FloorType.Floor)
//                        nodes[x, y].Draw(sb, FloorType.Floor, txrList[0]);
//                    if (nodes[x, y].Floor == FloorType.Wall)
//                        nodes[x, y].Draw(sb, FloorType.Wall, txrList[1]);
//                    if (nodes[x, y].Floor == FloorType.WayPoint)
//                        nodes[x, y].Draw(sb, FloorType.WayPoint, txrList[6]);
//                    if (nodes[x, y].Floor == FloorType.Open)
//                        nodes[x, y].Draw(sb, FloorType.Open, txrList[4]);
//                    if (nodes[x, y].Floor == FloorType.Closed)
//                        nodes[x, y].Draw(sb, FloorType.Closed, txrList[3]);
//                    if (nodes[x, y].Floor == FloorType.Path)
//                        nodes[x, y].Draw(sb, FloorType.Path, txrList[2]);
//                    if (nodes[x, y].Floor == FloorType.Goal)
//                        nodes[x, y].Draw(sb, FloorType.Goal, txrList[5]);
//                }
//            }
//        }
//
//
//        private void ChangeSetting<T>(KeyboardState n, KeyboardState o, T s, params Keys[] k)
//        {
//            foreach (Keys key in k)
//            {
//                if (n.IsKeyDown(key) && o.IsKeyUp(key))
//                {
//                    if (typeof(T) == typeof(Settings.Algorithm))
//                        s_algorithm = (Settings.Algorithm)(object)s;
//
//                    else if (typeof(T) == typeof(Settings.Heuristic))
//                        s_heuristic = (Settings.Heuristic)(object)s;
//
//                    else if (typeof(T) == typeof(Settings.DataStructure))
//                        s_dataStructure = (Settings.DataStructure)(object)s;
//                }
//            }
//        }
//    }
//}


