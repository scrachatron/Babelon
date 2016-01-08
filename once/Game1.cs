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
        const int RUNTHISMANYTIMES = 100;
        bool runmaze;

        GameState gs;
        public static int TILESIZE = 32;
        public static GraphicsDeviceManager graphics;
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
            

            countdown = new Stopwatch();
            gs = GameState.LevelEdit;
            Pixelclass.Content = Content;
            m_levels = new Dictionary<string, Level>();
            m_Player = new Player();
            a_star = new Astar();
            lee = new Lee();

            if (gs == GameState.LevelEdit)
                m_input = new InputManager();
            else
                m_input = new InputManager(PlayerIndex.One);
            edit = new MapEdit();
            //string fileLocation = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Pixel Studios\Once\levels.xml";

            base.Initialize();
            warning_string_Legnth = Pixelclass.Font.MeasureString(warning_string) / 2;
            a_star.ReloadPathfinding(edit.Level.Map, edit.Level.m_StartPos, edit.Level.m_WinPos);
            lee.ReloadPathfinding(edit.Level.Map, edit.Level.m_StartPos, edit.Level.m_WinPos);
            runningalgorythem = true;
            RunningTimes = new List<float>();
            counts = 1;
        }
        protected override void LoadContent()
        {

            spriteBatch = new SpriteBatch(GraphicsDevice);
            Pixelclass.Pixel = Content.Load<Texture2D>("Pixel");
            Pixelclass.Font = Content.Load<SpriteFont>("Font1");
            Pixelclass.Tfont = Content.Load<SpriteFont>("TinyFont");

            //TILESIZE = 2;
            //edit.Reset(TILESIZE);

            //warning_string_Legnth = Pixelclass.Font.MeasureString(warning_string);
        }

        protected override void UnloadContent()
        {

        }
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            edit.UpdateMe(gameTime, m_input);
            if (m_input.WasMiddleClickedBack())
            {
                Point Temp;
                Temp = edit.Level.m_StartPos;
                edit.Level.m_StartPos = edit.Level.m_WinPos;
                edit.Level.m_WinPos = Temp;

                edit.Level.Map[edit.Level.m_StartPos.X, edit.Level.m_StartPos.Y] = 2;
                edit.Level.Map[edit.Level.m_WinPos.X, edit.Level.m_WinPos.Y] = 3;
                edit.MapChanged = true;

            }
            if (m_input.WasPressedBack(Keys.S))
            {
                RunningTimes.Clear();
                for (int i = 0; i < RUNTHISMANYTIMES; i++)
                {
                    countdown.Reset();
                    countdown.Start();
                    if (runningalgorythem)
                        a_star.ReloadPathfinding(edit.Level.Map, edit.Level.m_StartPos, edit.Level.m_WinPos);
                    else
                        lee.ReloadPathfinding(edit.Level.Map, edit.Level.m_StartPos, edit.Level.m_WinPos);
                    countdown.Stop();
                    RunningTime = countdown.ElapsedMilliseconds;

                    RunningTimes.Add(RunningTime);
                }

                RunningTime = 0;
                for (int i = 0; i < RunningTimes.Count; i++)
                {
                    RunningTime += RunningTimes[i];
                }
                RunningTime /= (RunningTimes.Count - 1);
                counts = RunningTimes.Count;

            }
            if (m_input.WasPressedBack(Keys.Z))
            {

                countdown.Reset();
                countdown.Start();

                edit.RandomFill(33);

                countdown.Stop();
                counts = 1;

                RunningTime = countdown.ElapsedMilliseconds;
                RunningTime = (float)Math.Round(RunningTime, 10);

            }
            if (m_input.WasPressedBack(Keys.W))
            {
                runningalgorythem = !runningalgorythem;
                edit.MapChanged = true;
            }

            if (m_input.WasPressedBack(Keys.Q) && TILESIZE < 32)
            {
                TILESIZE *= 2;
                edit.Reset(TILESIZE);

            }
            else if (m_input.WasPressedBack(Keys.E) && TILESIZE > 2)
            {
                TILESIZE /= 2;
                edit.Reset(TILESIZE);
            }

            if (m_input.WasPressedBack(Keys.Space))
            {
                edit.Reset();
                TILESIZE = 32;
                edit.Reset();
            }
            if (edit.MapChanged)
            {
                countdown.Reset();
                countdown.Start();
                if (runningalgorythem)
                    a_star.ReloadPathfinding(edit.Level.Map, edit.Level.m_StartPos, edit.Level.m_WinPos);
                else
                    lee.ReloadPathfinding(edit.Level.Map, edit.Level.m_StartPos, edit.Level.m_WinPos);
                countdown.Stop();
                counts = 1;
                RunningTime = countdown.ElapsedMilliseconds;
            }
            if (m_input.IsDown(Keys.LeftControl))
            {
                a_star.StepFinding(edit.Level.m_StartPos,edit.Level.m_WinPos,edit.Level.Map);
            }

            RunningTime = (float)Math.Round(RunningTime, 3);
            m_input.UpdateMe();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            //spriteBatch.Begin(SpriteSortMode.BackToFront,null,SamplerState.PointClamp,DepthStencilState.Default,RasterizerState.CullNone,null);
            spriteBatch.Begin();

            if (gs == GameState.LevelEdit)
            {
                if (runningalgorythem)
                {
                    for (int i = 0; i < a_star.ClosedSet.Count; i++)
                    {
                        if (a_star.LargestGScore != 0)
                            spriteBatch.Draw(Pixelclass.Pixel, new Rectangle((a_star.ClosedSet[i].Location.Y * TILESIZE), (a_star.ClosedSet[i].Location.X * TILESIZE), TILESIZE, TILESIZE), Color.Blue * (1 - (float)((float)a_star.ClosedSet[i].G / (float)a_star.LargestGScore)));
                        
                    }
                    for (int i = 1; i < a_star.OpenSet.Count; i++)
                    {
                        if (a_star.LargestGScore != 0)
                            spriteBatch.Draw(Pixelclass.Pixel, new Rectangle((a_star.OpenSet[i].Location.Y * TILESIZE), (a_star.OpenSet[i].Location.X * TILESIZE), TILESIZE, TILESIZE), Color.DarkCyan * 0.8f);

                    }
                    for (int i = 0; i < a_star.CompleatedPath.Count; i++)
                    {
                        spriteBatch.Draw(Pixelclass.Pixel, new Rectangle((a_star.CompleatedPath[i].Location.Y * TILESIZE) + (TILESIZE / 2) - TILESIZE / 8, (a_star.CompleatedPath[i].Location.X * TILESIZE) + (TILESIZE / 2) - TILESIZE / 8, TILESIZE / 4, TILESIZE / 4), Color.Gold);
                        }
                }
                else
                {
                    for (int i = 0; i < lee.ClosedSet.Count; i++)
                    {
                        if (lee.LargestGScore != 0)
                            spriteBatch.Draw(Pixelclass.Pixel, new Rectangle((lee.ClosedSet[i].Location.Y * TILESIZE), (lee.ClosedSet[i].Location.X * TILESIZE), TILESIZE, TILESIZE), Color.Blue * (1 - (float)((float)lee.ClosedSet[i].G / (float)lee.LargestGScore)));

                    }


                    for (int i = 1; i < lee.OpenSet.Count; i++)
                    {
                        if (lee.LargestGScore != 0)
                            spriteBatch.Draw(Pixelclass.Pixel, new Rectangle((lee.OpenSet[i].Location.Y * TILESIZE), (lee.OpenSet[i].Location.X * TILESIZE), TILESIZE, TILESIZE), Color.DarkCyan * 0.8f);

                    }
                    for (int i = 0; i < lee.CompleatedPath.Count; i++)
                        spriteBatch.Draw(Pixelclass.Pixel, new Rectangle((lee.CompleatedPath[i].Location.Y * TILESIZE) + (TILESIZE / 2) - TILESIZE / 8, (lee.CompleatedPath[i].Location.X * TILESIZE) + (TILESIZE / 2) - TILESIZE / 8, TILESIZE / 4, TILESIZE / 4), Color.Gold);

                }
                edit.DrawMe(spriteBatch);
                //edit.help = false;
                //runmaze = true;
                if (runningalgorythem)
                {
                    for (int i = 0; i < a_star.ClosedSet.Count; i++)
                    {
                        Rectangle temp = new Rectangle(a_star.ClosedSet[i].Location.Y * TILESIZE, a_star.ClosedSet[i].Location.X * TILESIZE, TILESIZE, TILESIZE);
                        if (temp.Contains(m_input.MouseIsHere()))
                        {

                            spriteBatch.Draw(Pixelclass.Pixel, new Rectangle(0, 0, 45, 34), Color.White);
                            spriteBatch.Draw(Pixelclass.Pixel, temp, Color.Purple * 0.5f);
                            spriteBatch.DrawString(Pixelclass.Tfont, "F: " + Math.Round(a_star.ClosedSet[i].F, 4) + "", new Vector2(0, 0), Color.Black);
                            spriteBatch.DrawString(Pixelclass.Tfont, "G: " + Math.Round(a_star.ClosedSet[i].G, 4) + "", new Vector2(0, 10), Color.Black);
                            spriteBatch.DrawString(Pixelclass.Tfont, "H: " + Math.Round(a_star.ClosedSet[i].H, 4) + "", new Vector2(0, 20), Color.Black);

                        }
                    }
                }
                else
                    for (int i = 0; i < lee.ClosedSet.Count; i++)
                    {
                        Rectangle temp = new Rectangle(lee.ClosedSet[i].Location.Y * TILESIZE, lee.ClosedSet[i].Location.X * TILESIZE, TILESIZE, TILESIZE);
                        if (temp.Contains(m_input.MouseIsHere()))
                        {

                            spriteBatch.Draw(Pixelclass.Pixel, new Rectangle(0, 0, 45, 34), Color.White);
                            spriteBatch.Draw(Pixelclass.Pixel, temp, Color.Purple * 0.5f);
                            spriteBatch.DrawString(Pixelclass.Tfont, "G: " + Math.Round(lee.ClosedSet[i].G, 4) + "", new Vector2(0, 10), Color.Black);

                        }
                    }


                if (edit.help || a_star.ValidPath == false)
                {

                    Rectangle r = new Rectangle(graphics.PreferredBackBufferWidth / 4, graphics.PreferredBackBufferHeight / 4, graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2);
                    spriteBatch.Draw(Pixelclass.Pixel, r, Color.Black * 0.65f);

                    if (a_star.ValidPath == false)
                    {
                        spriteBatch.DrawString(Pixelclass.Font, warning_string, new Vector2(graphics.PreferredBackBufferWidth / 2 - warning_string_Legnth.X, graphics.PreferredBackBufferHeight / 2 - warning_string_Legnth.Y), Color.White);
                    }
                    else
                    {
                        string warn;
                        warn = "To place the start point press A with mouse over a tile \nTo place the end  point press D with mouse over a tile\nTo add and remove walls use Left and Right click\nTo Reset the world press SPACE\nPress the Q and E Keys to change Tilesize\nAlgorithm will slow down at smaller tilesizes\nTo change algorithms Press W\nTo run the selected algorithm 50 times press S\nTo randomly fill 33% of the screen Press Z\nTo hide/show this message press H";
                        spriteBatch.DrawString(Pixelclass.Font, warn, new Vector2(Game1.graphics.PreferredBackBufferWidth / 2 - Pixelclass.Font.MeasureString(warn).X / 2, Game1.graphics.PreferredBackBufferHeight / 2 - Pixelclass.Font.MeasureString(warn).Y / 2), Color.White);
                    }


                }

               

                if (runningalgorythem)
                {
                    spriteBatch.Draw(Pixelclass.Pixel, new Rectangle(Game1.graphics.PreferredBackBufferWidth - (int)Pixelclass.Font.MeasureString("AStar").X - 1, 0, (int)Pixelclass.Font.MeasureString("AStar").X, (int)Pixelclass.Font.MeasureString("AStar").Y), Color.Black * 0.75f);
                    spriteBatch.DrawString(Pixelclass.Font, "AStar", new Vector2(Game1.graphics.PreferredBackBufferWidth - Pixelclass.Font.MeasureString("AStar").X - 1, 0), Color.White);
                    spriteBatch.Draw(Pixelclass.Pixel,
                        new Rectangle(Game1.graphics.PreferredBackBufferWidth - 300, (int)Pixelclass.Font.MeasureString("AStar").Y, 300, (int)Pixelclass.Font.MeasureString("AStar Took: " + RunningTime + " . Over " + counts + " Times").Y), Color.Black * 0.75f);
                    spriteBatch.DrawString(Pixelclass.Font, "AStar Took: " + RunningTime + " ms. Over " + counts + " Times", new Vector2(Game1.graphics.PreferredBackBufferWidth - 300, (int)Pixelclass.Font.MeasureString("AStar").Y), Color.White);
                }
                else
                {
                    spriteBatch.Draw(Pixelclass.Pixel, new Rectangle(Game1.graphics.PreferredBackBufferWidth - (int)Pixelclass.Font.MeasureString("Lee").X - 1, 0, (int)Pixelclass.Font.MeasureString("Lee").X, (int)Pixelclass.Font.MeasureString("Lee").Y), Color.Black * 0.75f);
                    spriteBatch.DrawString(Pixelclass.Font, "Lee", new Vector2(Game1.graphics.PreferredBackBufferWidth - Pixelclass.Font.MeasureString("Lee").X - 1, 0), Color.White);
                    spriteBatch.Draw(Pixelclass.Pixel, new Rectangle(Game1.graphics.PreferredBackBufferWidth - 300, (int)Pixelclass.Font.MeasureString("Lee").Y, 300, (int)Pixelclass.Font.MeasureString("Lee Took:" + RunningTime + " . Over " + counts + " Times").Y), Color.Black * 0.75f);
                    spriteBatch.DrawString(Pixelclass.Font, "Lee Took: " + RunningTime + " ms. Over " + counts + " Times", new Vector2(Game1.graphics.PreferredBackBufferWidth - 300, (int)Pixelclass.Font.MeasureString("Lee").Y), Color.White);
                }

                spriteBatch.Draw(Pixelclass.Pixel,
                       new Rectangle(Game1.graphics.PreferredBackBufferWidth - 300, (int)Pixelclass.Font.MeasureString("AStar").Y * 2, 300, (int)Pixelclass.Font.MeasureString("AStar Took: " + RunningTime + " . Over " + counts + " Times").Y), Color.Black * 0.75f);
                spriteBatch.DrawString(Pixelclass.Font, "Grid size :" + edit.Level.Map.GetLength(0) + "," + edit.Level.Map.GetLength(1), new Vector2(Game1.graphics.PreferredBackBufferWidth - 300, (int)Pixelclass.Font.MeasureString("AStar").Y * 2), Color.White);


            }
            spriteBatch.End();
            


            base.Draw(gameTime);
        }
    }
}
