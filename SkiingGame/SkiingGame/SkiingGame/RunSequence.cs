using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Storage;
using System.IO;
using System.Diagnostics;

namespace SkiingGame
{
   
    public class RunSequence : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        
        enum GameState
        {
            Start,
            Atract,
            Pause,
            GameOver,
            Game,
        }
        public int currentGameState;

        Texture2D flagRighttexture;
        Texture2D flagLefttexture;
        Texture2D skyMantexture;

        Flags flag;
        int distacebetwenflags = 200;
        int screenHeight;

        SkyMan skyMan;

        Stream soundfile;
        SoundEffect soundEffect;
        SoundEffectInstance soundEffectInstance;

        /// <summary>
        /// Menus
        /// </summary>        
        Texture2D startButtonTexture;
        Texture2D ResumeButtonTexture;
        Texture2D BackToStartButtonTexture;
        Texture2D SaveButtonTexture;
        Texture2D LoadButtonTexture;
        Texture2D EnterButtonTexture;
        Texture2D Alphabet;

        Buttons StartButton;
        Buttons ResumeButton;
        Buttons BackToStartButton;
        Buttons Save;
        Buttons Load;
        Buttons Enter;
        float timer;
        SpriteFont font;
        Letter[] Letters;
        
        /// <summary>
        /// Score
        /// </summary>
        public struct Score
        {
            public string PlayerName;
            public int PlayerScore;
        }
        List<Score> Scores = new List<Score>();

        public PlayField field;

