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
        
        enum GameState// according to this states different objects will be visible on the screen
        {
            Win,
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
        Texture2D rockettexture;
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
        Rocket rokets;

        Explosion explosion;
        SmallExplosion smallexplosion;

        /// <summary>
        /// Sound 
        /// </summary>
        Stream soundfile;
        SoundEffect soundEffect;
        SoundEffectInstance soundEffectInstance;
        SoundEffectInstance[] soundEffects;

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
        Texture2D Comicpage;

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
        float timer;
        float timetillwin;
        bool gamefinished = false;
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

        /// <summary>
        /// Plaufield; list of sprites
        /// </summary>
        public PlayField field;

        public RunSequence()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            this.IsMouseVisible = true;

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
            rockettexture = Content.Load<Texture2D>("Rocket");
            bouldertexture = Content.Load<Texture2D>("Boulder");
            Bondtexture = Content.Load<Texture2D>("NewBond");
            GameOvertexture = Content.Load<Texture2D>("GameOverScreen");
            particle = Content.Load<Texture2D>("SnowParticle");
            Comicpage = Content.Load<Texture2D>("Comic Strip");

            //Sounds
            soundfile = TitleContainer.OpenStream(@"Content\Explosion2.wav");
            soundEffect = SoundEffect.FromStream(soundfile);
            soundEffectInstance = soundEffect.CreateInstance();
            SoundEffect[] soundEffects = new SoundEffect[5];// add sounds that you want to use here and then you can use them in skyman
            soundEffects[0] = soundEffect;

            //Initializing game objects
            screenHeight = GraphicsDevice.Viewport.Height;
            flag = new Flags(Vector2.Zero, 0.4f, flagRighttexture, 0, 1, field, screenHeight, distacebetwenflags, flagLefttexture);
            flag.SetupFlags(field);

            Texture2D[] Particles = new Texture2D[1];
            Particles[0] = particle;
            skyMan = new SkyMan(new Vector2(100, 100), 0.4f, skyMantexture,soundEffects, 0, 1, field, GraphicsDevice.Viewport.Height, GraphicsDevice.Viewport.Width, Particles);

            SaveLoad load = new SaveLoad(field, "LOAD", Scores, skyMan.scoreInfo);// ideal place to put the load so that the highscores will be persistent but the places of other objects wont be disturbed
            Scores = load.ReturnScores();

            explosion = new Explosion(Particles, new Vector2(120, 460), 400, 1, 50);
            smallexplosion = new SmallExplosion(Particles, new Vector2(200, 200), 100, 1, 20);
            boulder = new SnowBolder(Vector2.Zero, 0.1f, bouldertexture, 0, 1, field, 20);
            boulder.InitializeBoulders(field);
            cheese = new Cheese(Vector2.Zero, 0.1f, cheesetexture, 0, 1, field, 10);
            cheese.Initializecheese(field);
            rokets = new Rocket(Vector2.Zero, 20, rockettexture, 0, 1, field, 10);
            rokets.Initialize(field);
            Bond = new SmartSprite(new Vector2(55, 90), 0.75f, Bondtexture, 0, 1, field);
            Gameover = new SmartSprite(new Vector2(-5,-50), 1, GameOvertexture, 0, 1, field);
            ComicPage = new SmartSprite(new Vector2(0, 0), 1, Comicpage, 0, 1, field);


          

            //Buttons
            startButtonTexture = Content.Load<Texture2D>("Go");
            ResumeButtonTexture = Content.Load<Texture2D>("resume");
            BackToStartButtonTexture = Content.Load<Texture2D>("Back to Menu");
            SaveButtonTexture = Content.Load<Texture2D>("save");
            LoadButtonTexture = Content.Load<Texture2D>("load");
            EnterButtonTexture = Content.Load<Texture2D>("go");
            font = Content.Load<SpriteFont>("Pixel");
            Alphabet = Content.Load<Texture2D>("Alphabet v2");

            StartButton = new Buttons(new Vector2(GraphicsDevice.Viewport.Width / 2 - startButtonTexture.Width / 2, GraphicsDevice.Viewport.Height / 2 + GraphicsDevice.Viewport.Height / 4 - startButtonTexture.Height / 2), 1, startButtonTexture, 0, 1, field, true);
            ResumeButton = new Buttons(new Vector2(GraphicsDevice.Viewport.Width / 2 - startButtonTexture.Width /2 - 20, GraphicsDevice.Viewport.Height / 2 - GraphicsDevice.Viewport.Height / 4 - startButtonTexture.Height / 2), 0.5f, ResumeButtonTexture, 0, 1, field, false);
            BackToStartButton = new Buttons(new Vector2(GraphicsDevice.Viewport.Width / 2 - startButtonTexture.Width-20 , GraphicsDevice.Viewport.Height / 2 + GraphicsDevice.Viewport.Height / 4 + GraphicsDevice.Viewport.Height / 8 - startButtonTexture.Height / 2), 0.5f, BackToStartButtonTexture, 0, 1, field, false);
            Save = new Buttons(new Vector2(GraphicsDevice.Viewport.Width / 2 - startButtonTexture.Width / 2, GraphicsDevice.Viewport.Height / 2 + GraphicsDevice.Viewport.Height / 4 - startButtonTexture.Height / 2), 0.5f, SaveButtonTexture, 0, 1, field, false);
            Load = new Buttons(new Vector2(GraphicsDevice.Viewport.Width / 2 - startButtonTexture.Width / 2, GraphicsDevice.Viewport.Height / 2 - startButtonTexture.Height / 2), 0.5f, LoadButtonTexture, 0, 1, field, false);
            Enter = new Buttons(new Vector2(GraphicsDevice.Viewport.Width / 2 - startButtonTexture.Width / 2, GraphicsDevice.Viewport.Height / 2 + GraphicsDevice.Viewport.Height / 4 - startButtonTexture.Height / 2), 1, EnterButtonTexture, 0, 1, field, false);
            Letters = new Letter[3];
            for(int i = 0; i < 3; i++)
            {
                Letters[i] = new Letter(new Vector2(GraphicsDevice.Viewport.Width / 4 * (i + 1) - 20, GraphicsDevice.Viewport.Height / 2 - startButtonTexture.Height / 2), 1, Alphabet, null, 0, 1, field);
            }
            SkipComic = new Buttons(new Vector2(190, 430), 0.7f, startButtonTexture, 0, 1, field, true);

            currentGameState = (int)GameState.Start;
            //SaveLoad load = new SaveLoad(field, "LOAD", Scores, skyMan.scoreInfo);
            //Scores = load.ReturnScores();
        }

       
        protected override void UnloadContent()
        {
        }

        int count = 0;
        KeyboardState keyboard;
        KeyboardState Oldkeyboard;
        protected override void Update(GameTime gameTime)
        {
           
            timer += 0.16f;//approximately the time it takes a frame at 30FPS
            GraphicsDevice.Clear(Color.Black);
            Oldkeyboard = keyboard;
            keyboard = Keyboard.GetState();           
            MouseState mouse = Mouse.GetState();
            Debug.WriteLine(mouse.X);
            Debug.WriteLine(mouse.Y);

            if (currentGameState == (int)GameState.Start)//Main Menu where you can press start or wait to go into Attract mode
            {
                //Implementation of the small explosive animation, call reset to start the animation again
                //smallexplosion.reset();
                //smallexplosion.isvisible = true;
                //smallexplosion.Update();


                Bond.Isvisible = true;
                GraphicsDevice.Clear(Color.Black);
                StartButton.Isvisible = true;
                if (StartButton.IsPressed(mouse) == true)//start the game
                {
                    Bond.Isvisible = false;
                    ComicPage.Isvisible = true;
                    StartButton.Isvisible = false;
                    SkipComic.Isvisible = true;
                    currentGameState = (int)GameState.Comic;
                    Debug.Write("yes");
                } else if (timer > 50)//go to Attract mode
                {

                    SaveLoad load = new SaveLoad(field, "LOAD", Scores, skyMan.scoreInfo);
                    Scores = load.ReturnScores();
                    skyMan.Reset();
                    flag.Reset(field);
                    boulder.Reset();
                    cheese.Reset();
                    rokets.Reset();
                    StartButton.Isvisible = false;
                    flag.Isvisible = true;
                    skyMan.Isvisible = true;
                    flag.atractmode = true;
                    Bond.Isvisible = false;
                    currentGameState = (int)GameState.Atract;
                }
            }

            if(currentGameState == (int)GameState.Comic)// Display the comic and a button to go
            {
               
                //GraphicsDevice.Clear(Color.Black);
                SkipComic.Isvisible = true;
                ComicPage.Isvisible = true;
                if (SkipComic.IsPressed(mouse) == true)
                {
                    flag.Reset(field);
                    explosion.reset();
                    skyMan.Reset();
                    boulder.Reset();
                    cheese.Reset();
                    cheese.Isvisible = true;
                    rokets.Reset();
                    rokets.Isvisible = true;
                    boulder.Isvisible = true;
                    StartButton.Isvisible = false;
                    Bond.Isvisible = false;
                    explosion.isvisible = true;
                    ComicPage.Isvisible = false;
                    SkipComic.Isvisible = false;
                    currentGameState = (int)GameState.Game;
                    SaveLoad save = new SaveLoad(field, "SAVE", Scores, skyMan.scoreInfo);
                }
            }
            
            if (currentGameState == (int)GameState.Atract)// Attract mode for showing off a part of the game
            {
                GraphicsDevice.Clear(Color.Black);
                flag.UpdateInAttract();
                skyMan.UpdateInAttract();
                if(keyboard.IsKeyDown(Keys.Space))// go back to Main Menu
                {
                    currentGameState = (int)GameState.Start;
                    timer = 0;
                    Debug.Write("yes");
                    flag.Isvisible = false;
                    flag.atractmode = false;
                    skyMan.Isvisible = false;
                    StartButton.Isvisible = true;
                }

            }


            if (currentGameState == (int)GameState.Game)//The Main Loop for the Gameplay
            {
                timetillwin += 0.16f;
                flag.Isvisible = true;
                skyMan.Isvisible = true;
                GraphicsDevice.Clear(Color.Black);
                flag.Update();
                flag.IncreaseScore(skyMan);
                skyMan.Update();
                boulder.Update();
                explosion.position.Y += 1f;
                if(explosion.position.Y < 1000)
                {
                    explosion.Update();
                }
                if(timetillwin > 800 && gamefinished == false)
                {
                    cheese.Isvisible = false;
                    rokets.Isvisible = false;
                    boulder.Isvisible = false;
                    skyMan.Isvisible = false;
                    flag.Isvisible = false;
                    explosion.isvisible = false;
                    currentGameState = (int)GameState.Win;
                }
                

                cheese.Update();
                rokets.Update();
                
                for (int i= 0; i < flag.numberOfFlags; i++)//checks collisions with flags
                {
                    skyMan.PhysicsUpdate(skyMan, flag.Phizicalchildren[i]) ;
                }
                for (int i = 0; i < cheese.maxcheese; i++)//checks collisions with chees
                {
                    skyMan.PhysicsUpdate(skyMan, cheese.Phizicalchildren[i]);
                }
                for( int i = 0; i < rokets.maxrockets; i++)
                {
                    skyMan.PhysicsUpdate(skyMan, rokets.Phizicalchildren[i]);
                }
                for (int i = 0; i < boulder.maxboulders; i++)//checks collisions with boulders
                {
                    skyMan.PhysicsUpdate(skyMan, boulder.Phizicalchildren[i]);
                }
                if (keyboard.IsKeyDown(Keys.N))//starting a new game
                {

                    flag.Reset(field);
                    explosion.reset();
                    skyMan.Reset();
                    boulder.Reset();
                    cheese.Reset();
                    cheese.Isvisible = true;
                    rokets.Reset();
                    rokets.Isvisible = true;
                    boulder.Isvisible = true;
                    StartButton.Isvisible = false;
                    Bond.Isvisible = false;
                    explosion.isvisible = true;
                    currentGameState = (int)GameState.Game;
                    Debug.Write("yes");
                }
                if (keyboard.IsKeyDown(Keys.R))//Load from file
                {
                    SaveLoad load = new SaveLoad(field, "LOAD", Scores, skyMan.scoreInfo);
                    field = load.AfterLoad();
                    skyMan.scoreInfo = load.ReturnSkyMan();
                    skyMan.lives = skyMan.scoreInfo.lives;
                    skyMan.score = skyMan.scoreInfo.score;
                }
                if (keyboard.IsKeyDown(Keys.S))//Saves to file
                {
                    SaveLoad save = new SaveLoad(field, "SAVE", Scores, skyMan.scoreInfo);
                }
                if (keyboard.IsKeyDown(Keys.Q))//Quits the game
                {
                    Exit();
                }
                if (skyMan.lives <= 0)//Moves game to the Insert name Screen
                {

                    skyMan.Isvisible = false;
                    flag.Isvisible = false;
                    GraphicsDevice.Clear(Color.Blue);
                    cheese.Isvisible = false;
                    rokets.Isvisible = false;
                    boulder.Isvisible = false;
                    explosion.isvisible = false;
                    
                    Gameover.Isvisible = true;
                    for (int i = 0; i < 3; i++)
                    {
                        Letters[i].Isvisible = true;
                    }
                    currentGameState = (int)GameState.GameOver;
                }
                if (keyboard.IsKeyDown(Keys.Escape))//Opens the Pause menu
                {
                    cheese.Isvisible = false;
                    rokets.Isvisible = false;
                    boulder.Isvisible = false;
                    skyMan.Isvisible = false;
                    flag.Isvisible = false;
                    explosion.isvisible = false;
                    currentGameState = (int)GameState.Pause;
                }
                
            }

            if(currentGameState == (int)GameState.Win)
            {
                ResumeButton.Isvisible = true;
                if (ResumeButton.IsPressed(mouse) == true)//gose back to game mode
                {
                    Save.Isvisible = false;
                    Load.Isvisible = false;
                    ResumeButton.Isvisible = false;
                    BackToStartButton.Isvisible = false;
                    cheese.Isvisible = true;
                    rokets.Isvisible = false;
                    boulder.Isvisible = true;
                    gamefinished = true;
                    currentGameState = (int)GameState.Game;
                }
            }

            if (currentGameState == (int)GameState.Pause)//menu for saving, loading and going back to main menu
            {
                Save.Isvisible = true;
                Load.Isvisible = true;
                ResumeButton.Isvisible = true;
                BackToStartButton.Isvisible = true;
                if (Save.IsPressed(mouse) == true)//save
                {
                    SaveLoad save = new SaveLoad(field, "SAVE", Scores, skyMan.scoreInfo);
                }
                if (Load.IsPressed(mouse) == true)//load
                {
                    SaveLoad load = new SaveLoad(field, "LOAD",Scores, skyMan.scoreInfo);
                    field = load.AfterLoad();
                    skyMan.scoreInfo = load.ReturnSkyMan();
                    skyMan.lives = skyMan.scoreInfo.lives;
                    skyMan.score = skyMan.scoreInfo.score;
                }
                if (ResumeButton.IsPressed(mouse) == true)//gose back to game mode
                {
                    Save.Isvisible = false;
                    Load.Isvisible = false;
                    ResumeButton.Isvisible = false;
                    BackToStartButton.Isvisible = false;
                    cheese.Isvisible = true;
                    rokets.Isvisible = false;
                    boulder.Isvisible = true;
                    currentGameState = (int)GameState.Game;
                }
                if (BackToStartButton.IsPressed(mouse) == true)//back to main menu
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
                
                for (int i = 0; i < 3; i++)//used to update the letters dail
                {                   
                    Letters[i].Update();
                }
                GraphicsDevice.Clear(Color.Black);
             
                if ( keyboard.IsKeyDown(Keys.S) && (!keyboard.Equals(Oldkeyboard)))//previous letter
                {
                   Letters[count].Up();
                }
                if ( keyboard.IsKeyDown(Keys.W) && (!keyboard.Equals(Oldkeyboard)))//next letter
                {
                    Letters[count].Down();                  
                }
                if (keyboard.IsKeyDown(Keys.Enter) && (!keyboard.Equals(Oldkeyboard)))//used to go from the first dail to the next one
                {
                    count++;
                }
                

                
                if (count == 3)//checks to see if all the letters are done
                {
                    flag.Reset(field);
                    
                    count = 0;
                    
                    currentGameState = (int)GameState.Atract;
                    Gameover.Isvisible = false;
                    flag.Isvisible = true;
                    skyMan.Isvisible = true;
                    flag.atractmode = true;
                    
                    for (int i = 0; i < 3; i++)
                    {
                        Letters[i].Isvisible = false;
                        skyMan.name += Letters[i].CurrentLetter();
                    }
                   
                    SaveLoad load = new SaveLoad(field, "LOAD", Scores, skyMan.scoreInfo);//takes the high score form the file
                    Scores = load.ReturnScores();
                   
                    Score currentscore = new Score();
                    currentscore.PlayerName = skyMan.name;
                    currentscore.PlayerScore = skyMan.score;
                    Scores.Add(currentscore);//adds this sesion high score to the list
                    Scores.Sort((a, b) => b.PlayerScore.CompareTo(a.PlayerScore));//sorts the list
                    SaveLoad save = new SaveLoad(field, "SAVE", Scores, skyMan.scoreInfo);//saves to file
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
            flag.DrawInAttract(Scores, spriteBatch, font);
            for (int i = 0; i < 3; i++)//draws the letters one by one
            {
                Letters[i].DrawWithAnimation(spriteBatch);
            }
            cheese.Draw(spriteBatch);
            rokets.Draw(spriteBatch);
            boulder.Draw(spriteBatch);
            Bond.Draw(spriteBatch);
            Gameover.Draw(spriteBatch);
            explosion.Draw(spriteBatch);
            skyMan.DrawWithAnimation(spriteBatch);
            skyMan.DrawScore(spriteBatch, font);            
            ComicPage.Draw(spriteBatch);
            SkipComic.Draw(spriteBatch);
            smallexplosion.Draw(spriteBatch);

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
