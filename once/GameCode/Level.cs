using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Once.GameCode;

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
        public static Point m_LayerSize;
        public MazeGenerator m_mazeGen;
        private int Height;
        private List<Enemy> m_enemies; 

        public Level()
        {
            m_mazeGen = new MazeGenerator();
            m_enemies = new List<Enemy>();
            Height = 1;
            RegenMaze();
        }

        private void RegenMaze()
        {
            m_mazeGen.GenerateMaze(new MazeInfo(new Point(128, 100), 100, 7, 5, 60));
            Map = m_mazeGen.MapInformation.Map;

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
            m_enemies.Clear();
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

            m_enemies.Clear();
        }
        private void Carve(Rectangle rect)
        {
            for (int x = 0; x < rect.Width; x++)
                for (int y = 0; y < rect.Height; y++)
                {
                    Map[rect.X + x, rect.Y + y] = 0;
                }
        }

        public void UpdateMe(GameTime gt,Player player,InputManager input)
        {
            if (input.WasPressedBack(Keys.Enter))
            {
                if (player.VirtualPosition == m_WinPos)
                {
                    Height++;
                    if (Height % 10 == 0)
                        RegenTown();
                    else
                        RegenMaze();
                    player.VirtualPosition = m_StartPos;
                }
                else if (player.VirtualPosition == m_StartPos)
                {
                    Height--;
                    if (Height % 10 == 0)
                        RegenTown();
                    else
                        RegenMaze();
                    player.VirtualPosition = m_WinPos;
                }
            }
            if (input.IsDown(Keys.Space))
            {
                Rectangle temprect;
                temprect = m_mazeGen.m_rooms[Game1.RNG.Next(0, m_mazeGen.m_rooms.Count)];

                m_enemies.Add(new Enemy(new Point(temprect.X + Game1.RNG.Next(1, temprect.Width - 1), temprect.Y + Game1.RNG.Next(1, temprect.Height - 1))));
            }
            for (int i = 0; i < m_enemies.Count; i++)
                m_enemies[i].UpdateMe(gt, this);
        }
        public void DrawMe(SpriteBatch sb)
        {
            sb.Draw(Pixel, new Rectangle(0, 0, Map.GetLength(0) * m_LayerSize.X, Map.GetLength(1) * m_LayerSize.Y), Color.Gray);

            for (int x = 0; x < Map.GetLength(0); x++)
            {
                for (int y = 0; y < Map.GetLength(1); y++)
                {
                    if (Map[x, y] == 1)
                    {
                        sb.Draw(Pixel, new Rectangle(x * m_LayerSize.X, y * m_LayerSize.Y, m_LayerSize.X, m_LayerSize.Y), Color.Black);
                        
                    }
                    else if (Map[x, y] == 2)
                    {
                        sb.Draw(Pixel, new Rectangle(x * m_LayerSize.X, y * m_LayerSize.Y, m_LayerSize.X, m_LayerSize.Y), Color.Green);

                    }
                    else if (Map[x, y] == 3)
                    {
                        sb.Draw(Pixel, new Rectangle(x * m_LayerSize.X, y * m_LayerSize.Y, m_LayerSize.X, m_LayerSize.Y), Color.Red);

                    }
                }
            }

            for (int i = 0; i < m_enemies.Count; i++)
                m_enemies[i].DrawMe(sb);

            sb.Draw(Pixel, new Rectangle(m_WinPos.X * m_LayerSize.X, m_WinPos.Y * m_LayerSize.Y, m_LayerSize.X, m_LayerSize.Y), Color.Green);
            sb.Draw(Pixel, new Rectangle(m_StartPos.X * m_LayerSize.X, m_StartPos.Y * m_LayerSize.Y, m_LayerSize.X, m_LayerSize.Y), Color.Red);
        }

        public void DrawMap(SpriteBatch sb)
        {
            int size = 4;

            for (int x = 0; x < Map.GetLength(0); x++)
            {
                for (int y = 0; y < Map.GetLength(1); y++)
                {
                    if (Map[x, y] == 0)
                    {
                        sb.Draw(Pixel, new Rectangle(x *size, y * size, size, size), Color.White * 0.2f);
                    }
                }
            }

            for (int i = 0; i < m_enemies.Count; i++)
                m_enemies[i].DrawMap(sb);

            sb.Draw(Pixel, new Rectangle(m_WinPos.X * size, m_WinPos.Y * size, size, size), Color.Green * 0.5f);
            sb.Draw(Pixel, new Rectangle(m_StartPos.X * size, m_StartPos.Y * size, size, size), Color.Red * 0.5f);


        }
    }
}
