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
        public static int TILESIZE = 32;
        public static GraphicsDeviceManager graphics;
        public static readonly Random RNG = new Random();

        SpriteBatch spriteBatch;
        Dictionary<string, Level> m_levels;
        Player m_Player;
        InputManager m_input;
        MapEdit edit;
        Astar a_star;
        Lee lee;

        bool runningalgorythem;
        float RunningTime;
        float counts;
        List<float> RunningTimes;
        Stopwatch countdown;
        string warning_string = "No Path Found";
        Vector2 warning_string_Legnth;

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

            m_input = new InputManager(PlayerIndex.One);

            base.Initialize();
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

            
            m_input.UpdateMe();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            //spriteBatch.Begin(SpriteSortMode.BackToFront,null,SamplerState.PointClamp,DepthStencilState.Default,RasterizerState.CullNone,null);
            spriteBatch.Begin();

            
            spriteBatch.End();
            


            base.Draw(gameTime);
        }
    }
}
