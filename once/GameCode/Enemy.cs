using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace Once.GameCode
{
    class Enemy : Actor
    {
        public Enemy(Point pos)
            :base(pos,Color.Blue,0)
        {

        }
        public override void UpdateMe(GameTime gt, Level level)
        {
            base.UpdateMe(gt, level);
        }
        public override void DrawMe(SpriteBatch sb)
        {
            base.DrawMe(sb);
        }

    }
}
