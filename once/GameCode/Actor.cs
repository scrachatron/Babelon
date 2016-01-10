using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Once
{
    class Actor : Pixelclass
    {
        public Rectangle Rect
        {
            get { return m_rect; }
            set
            {
                m_rect = value;
                m_pos.X = value.X;
                m_pos.Y = value.Y;
            }

        }
        public Vector2 Position
        {
            get { return m_pos; }
            set
            {
                m_pos = value;
                m_rect.X = (int)value.X;
                m_rect.Y = (int)value.Y;
            }
        }

        private Color m_tint;
        private Rectangle m_rect;
        private Vector2 m_pos;

        private Vector2 m_vel;
        protected Point MoveHere = new Point(0, 0);
        protected Point IAM = new Point(0, 0);

        public Actor(Rectangle rect, Color tint, int layer)
        {
            Rect = rect;
            m_vel = Vector2.Zero;
            m_tint = tint;
        }
        public virtual void UpdateMe(GameTime gt, Level level)
        {
            m_rect.Width = level.LayerSize.X;
            m_rect.Height = level.LayerSize.Y;
            m_rect.X = (int)m_pos.X;
            m_rect.Y = (int)m_pos.Y;

            IAM.X = (int)((m_pos.X + m_rect.Width/2) / level.LayerSize.X);
            IAM.Y = (int)((m_pos.Y + m_rect.Height / 2) / level.LayerSize.Y);

            Collision(level);
            //Position += m_vel;
            m_vel = Vector2.Zero;
        }
        public virtual void DrawMe(SpriteBatch sb)
        {
            sb.Draw(Pixel, m_rect , m_tint);
        }
        private void Collision(Level lvl)
        {
            if (lvl.Map[IAM.X + MoveHere.X, IAM.Y + MoveHere.Y] == 0)
            {
                m_pos += MoveHere.ToVector2();
            }
        }
    }

    class Player : Actor
    {
        public Point IAMHERE
        {
            get { return IAM; }
        }

        public Player()
            :base(new Rectangle(0,0,14,14),Color.Red, 0)
        {

        }
        public void UpdateMe(GameTime gt, Level level, InputManager input)
        {
            if (input.IsDown(Keys.Left))
                MoveHere.X += -1;
            if (input.IsDown(Keys.Right))
                MoveHere.X += 1;
            if (input.IsDown(Keys.Up))
                MoveHere.Y += -1;
            if (input.IsDown(Keys.Down))
                MoveHere.Y += 1;

            base.UpdateMe(gt, level);

            MoveHere.X = 0;
            MoveHere.Y = 0;
        }
        public override void DrawMe(SpriteBatch sb)
        {
            
            base.DrawMe(sb);
            sb.DrawString(Font, IAM.X + "," + IAM.Y, Position, Color.White);
        }
    }
}
