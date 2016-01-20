using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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
        private int Height;

        public Level()
        {
            m_mazeGen = new MazeGenerator();
            Height = 1;
            RegenMaze();
        }

        private void RegenMaze()
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

        private void RegenTown()
        {
            for (int x = 0; x < Map.GetLength(0); x++)
                for (int y = 0; y < Map.GetLength(1); y++)
                {
                    Map[x, y] = 1;
                }
            Rectangle entryway = new Rectangle(Map.GetLength(0) / 16, 5, Map.GetLength(0) / 8, 10);
            Carve(entryway);

            m_StartPos = new Point(entryway.X + ((entryway.Width / 3) * 2), entryway.Y + entryway.Height / 3);
            m_WinPos = new Point(entryway.X + entryway.Width / 3, entryway.Y + entryway.Height / 3);
        }
        private void Carve(Rectangle rect)
        {
            for (int x = 0; x < rect.Width; x++)
                for (int y = 0; y < rect.Height; y++)
                {
                    Map[rect.X + x, rect.Y + y] = 0;
                }
        }

        public void UpdateMe(Player player,InputManager input)
        {
            if (player.VirtualPosition == m_WinPos && input.WasPressedBack(Keys.Enter))
            {
                Height++;
                if (Height % 10 == 0)
                    RegenTown();
                else
                    RegenMaze();
                player.VirtualPosition = m_StartPos;
            }
            else if (player.VirtualPosition == m_StartPos && input.WasPressedBack(Keys.Enter))
            {
                Height--;
                if (Height % 10 == 0)
                    RegenTown();
                else
                    RegenMaze();
                player.VirtualPosition = m_WinPos;
            }



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
