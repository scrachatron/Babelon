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
        private Rectangle Rect
        {
            get { return m_rect; }
            set
            {
                m_rect = value;
                m_position.X = value.X;
                m_position.Y = value.Y;
            }

        }
        public Point VirtualPosition
        {
            get { return m_virtualpos; }
            set
            {
                m_virtualpos = value;
                m_position.X = value.X * m_rect.Width;
                m_position.Y = value.Y * m_rect.Height;
                m_targetPos = m_position.ToPoint();
                m_velocity = Vector2.Zero;
            }
        }
        public Vector2 Position
        {
            get { return m_position; }
            set { m_position = value;
                m_targetPos = value.ToPoint();
                m_velocity = Vector2.Zero;
            }
        }

        private Color m_tint;
        private Rectangle m_rect;

        private Vector2 m_position;
        private Vector2 m_velocity;
        private Point m_targetPos;

        private Point m_virtualpos;

        protected Point MoveHere = new Point(0, 0);

        public Actor(Rectangle rect, Color tint, int layer)
        {
            Rect = rect;
            m_position = new Vector2(rect.X, rect.Y);
            m_targetPos = m_position.ToPoint();
            m_tint = tint;
        }
        public virtual void UpdateMe(GameTime gt, Level level)
        {
            m_rect.X = (int)Math.Round(m_position.X);
            m_rect.Y = (int)Math.Round(m_position.Y);

            if (m_rect.X != (int)m_targetPos.X || m_rect.Y != (int)m_targetPos.Y)
            {
                m_velocity = new Vector2(m_virtualpos.X * level.LayerSize.X - Position.X, m_virtualpos.Y * level.LayerSize.Y - Position.Y);
                m_position += m_velocity/3;
            }
            else
                Collision(level);
        }
        public virtual void DrawMe(SpriteBatch sb)
        {
            sb.Draw(Pixel, m_rect , m_tint);
        }
        private void Collision(Level lvl)
        {
            if (MoveHere.X != 0 && MoveHere.Y != 0)
            {
                Point tmp = new Point(0, 0);
                if (lvl.Map[VirtualPosition.X + MoveHere.X, VirtualPosition.Y] == 0)
                {
                    tmp.X = MoveHere.X;
                }
                if (lvl.Map[VirtualPosition.X, VirtualPosition.Y + MoveHere.Y] == 0)
                {
                    tmp.Y = MoveHere.Y;
                }
                if (lvl.Map[VirtualPosition.X + tmp.X, VirtualPosition.Y + tmp.Y] == 0)
                {
                    m_virtualpos += tmp;
                    m_targetPos = (m_virtualpos * lvl.LayerSize);

                    return;
                }


            }
            else if (lvl.Map[VirtualPosition.X + MoveHere.X, VirtualPosition.Y + MoveHere.Y] == 0)
            {
                m_virtualpos += MoveHere;
                m_targetPos = (m_virtualpos * lvl.LayerSize);

                return;
            }
        }
    }

    class Player : Actor
    {

        public Player(Point dim)
            :base(new Rectangle(0,0,dim.X,dim.Y),Color.Red, 0)
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
            sb.DrawString(Font, VirtualPosition.X + "," + VirtualPosition.Y, Position, Color.White);
        }
    }
}
