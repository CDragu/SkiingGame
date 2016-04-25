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

        Buttons StartButton;
        Buttons ResumeButton;
        Buttons BackToStartButton;
        Buttons Save;
        Buttons Load;
        Buttons Enter;
        float timer;

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

            skyMan = new SkyMan(Vector2.Zero, 0.1f, skyMantexture, 0, 1, field);

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

            StartButton = new Buttons(new Vector2(GraphicsDevice.Viewport.Width / 2 - startButtonTexture.Width / 2, GraphicsDevice.Viewport.Height / 2 + GraphicsDevice.Viewport.Height / 4 - startButtonTexture.Height / 2), 1, startButtonTexture, 0, 1, field, true);
            ResumeButton = new Buttons(new Vector2(GraphicsDevice.Viewport.Width / 2 - startButtonTexture.Width / 2, GraphicsDevice.Viewport.Height / 2 - GraphicsDevice.Viewport.Height / 4 - startButtonTexture.Height / 2), 1, startButtonTexture, 0, 1, field, false);
            BackToStartButton = new Buttons(new Vector2(GraphicsDevice.Viewport.Width / 2 - startButtonTexture.Width / 2, GraphicsDevice.Viewport.Height / 2 + GraphicsDevice.Viewport.Height / 4 + GraphicsDevice.Viewport.Height / 4 - startButtonTexture.Height / 2), 1, startButtonTexture, 0, 1, field, false);
            Save = new Buttons(new Vector2(GraphicsDevice.Viewport.Width / 2 - startButtonTexture.Width / 2, GraphicsDevice.Viewport.Height / 2 + GraphicsDevice.Viewport.Height / 4 - startButtonTexture.Height / 2), 1, startButtonTexture, 0, 1, field, false);
            Load = new Buttons(new Vector2(GraphicsDevice.Viewport.Width / 2 - startButtonTexture.Width / 2, GraphicsDevice.Viewport.Height / 2 - startButtonTexture.Height / 2), 1, startButtonTexture, 0, 1, field, false);
            Enter = new Buttons(new Vector2(GraphicsDevice.Viewport.Width / 2 - startButtonTexture.Width / 2, GraphicsDevice.Viewport.Height / 2 + GraphicsDevice.Viewport.Height / 4 - startButtonTexture.Height / 2), 1, startButtonTexture, 0, 1, field, false);

            currentGameState = (int)GameState.Start;           
        }

       
        protected override void UnloadContent()
        {
        }

       
        protected override void Update(GameTime gameTime)
        {
            timer += 0.16f;//to be changed
            GraphicsDevice.Clear(Color.Black);
            KeyboardState keyboard = Keyboard.GetState();
            MouseState mouse = Mouse.GetState();
            Debug.WriteLine(mouse.X);
            Debug.WriteLine(mouse.Y);

            if (currentGameState == (int)GameState.Start)
            {
                GraphicsDevice.Clear(Color.Black);
                StartButton.Isvisible = true;
                if (StartButton.IsPressed(mouse) == true)
                {
                    StartButton.Isvisible = false;
                    currentGameState = (int)GameState.Game;
                    Debug.Write("yes");
                } else if (timer > 50)
                {
                    StartButton.Isvisible = false;
                    currentGameState = (int)GameState.Atract;
                }
            }
            if (currentGameState == (int)GameState.Atract)
            {
                GraphicsDevice.Clear(Color.Green);
                StartButton.Isvisible = false;
                if(keyboard.IsKeyDown(Keys.Space))
                {
                    currentGameState = (int)GameState.Start;
                    timer = 0;
                    Debug.Write("yes");
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
                    SaveLoad save = new SaveLoad(field, "SAVE");
                }
                if (Load.IsPressed(mouse) == true)
                {
                    SaveLoad load = new SaveLoad(field, "LOAD");
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
                Enter.Isvisible = true;
                GraphicsDevice.Clear(Color.Blue);
                if (Enter.IsPressed(mouse) == true)
                {
                    SaveLoad save = new SaveLoad(field, "SAVE");
                    Enter.Isvisible = false;
                    currentGameState = (int)GameState.Atract;
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
            flag.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
