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

        Texture2D flagRighttexture;
        Texture2D flagLefttexture;
        Texture2D skyMantexture;
       
        
        Stream soundfile;
        SoundEffect soundEffect;
        SoundEffectInstance soundEffectInstance;


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

            flagRighttexture = Content.Load<Texture2D>("LeftBlueflag");
            flagLefttexture = Content.Load<Texture2D>("leftRedFlag");
            skyMantexture = Content.Load<Texture2D>("Skier");
            
            soundfile = TitleContainer.OpenStream(@"Content\buzz.wav");
            soundEffect = SoundEffect.FromStream(soundfile);
            soundEffectInstance = soundEffect.CreateInstance();            
        }

       
        protected override void UnloadContent()
        {
        }

       
        protected override void Update(GameTime gameTime)
        {           
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            KeyboardState keyboard = Keyboard.GetState();            

            base.Update(gameTime);
        }

       
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
           
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
