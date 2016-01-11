using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Timers;

namespace Once
{
    public enum GameState
    {
        MainMenu,
        PauseMenu,
        LevelEdit,
        GamePlay
    }
    public class Game1 : Game
    {
        GameState gs;
        public static int TILESIZE = 4;
        public static GraphicsDeviceManager graphics;
        public static readonly Random RNG = new Random();

        SpriteBatch spriteBatch;
        Level m_level;
        Player m_Player;
        Camera m_cam;
        InputManager m_input;

        public Game1()
        { 
            graphics = new GraphicsDeviceManager(this);
            //graphics.PreferredBackBufferWidth = 1920;
            //graphics.PreferredBackBufferHeight = 1080;
            Content.RootDirectory = "Content";
            //Window.IsBorderless = true;
            IsMouseVisible = true;
            //graphics.SynchronizeWithVerticalRetrace = false;
            //IsFixedTimeStep = false;
            //graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            Pixelclass.Content = Content;
            m_Player = new Player();
            m_input = new InputManager();
            m_cam = new Camera(GraphicsDevice.Viewport);

            m_level = new Level();
            
            base.Initialize();

            m_level.m_mazeGen.GenerateMaze(new MazeInfo(new Point(65, 65), 10, 2, 2, 100));
            m_Player.Position = m_level.m_StartPos;

        }
        protected override void LoadContent()
        {

            spriteBatch = new SpriteBatch(GraphicsDevice);
            Pixelclass.Pixel = Content.Load<Texture2D>("Pixel");
            Pixelclass.Font = Content.Load<SpriteFont>("Font1");
            Pixelclass.Tfont = Content.Load<SpriteFont>("TinyFont");

           
        }

        protected override void UnloadContent()
        {

        }
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (m_input.WasPressedBack(Keys.Enter))
            {
                m_level.RegenMaze();// (new MazeInfo(new Point(65, 65), 10, 2, 2, 40));
                m_Player.Position = m_level.m_StartPos;
            }

            m_Player.UpdateMe(gameTime, m_level, m_input);

            m_input.UpdateMe();
            m_cam.UpdateMe(new Vector2(m_Player.Rect.X,m_Player.Rect.Y), new Point(m_level.Map.GetLength(0) * m_level.LayerSize.X, m_level.Map.GetLength(1) * m_level.LayerSize.Y));
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, RasterizerState.CullNone, null, m_cam.Transform);
            m_level.DrawMe(spriteBatch);
            m_Player.DrawMe(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
