using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Graphics;

namespace Once
{

    class Level : Pixelclass
    {
        
        public int[,] Map
        {
            get; set; 
        }
        public Point LayerSize
        {
            get { return m_LayerSize; }
        }

        public Point m_StartPos;
        public Point m_WinPos;
        private Point m_LayerSize;
        public MazeGenerator m_mazeGen;

        public Level()
        {
            m_mazeGen = new MazeGenerator();

            RegenMaze();
            //m_mazeGen.GenerateMaze(new MazeInfo(new Point(65, 65), 10, 2, 2, 46));

        }

        public void RegenMaze()
        {
            m_mazeGen.GenerateMaze(new MazeInfo(new Point(65, 65), 10, 2, 2, 46));
            Map = m_mazeGen.MapInformation.Map;

            m_LayerSize = new Point(32, 32);
            Rectangle temprect = m_mazeGen.m_rooms[Game1.RNG.Next(0, m_mazeGen.m_rooms.Count)];
            temprect = m_mazeGen.m_rooms[0];
            m_StartPos = new Point(temprect.X, temprect.Y);

        }

        public void DrawMe(SpriteBatch sb)
        {
            Map = m_mazeGen.MapInformation.Map;
            for (int x = 0; x < Map.GetLength(0); x++)
            {
                for (int y = 0; y < Map.GetLength(1); y++)
                {
                    if (Map[x, y] == 1)
                        sb.Draw(Pixel, new Rectangle(x * m_LayerSize.X, y * m_LayerSize.Y, m_LayerSize.X, m_LayerSize.Y), Color.Black);
                    else if (Map[x, y] == 2)
                        sb.Draw(Pixel, new Rectangle(x * m_LayerSize.X, y * m_LayerSize.Y, m_LayerSize.X, m_LayerSize.Y), Color.Green);
                    else if (Map[x, y] == 3)
                        sb.Draw(Pixel, new Rectangle(x * m_LayerSize.X, y * m_LayerSize.Y, m_LayerSize.X, m_LayerSize.Y), Color.Red);
                }
            }
        }
    }
}