        public RunSequence()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            this.IsMouseVisible = true;
            graphics.PreferredBackBufferWidth = 272;
            graphics.PreferredBackBufferHeight = 480;
        }

       
        protected override void Initialize()
        {
            
            base.Initialize();
        }

       
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            field = new PlayField();
            //Objects
            flagRighttexture = Content.Load<Texture2D>("LeftBlueflag");
            flagLefttexture = Content.Load<Texture2D>("leftRedFlag");
            skyMantexture = Content.Load<Texture2D>("Skier");

            screenHeight = GraphicsDevice.Viewport.Height;
            flag = new Flags(Vector2.Zero, 0.2f, flagRighttexture, 0, 1, field, screenHeight, distacebetwenflags);
            flag.SetupFlags(field);

            skyMan = new SkyMan(new Vector2(100,100), 0.1f, skyMantexture, 0, 1, field);

            //Sounds
            soundfile = TitleContainer.OpenStream(@"Content\buzz.wav");
            soundEffect = SoundEffect.FromStream(soundfile);
            soundEffectInstance = soundEffect.CreateInstance();

            //Buttons
            startButtonTexture = Content.Load<Texture2D>("StartButton");
            ResumeButtonTexture = Content.Load<Texture2D>("StartButton");
            BackToStartButtonTexture = Content.Load<Texture2D>("StartButton");
            SaveButtonTexture = Content.Load<Texture2D>("StartButton");
            LoadButtonTexture = Content.Load<Texture2D>("StartButton");
            EnterButtonTexture = Content.Load<Texture2D>("StartButton");
            font = Content.Load<SpriteFont>("Pixel");
            Alphabet = Content.Load<Texture2D>("Alphabet v2");

            StartButton = new Buttons(new Vector2(GraphicsDevice.Viewport.Width / 2 - startButtonTexture.Width / 2, GraphicsDevice.Viewport.Height / 2 + GraphicsDevice.Viewport.Height / 4 - startButtonTexture.Height / 2), 1, startButtonTexture, 0, 1, field, true);
            ResumeButton = new Buttons(new Vector2(GraphicsDevice.Viewport.Width / 2 - startButtonTexture.Width / 2, GraphicsDevice.Viewport.Height / 2 - GraphicsDevice.Viewport.Height / 4 - startButtonTexture.Height / 2), 1, startButtonTexture, 0, 1, field, false);
            BackToStartButton = new Buttons(new Vector2(GraphicsDevice.Viewport.Width / 2 - startButtonTexture.Width / 2, GraphicsDevice.Viewport.Height / 2 + GraphicsDevice.Viewport.Height / 4 + GraphicsDevice.Viewport.Height / 4 - startButtonTexture.Height / 2), 1, startButtonTexture, 0, 1, field, false);
            Save = new Buttons(new Vector2(GraphicsDevice.Viewport.Width / 2 - startButtonTexture.Width / 2, GraphicsDevice.Viewport.Height / 2 + GraphicsDevice.Viewport.Height / 4 - startButtonTexture.Height / 2), 1, startButtonTexture, 0, 1, field, false);
            Load = new Buttons(new Vector2(GraphicsDevice.Viewport.Width / 2 - startButtonTexture.Width / 2, GraphicsDevice.Viewport.Height / 2 - startButtonTexture.Height / 2), 1, startButtonTexture, 0, 1, field, false);
            Enter = new Buttons(new Vector2(GraphicsDevice.Viewport.Width / 2 - startButtonTexture.Width / 2, GraphicsDevice.Viewport.Height / 2 + GraphicsDevice.Viewport.Height / 4 - startButtonTexture.Height / 2), 1, startButtonTexture, 0, 1, field, false);
            Letters = new Letter[3];
            for(int i = 0; i < 3; i++)
            {
                Letters[i] = new Letter(new Vector2(GraphicsDevice.Viewport.Width / 4 * (i + 1) - 20, GraphicsDevice.Viewport.Height / 2 - startButtonTexture.Height / 2), 1, Alphabet, null, 0, 1, field);
            }

            //LoadTheScores
            //try
            //{
            //    SaveLoad load = new SaveLoad(field, "LOAD", Scores);
            //    Scores = load.ReturnScores();
            //}
            //catch
            //{

            //}


            currentGameState = (int)GameState.Start;           
        }

       
        protected override void UnloadContent()
        {
        }

        int count = 0;
        KeyboardState keyboard;
        KeyboardState Oldkeyboard;
        protected override void Update(GameTime gameTime)
        {
           
            timer += 0.16f;//to be changed
            GraphicsDevice.Clear(Color.Black);
            Oldkeyboard = keyboard;
            keyboard = Keyboard.GetState();
            
            MouseState mouse = Mouse.GetState();
            Debug.WriteLine(mouse.X);
            Debug.WriteLine(mouse.Y);

            if (currentGameState == (int)GameState.Start)
            {
                
                GraphicsDevice.Clear(Color.Black);
                StartButton.Isvisible = true;
                if (StartButton.IsPressed(mouse) == true)
                {

                    flag.Reset(field);
                    skyMan.Reset();
                    StartButton.Isvisible = false;
                    currentGameState = (int)GameState.Game;
                    Debug.Write("yes");
                } else if (timer > 50)
                {
                    SaveLoad load = new SaveLoad(field, "LOAD", Scores);
                    Scores = load.ReturnScores();
                    skyMan.Reset();
                    flag.Reset(field);
                    StartButton.Isvisible = false;
                    flag.Isvisible = true;
                    skyMan.Isvisible = true;
                    flag.atractmode = true;
                    currentGameState = (int)GameState.Atract;
                }
            }

            
            if (currentGameState == (int)GameState.Atract)
            {
                GraphicsDevice.Clear(Color.Green);
                flag.UpdateInAttract();
                skyMan.UpdateInAttract();
                if(keyboard.IsKeyDown(Keys.Space))
                {
                    currentGameState = (int)GameState.Start;
                    timer = 0;
                    Debug.Write("yes");
                    flag.Isvisible = false;
                    flag.atractmode = false;
                    skyMan.Isvisible = false;
                }

            }


            if (currentGameState == (int)GameState.Game)
            {
                flag.Isvisible = true;
                skyMan.Isvisible = true;
                GraphicsDevice.Clear(Color.Green);
                flag.Update();
                skyMan.Update();
                for(int i= 0; i < flag.numberOfFlags; i++)
                {
                    skyMan.PhysicsUpdate(skyMan, flag.Phizicalchildren[i]) ;
                }
                if(skyMan.lives <= 0)
                {

                    skyMan.Isvisible = false;
                    flag.Isvisible = false;
                    GraphicsDevice.Clear(Color.Blue);

                    Enter.Isvisible = true;
                    for (int i = 0; i < 3; i++)
                    {
                        Letters[i].Isvisible = true;
                    }
                    currentGameState = (int)GameState.GameOver;
                }
                if (keyboard.IsKeyDown(Keys.Escape))
                {
                    skyMan.Isvisible = false;
                    flag.Isvisible = false;
                    currentGameState = (int)GameState.Pause;
                }
                if (keyboard.IsKeyDown(Keys.N))
                {
                    skyMan.Isvisible = false;
                    flag.Isvisible = false;
                    GraphicsDevice.Clear(Color.Blue);

                    Enter.Isvisible = true;
                    for (int i = 0; i < 3; i++)
                    {
                        Letters[i].Isvisible = true;                       
                    }
                    currentGameState = (int)GameState.GameOver;
                }
            }



            if (currentGameState == (int)GameState.Pause)
            {
                Save.Isvisible = true;
                Load.Isvisible = true;
                ResumeButton.Isvisible = true;
                BackToStartButton.Isvisible = true;
                if (Save.IsPressed(mouse) == true)
                {
                    SaveLoad save = new SaveLoad(field, "SAVE", Scores);
                }
                if (Load.IsPressed(mouse) == true)
                {
                    SaveLoad load = new SaveLoad(field, "LOAD",Scores);
                    field = load.AfterLoad();
                }
                if (ResumeButton.IsPressed(mouse) == true)
                {
                    Save.Isvisible = false;
                    Load.Isvisible = false;
                    ResumeButton.Isvisible = false;
                    BackToStartButton.Isvisible = false;
                    currentGameState = (int)GameState.Game;
                }
                if (BackToStartButton.IsPressed(mouse) == true)
                {
                    Save.Isvisible = false;
                    Load.Isvisible = false;
                    ResumeButton.Isvisible = false;
                    BackToStartButton.Isvisible = false;
                    currentGameState = (int)GameState.Start;
                    timer = 0;
                }
               
            }
            
            if (currentGameState == (int)GameState.GameOver)
            {
                
                for (int i = 0; i < 3; i++)
                {                   
                    Letters[i].Update();
                }
                GraphicsDevice.Clear(Color.Blue);
             
                if (keyboard.IsKeyDown(Keys.Down) || keyboard.IsKeyDown(Keys.S) && (!keyboard.Equals(Oldkeyboard)))
                {
                   Letters[count].Up();
                }
                if (keyboard.IsKeyDown(Keys.Up) || keyboard.IsKeyDown(Keys.W) && (!keyboard.Equals(Oldkeyboard)))
                {
                    Letters[count].Down();                  
                }
                if (keyboard.IsKeyDown(Keys.Enter) && (!keyboard.Equals(Oldkeyboard)))
                {
                    count++;
                }
                

                
                if (count == 3)
                {
                    flag.Reset(field);
                    
                    count = 0;
                    Enter.Isvisible = false;
                    currentGameState = (int)GameState.Atract;
                    
                    flag.Isvisible = true;
                    skyMan.Isvisible = true;
                    flag.atractmode = true;
                    
                    for (int i = 0; i < 3; i++)
                    {
                        Letters[i].Isvisible = false;
                        skyMan.name += Letters[i].CurrentLetter();
                    }
                    Score currentscore = new Score();
                    currentscore.PlayerName = skyMan.name;
                    currentscore.PlayerScore = skyMan.score;
                    Scores.Add(currentscore);
                    Scores.OrderBy(c => c.PlayerScore);
                    SaveLoad save = new SaveLoad(field, "SAVE", Scores);
                    skyMan.Reset();
                    
                }

            }
           
            base.Update(gameTime);
        }

       
        protected override void Draw(GameTime gameTime)
        {
            

            spriteBatch.Begin();
            StartButton.Draw(spriteBatch); 
            ResumeButton.Draw(spriteBatch);
            BackToStartButton.Draw(spriteBatch);
            Save.Draw(spriteBatch);
            Load.Draw(spriteBatch);
            Enter.Draw(spriteBatch);
            skyMan.Draw(spriteBatch);
            skyMan.DrawScore(spriteBatch, font);
            flag.Draw(spriteBatch);
            flag.DrawInAttract(Scores, spriteBatch, font);
            for (int i = 0; i < 3; i++)
            {
                Letters[i].DrawWithAnimation(spriteBatch);
            }

            spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}
