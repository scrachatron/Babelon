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
        public Point m_StartPos;
        public Point m_WinPos;
        private Point m_LayerSize;
        

        public Level()
        {
            

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
