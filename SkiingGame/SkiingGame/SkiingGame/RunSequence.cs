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
        int currentGameState;

        Texture2D flagRighttexture;
        Texture2D flagLefttexture;
        Texture2D skyMantexture;
       
        
        Stream soundfile;
        SoundEffect soundEffect;
        SoundEffectInstance soundEffectInstance;

        /// <summary>
        /// Menus
        /// </summary>
        Buttons StartButton;
        Texture2D startButtonTexture;

        public PlayField field;

        public RunSequence()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
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
            //Sounds
            soundfile = TitleContainer.OpenStream(@"Content\buzz.wav");
            soundEffect = SoundEffect.FromStream(soundfile);
            soundEffectInstance = soundEffect.CreateInstance();

            //Buttons
            startButtonTexture = Content.Load<Texture2D>("StartButton");
            StartButton = new Buttons(new Vector2(100,100), 1, startButtonTexture, 0, 0, field,true);//temporal
            currentGameState = (int)GameState.Start;           
        }

       
        protected override void UnloadContent()
        {
        }

       
        protected override void Update(GameTime gameTime)
        {           
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            KeyboardState keyboard = Keyboard.GetState();            
            if(currentGameState == (int)GameState.Start)
            {
                StartButton.Isvisible = true;
                if(StartButton.IsPressed() == true)
                {
                    currentGameState = (int)GameState.Game;
                }
            }
            if (currentGameState == (int)GameState.Atract)
            {

            }
            if (currentGameState == (int)GameState.Game)
            {

            }
            if (currentGameState == (int)GameState.Pause)
            {

            }
            if (currentGameState == (int)GameState.GameOver)
            {

            }
            base.Update(gameTime);
        }

       
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            spriteBatch.Begin();
            StartButton.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
