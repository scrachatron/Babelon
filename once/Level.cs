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
        public int[,] Map;
        public int Layer { get { return CurrLayer; } set { CurrLayer = value; } }

        public Point m_StartPos;
        public Point m_WinPos;

        private Point m_LayerSize;
        private int CurrLayer = 0;
        public Vector2 Gravity { get; set; }

        public Level()
        {

            m_LayerSize = new Point(Game1.graphics.PreferredBackBufferWidth / Game1.TILESIZE, Game1.graphics.PreferredBackBufferHeight / Game1.TILESIZE);
            Map = new int[15, 25]
            {
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}
            };




            m_StartPos = new Point(-1, -1);
            m_WinPos = new Point(-1, -1);
            FindPoints();
        }

        public void Recalculate()
        {

            m_LayerSize = new Point(Game1.graphics.PreferredBackBufferWidth / Game1.TILESIZE, Game1.graphics.PreferredBackBufferHeight / Game1.TILESIZE);
            
            int[,] newArray = new int[(int)Game1.graphics.PreferredBackBufferHeight / Game1.TILESIZE, (int)Game1.graphics.PreferredBackBufferWidth / Game1.TILESIZE];
            int minX = Math.Min(Map.GetLength(0), newArray.GetLength(0));
            int minY = Math.Min(Map.GetLength(1), newArray.GetLength(1));

            for (int i = 0; i < minX; ++i)
                Array.Copy(Map, i * Map.GetLength(1), newArray, i * newArray.GetLength(1), minY);

            Map = newArray;

            

            m_StartPos = new Point(-1, -1);
            m_WinPos = new Point(-1, -1);
            FindPoints();
        }

        public void FindPoints()
        {
            for (int x = 0; x < Map.GetLength(0); x++)
                for (int y = 0; y < Map.GetLength(1); y++)
                {
                    if (Map[x, y] == 2)
                        m_StartPos = new Point(x, y);
                    else if (Map[x, y] == 3)
                        m_WinPos = new Point(x, y);
                }

            if (m_StartPos.X < 0)
            {
                for (int x = 0; x < Map.GetLength(1); x++)
                    for (int y = 0; y < Map.GetLength(0); y++)
                    {
                        if (Map[y, x] == 2)
                            Map[y, x] = 0;
                    }
                Map[(int)(Map.GetLength(0) / 2) , 3] = 2;
                m_StartPos = new Point(Map.GetLength(0) / 2, 3);
                
            }
            if (m_WinPos.X < 0)
            {
                for (int x = 0; x < Map.GetLength(1); x++)
                    for (int y = 0; y < Map.GetLength(0); y++)
                    {
                        if (Map[y, x] == 3)
                            Map[y, x] = 0;
                    }
                Map[(int)(Map.GetLength(0) / 2) , Map.GetLength(1) - 4] = 3;
                m_WinPos = new Point(Map.GetLength(0) / 2, Map.GetLength(1) - 4);
                

            }
        }

        public void DrawMe(SpriteBatch sb)
        {
            for (int x = 0; x < Map.GetLength(1); x++)
            {
                for (int y = 0; y < Map.GetLength(0); y++)
                {
                    if (Map[y, x] == 1)
                        sb.Draw(Pixel, new Rectangle(x * Game1.TILESIZE, y * Game1.TILESIZE, Game1.TILESIZE, Game1.TILESIZE), Color.Black);
                    else if (Map[y, x] == 2)
                        sb.Draw(Pixel, new Rectangle(x * Game1.TILESIZE, y * Game1.TILESIZE, Game1.TILESIZE, Game1.TILESIZE), Color.Green);
                    else if (Map[y, x] == 3)
                        sb.Draw(Pixel, new Rectangle(x * Game1.TILESIZE, y * Game1.TILESIZE, Game1.TILESIZE, Game1.TILESIZE), Color.Red);
                }
            }
        }
    }
}
