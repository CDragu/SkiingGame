using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.IO;
using System.Diagnostics;
using Microsoft.Xna.Framework.Input.Touch;

namespace The_Magic_Scarf_And_The_Quest_for_Cheese
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        enum GameState// according to this states different objects will be visible on the screen
        {
            Comic,
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
        Texture2D cheesetexture;
        Texture2D bouldertexture;

        /// <summary>
        /// GameObjects
        /// </summary>
        Flags flag;
        int distacebetwenflags = 200;
        int screenHeight;

        SkyMan skyMan;
        Texture2D particle;

        SnowBolder boulder;
        Cheese cheese;

        Explosion explosion;
        SmallExplosion smallexplosion;

        /// <summary>
        /// Sound 
        /// </summary>
        //Stream soundfile;
        //SoundEffect soundEffect;
        //SoundEffectInstance soundEffectInstance;

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
        Texture2D Bondtexture;
        Texture2D GameOvertexture;
        Texture2D Arrow;
        Texture2D ArrowDown;
        Texture2D ArrowLeft;
        Texture2D ArrowRight;
        Texture2D Comicpage;
        Texture2D Goagain;

        SmartSprite Bond;
        SmartSprite Gameover;
        SmartSprite ComicPage;
        Buttons StartButton;
        Buttons ResumeButton;
        Buttons BackToStartButton;
        Buttons Save;
        Buttons Load;
        Buttons Enter;
        Buttons SkipComic;
        Buttons GoAgain;
        float timer;
        SpriteFont font;
        

        Buttons Up1;
        Buttons Up2;
        Buttons Down1;
        Buttons Down2;
        Buttons Left;
        Buttons Right; 

        /// <summary>
        /// Plaufield; list of sprites
        /// </summary>
        public PlayField field;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            //this.IsMouseVisible = true;

            graphics.PreferredBackBufferWidth = 272;//sets the width of the screen
            graphics.PreferredBackBufferHeight = 480;// sets the height of the screnn
        }


        protected override void Initialize()
        {

            base.Initialize();
        }


        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            field = new PlayField();
            //Textures
            flagRighttexture = Content.Load<Texture2D>("BlueFlag");
            flagLefttexture = Content.Load<Texture2D>("RedFlag");
            skyMantexture = Content.Load<Texture2D>("skymanrevampwhite");
            cheesetexture = Content.Load<Texture2D>("Cheese");
            bouldertexture = Content.Load<Texture2D>("Boulder");
            Bondtexture = Content.Load<Texture2D>("NewBond");
            GameOvertexture = Content.Load<Texture2D>("GameOverScreen");
            particle = Content.Load<Texture2D>("SnowParticle");
            Arrow = Content.Load<Texture2D>("Arrow");
            ArrowDown = Content.Load<Texture2D>("ArrowDown");
            ArrowLeft = Content.Load<Texture2D>("ArrowLeft");
            ArrowRight = Content.Load<Texture2D>("ArrowRight");
            Comicpage = Content.Load<Texture2D>("Comic Strip");
            //Initializing game objects
            screenHeight = GraphicsDevice.Viewport.Height;
            flag = new Flags(Vector2.Zero, 0.4f, flagRighttexture, 0, 1, field, screenHeight, distacebetwenflags, flagLefttexture);
            flag.SetupFlags(field);

            Texture2D[] Particles = new Texture2D[1];
            Particles[0] = particle;
            skyMan = new SkyMan(new Vector2(100, 100), 0.4f, skyMantexture, 0, 1, field, GraphicsDevice.Viewport.Height, GraphicsDevice.Viewport.Width, Particles);

            explosion = new Explosion(Particles, new Vector2(120, 460), 400, 1, 50);
            smallexplosion = new SmallExplosion(Particles, new Vector2(200, 200), 100, 1, 20);
            boulder = new SnowBolder(Vector2.Zero, 0.1f, bouldertexture, 0, 1, field, 20);
            boulder.InitializeBoulders(field);
            cheese = new Cheese(Vector2.Zero, 0.1f, cheesetexture, 0, 1, field, 10);
            cheese.Initializecheese(field);
            Bond = new SmartSprite(new Vector2(55, 90), 0.75f, Bondtexture, 0, 1, field);
            Gameover = new SmartSprite(new Vector2(-5, -50), 1, GameOvertexture, 0, 1, field);
            ComicPage = new SmartSprite(new Vector2(0, 0), 1, Comicpage, 0, 1, field);


            //Sounds
            //soundfile = TitleContainer.OpenStream(@"Content\buzz.wav");
            //soundEffect = SoundEffect.FromStream(soundfile);
            //soundEffectInstance = soundEffect.CreateInstance();

            //Buttons
            startButtonTexture = Content.Load<Texture2D>("Go");
            ResumeButtonTexture = Content.Load<Texture2D>("resume");
            BackToStartButtonTexture = Content.Load<Texture2D>("Back to Menu");
            SaveButtonTexture = Content.Load<Texture2D>("save");
            LoadButtonTexture = Content.Load<Texture2D>("load");
            EnterButtonTexture = Content.Load<Texture2D>("go");
            font = Content.Load<SpriteFont>("Pixel");
            Alphabet = Content.Load<Texture2D>("Alphabet v2");
            Goagain = Content.Load<Texture2D>("Go Again");

            StartButton = new Buttons(new Vector2(GraphicsDevice.Viewport.Width / 2 - startButtonTexture.Width / 2, GraphicsDevice.Viewport.Height / 2 + GraphicsDevice.Viewport.Height / 4 - startButtonTexture.Height / 2), 1, startButtonTexture, 0, 1, field, true);
            ResumeButton = new Buttons(new Vector2(GraphicsDevice.Viewport.Width / 2 - startButtonTexture.Width / 2 - 20, GraphicsDevice.Viewport.Height / 2 - GraphicsDevice.Viewport.Height / 4 - startButtonTexture.Height / 2), 0.5f, ResumeButtonTexture, 0, 1, field, false);
            BackToStartButton = new Buttons(new Vector2(GraphicsDevice.Viewport.Width / 2 - startButtonTexture.Width - 20, GraphicsDevice.Viewport.Height / 2 + GraphicsDevice.Viewport.Height / 4 + GraphicsDevice.Viewport.Height / 8 - startButtonTexture.Height / 2), 0.5f, BackToStartButtonTexture, 0, 1, field, false);
            Save = new Buttons(new Vector2(GraphicsDevice.Viewport.Width / 2 - startButtonTexture.Width / 2, GraphicsDevice.Viewport.Height / 2 + GraphicsDevice.Viewport.Height / 4 - startButtonTexture.Height / 2), 0.5f, SaveButtonTexture, 0, 1, field, false);
            Load = new Buttons(new Vector2(GraphicsDevice.Viewport.Width / 2 - startButtonTexture.Width / 2, GraphicsDevice.Viewport.Height / 2 - startButtonTexture.Height / 2), 0.5f, LoadButtonTexture, 0, 1, field, false);
            Enter = new Buttons(new Vector2(GraphicsDevice.Viewport.Width / 2 - startButtonTexture.Width / 2, GraphicsDevice.Viewport.Height / 2 + GraphicsDevice.Viewport.Height / 4 - startButtonTexture.Height / 2), 1, EnterButtonTexture, 0, 1, field, false);
            SkipComic = new Buttons(new Vector2(190, 430), 0.7f, startButtonTexture, 0, 1, field, true);
            GoAgain = new Buttons(new Vector2(GraphicsDevice.Viewport.Width / 2 - Goagain.Width / 2, GraphicsDevice.Viewport.Height / 2 + GraphicsDevice.Viewport.Height / 8 - Goagain.Height / 2), 1, Goagain, 0, 1, field, true);

            Up1 = new Buttons(new Vector2(10, 210), 0.2f, Arrow, 0, 1, field, false);
            Up2 = new Buttons(new Vector2(240, 210), 0.2f, Arrow, 0, 1, field, false);
            Down1 = new Buttons(new Vector2(10, 310), 0.2f, ArrowDown, 0, 1, field, false);
            Down2 = new Buttons(new Vector2(240, 310), 0.2f, ArrowDown, 0, 1, field, false);
            Left = new Buttons(new Vector2(10, 260), 0.2f, ArrowLeft, 0, 1, field, false);
            Right = new Buttons(new Vector2(240, 260), 0.2f, ArrowRight, 0, 1, field, false);

            currentGameState = (int)GameState.Start;
        }


        protected override void UnloadContent()
        {
        }

       
        KeyboardState keyboard;
        KeyboardState Oldkeyboard;
        bool pressed = false;
        protected override void Update(GameTime gameTime)
        {

            timer += 0.16f;//approximately the time it takes a frame at 30FPS
            GraphicsDevice.Clear(Color.Black);
            Oldkeyboard = keyboard;
            keyboard = Keyboard.GetState();
            MouseState mouse = Mouse.GetState();
            TouchCollection touchCollection = TouchPanel.GetState();
            Debug.WriteLine(mouse.X);
            Debug.WriteLine(mouse.Y);

            if (currentGameState == (int)GameState.Start)//Main Menu where you can press start or wait to go into Attract mode
            {
                GoAgain.Isvisible = false;
                Bond.Isvisible = true;
                GraphicsDevice.Clear(Color.Black);
                StartButton.Isvisible = true;
                SkipComic.Isvisible = false;
                if (StartButton.IsPressed(mouse, touchCollection) == true)//start the game
                {

                    Bond.Isvisible = false;
                    ComicPage.Isvisible = true;
                    StartButton.Isvisible = false;
                    SkipComic.Isvisible = true;
                    currentGameState = (int)GameState.Comic;

                }
                else if (timer > 50)//go to Attract mode
                {

                    skyMan.Reset();
                    flag.Reset(field);
                    boulder.Reset();
                    cheese.Reset();
                    StartButton.Isvisible = false;
                    flag.Isvisible = true;
                    skyMan.Isvisible = true;
                    flag.atractmode = true;
                    Bond.Isvisible = false;
                    currentGameState = (int)GameState.Atract;
                }
            }

            if (currentGameState == (int)GameState.Comic)// Display the comic and a button to go
            {

                //GraphicsDevice.Clear(Color.Black);
                SkipComic.Isvisible = true;
                ComicPage.Isvisible = true;
                if (SkipComic.IsPressed(mouse, touchCollection) == true)
                {
                    flag.Reset(field);
                    explosion.reset();
                    skyMan.Reset();
                    boulder.Reset();
                    cheese.Reset();
                    cheese.Isvisible = true;
                    boulder.Isvisible = true;                    
                    StartButton.Isvisible = false;
                    Bond.Isvisible = false;
                    explosion.isvisible = true;
                    ComicPage.Isvisible = false;
                    SkipComic.Isvisible = false;
                    skyMan.trail.isvisible = true;
                    Up1.Isvisible = true;
                    Up2.Isvisible = true;
                    Down1.Isvisible = true;
                    Down2.Isvisible = true;
                    Right.Isvisible = true;
                    Left.Isvisible = true;
                    currentGameState = (int)GameState.Game;
                   
                }
            }

            if (currentGameState == (int)GameState.Atract)// Attract mode for showing off a part of the game
            {
                GraphicsDevice.Clear(Color.Black);
                flag.UpdateInAttract();
                skyMan.UpdateInAttract();
                skyMan.trail.isvisible = true;
                if (keyboard.IsKeyDown(Keys.Space))// go back to Main Menu
                {
                    currentGameState = (int)GameState.Start;
                    timer = 0;

                    flag.Isvisible = false;
                    flag.atractmode = false;
                    skyMan.Isvisible = false;
                    StartButton.Isvisible = true;
                    skyMan.trail.isvisible = false ;
                }

            }


            if (currentGameState == (int)GameState.Game)//The Main Loop for the Gameplay
            {
                flag.Isvisible = true;
                skyMan.Isvisible = true;
                GraphicsDevice.Clear(Color.Black);
                flag.Update();
                
                skyMan.Update();
                boulder.Update();
                explosion.position.Y += 1f;
                if (explosion.position.Y < 1000)
                {
                    explosion.Update();
                }
                cheese.Update();

                for (int i = 0; i < flag.numberOfFlags; i++)//checks collisions with flags
                {
                    skyMan.PhysicsUpdate(skyMan, flag.Phizicalchildren[i]);
                }
                for (int i = 0; i < cheese.maxcheese; i++)//checks collisions with chees
                {
                    skyMan.PhysicsUpdate(skyMan, cheese.Phizicalchildren[i]);
                }
                for (int i = 0; i < boulder.maxboulders; i++)//checks collisions with boulders
                {
                    skyMan.PhysicsUpdate(skyMan, boulder.Phizicalchildren[i]);
                }



                if (Up1.IsPressed(mouse, touchCollection) && pressed == false)
                {
                    skyMan.up();
                    pressed = true;
                }
                if (Up2.IsPressed(mouse, touchCollection) && pressed == false)
                {
                    skyMan.up();
                    pressed = true;
                }
                if (Down1.IsPressed(mouse, touchCollection) && pressed == false)
                {
                    skyMan.down();
                    pressed = true;
                }
                if (Down2.IsPressed(mouse, touchCollection) && pressed == false)
                {
                    skyMan.down();
                    pressed = true;
                }
                if (Left.IsPressed(mouse, touchCollection))
                {
                    skyMan.left();
                    pressed = true;
                }
                if (Right.IsPressed(mouse, touchCollection))
                {
                    skyMan.right();
                    pressed = true;
                }
                pressed = false;

                if (skyMan.lives <= 0)//Moves game to the Insert name Screen
                {
                    skyMan.trail.isvisible = false;
                    skyMan.Isvisible = false;
                    flag.Isvisible = false;
                    GraphicsDevice.Clear(Color.Blue);
                    cheese.Isvisible = false;
                    boulder.Isvisible = false;
                    explosion.isvisible = false;
                    Up1.Isvisible = false;
                    Up2.Isvisible = false;
                    Down1.Isvisible = false;
                    Down2.Isvisible = false;
                    Left.Isvisible = false;
                    Right.Isvisible = false;
                    Gameover.Isvisible = true;
                    
                    currentGameState = (int)GameState.GameOver;
                }
                if (keyboard.IsKeyDown(Keys.Escape))//Opens the Pause menu
                {
                    cheese.Isvisible = false;
                    boulder.Isvisible = false;
                    skyMan.Isvisible = false;
                    flag.Isvisible = false;
                    explosion.isvisible = false;
                    Up1.Isvisible = false;
                    Up2.Isvisible = false;
                    Down1.Isvisible = false;
                    Down2.Isvisible = false;
                    Left.Isvisible = false;
                    Right.Isvisible = false;
                    currentGameState = (int)GameState.Pause;
                }

            }



            if (currentGameState == (int)GameState.Pause)//menu for saving, loading and going back to main menu
            {
               
                ResumeButton.Isvisible = true;
                BackToStartButton.Isvisible = true;
               
                if (ResumeButton.IsPressed(mouse, touchCollection) == true)//gose back to game mode
                {
                   
                    ResumeButton.Isvisible = false;
                    BackToStartButton.Isvisible = false;
                    cheese.Isvisible = true;
                    boulder.Isvisible = true;
                    Up1.Isvisible = true;
                    Up2.Isvisible = true;
                    Down1.Isvisible = true;
                    Down2.Isvisible = true;
                    Left.Isvisible = true;
                    Right.Isvisible = true;
                    currentGameState = (int)GameState.Game;
                }
                if (BackToStartButton.IsPressed(mouse, touchCollection) == true)//back to main menu
                {
                    Save.Isvisible = false;
                    Load.Isvisible = false;
                    ResumeButton.Isvisible = false;
                    BackToStartButton.Isvisible = false;
                    currentGameState = (int)GameState.Start;
                    timer = 0;
                }

            }

            if (currentGameState == (int)GameState.GameOver)//Screen for name input, when is done go to Attract mode
            {

                Gameover.Isvisible = true;
                GoAgain.Isvisible = true;
                if (GoAgain.IsPressed(mouse, touchCollection) == true)
                {
                    timer = 0;
                    currentGameState = (int)GameState.Start;
                    StartButton.Isvisible = true;
                    GoAgain.Isvisible = false;
                    Gameover.Isvisible = false;
                    skyMan.Reset();
                }
               

            }

        
    
            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)//draws the screen
        {
            spriteBatch.Begin();

            StartButton.Draw(spriteBatch);
            ResumeButton.Draw(spriteBatch);
            BackToStartButton.Draw(spriteBatch);
            Save.Draw(spriteBatch);
            Load.Draw(spriteBatch);
            Enter.Draw(spriteBatch);
            flag.Draw(spriteBatch);
            
          
            cheese.Draw(spriteBatch);
            boulder.Draw(spriteBatch);
            Bond.Draw(spriteBatch);
            Gameover.Draw(spriteBatch);
            explosion.Draw(spriteBatch);
            skyMan.DrawWithAnimation(spriteBatch);
            skyMan.DrawScore(spriteBatch, font);

            Up1.Draw(spriteBatch);
            Up2.Draw(spriteBatch);
            Down1.Draw(spriteBatch);
            Down2.Draw(spriteBatch);
            Left.Draw(spriteBatch);
            Right.Draw(spriteBatch);
            ComicPage.Draw(spriteBatch);
            SkipComic.Draw(spriteBatch);
            GoAgain.Draw(spriteBatch);


            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
