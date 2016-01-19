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
            m_mazeGen.GenerateMaze(new MazeInfo(new Point(128, 100), 100, 7, 5, 60));
            Map = m_mazeGen.MapInformation.Map;

            m_LayerSize = new Point(32, 32);
            Rectangle temprect;
            temprect = m_mazeGen.m_rooms[Game1.RNG.Next(0, m_mazeGen.m_rooms.Count)];
            Rectangle temprect2 = temprect;

            while (temprect == temprect2)
            {
                temprect2 = m_mazeGen.m_rooms[Game1.RNG.Next(0, m_mazeGen.m_rooms.Count)];
            }


            m_StartPos = new Point(temprect.X  + Game1.RNG.Next(1, temprect.Width - 1), temprect.Y + Game1.RNG.Next(1, temprect.Height - 1));
            //temprect = m_mazeGen.m_rooms[Game1.RNG.Next(0, m_mazeGen.m_rooms.Count)];
            m_WinPos = new Point(temprect2.X  + Game1.RNG.Next(1, temprect2.Width - 1), temprect2.Y + Game1.RNG.Next(1, temprect2.Height - 1));

            //Map[m_WinPos.X, m_WinPos.Y] = 2;
            //Map[m_StartPos.X, m_StartPos.Y] = 3;

        }

        public void DrawMe(SpriteBatch sb)
        {

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
            sb.Draw(Pixel, new Rectangle(m_WinPos.X * m_LayerSize.X, m_WinPos.Y * m_LayerSize.Y, m_LayerSize.X, m_LayerSize.Y), Color.Green);
            sb.Draw(Pixel, new Rectangle(m_StartPos.X * m_LayerSize.X, m_StartPos.Y * m_LayerSize.Y, m_LayerSize.X, m_LayerSize.Y), Color.Red);
        }
    }
}
