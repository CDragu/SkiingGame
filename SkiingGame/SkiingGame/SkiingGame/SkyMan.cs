﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SkiingGame
{
    public class SkyMan : PhysicsObject
    {
        
        private bool isvisible;
        public int speed = 2;
        public int lives;
        public int score;
        public int timer;
        public string name;
        public ScoreInfo scoreInfo;
        public int WindowHeight;
        public int WindowLenght;

        public struct ScoreInfo
        {
            public int score;
            public int lives;
        }

        public bool Isvisible
        {
            get { return isvisible; }
            set { isvisible = value; }
        }
       
        public SkyMan(Vector2 position, float scale, Texture2D texture, float rotation, float transparency, PlayField field, int WindowHeight, int WindowLenght ) : base(position, scale, texture, rotation, transparency, field)
        {
            this.Position = position;
            this.Scale = scale;
            this.Rotation = rotation;
            this.Texture = texture;
            this.Transparency = transparency;
            this.WindowHeight = WindowHeight;
            this.WindowLenght = WindowLenght;
            children = new List<Sprite>();
            Phizicalchildren = new List<PhysicsObject>();
            field.Addtoplayfield(this);
            lives = 3;
            score = 0;
            InitializeAnimation(70, 130, 0.2f, 0.16f);
        }

        public override void Update()
        {
            
            KeyboardState keyboard = Keyboard.GetState();
             if (keyboard.IsKeyDown(Keys.Down) || keyboard.IsKeyDown(Keys.S))
            {
                this.Position += new Vector2(0, +speed);
                currentFrame = 0;
            }
             if (keyboard.IsKeyDown(Keys.Up) || keyboard.IsKeyDown(Keys.W))
            {
                this.Position += new Vector2(0, -speed);
                currentFrame = 0;
            }
             if(keyboard.IsKeyDown(Keys.Left) || keyboard.IsKeyDown(Keys.A))
            {
                this.Position += new Vector2(-speed, 0);
                currentFrame = 1;
            }
             if(keyboard.IsKeyDown(Keys.Right) || keyboard.IsKeyDown(Keys.D))
            {
                this.Position += new Vector2(+speed, 0);
                currentFrame = 2;
            }
             if(this.Position.X < 0)
            {
                this.Position +=new  Vector2(speed,0);
            }
             if(this.Position.X > WindowLenght - Texture.Width*Scale)
            {
                this.Position += new Vector2(-speed, 0);
            }
             if (this.Position.Y < 0)
            {
                this.Position += new Vector2(0, speed);
            }
            if (this.Position.Y > WindowHeight - Texture.Height * Scale)
            {
                this.Position += new Vector2(0, -speed);
            }
            hitbox = new Rectangle((int)Position.X, (int)Position.Y, (int)Math.Ceiling(Texture.Width * Scale), (int)Math.Ceiling(Texture.Height * Scale));
            ScoreUP();
            scoreInfo.score = this.score;
            scoreInfo.lives = this.lives;
            SetSourceRect();
        }

        public override void Collision(PhysicsObject Obj1, PhysicsObject Obj2)
        {
            if (Obj2.HasBeenHit == false)
            {
                if(Obj2.type == "Cheese")
                {
                    this.lives++;
                    this.score += 500;
                    Obj2.Position = new Vector2(+500, +500);
                }
                if (Obj2.type == "Boulder")
                {
                    Hit();
                    Obj2.Position = new Vector2(+500, +500);
                }
                if (Obj2.type == "Flags")
                {
                    Hit();
                }                
                Obj2.HasBeenHit = true;
            }
        }

        public void Hit()
        {
            lives--;
        }

        public void ScoreUP()
        {
            timer++;
            if (timer > 10)
            {
                score++;
                timer = 0;
            }
        }
        public void SetName(string name)
        {
            this.name = name;
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            if(Isvisible == true)
                base.Draw(spriteBatch);
        }
        public override void DrawWithAnimation(SpriteBatch spriteBatch)
        {
            if (Isvisible == true)
                base.DrawWithAnimation(spriteBatch);
        }

        public void DrawScore(SpriteBatch spriteBatch , SpriteFont font)
        {
            if (Isvisible == true)
            {
                spriteBatch.DrawString(font, "Score: " + score.ToString(), new Vector2(40, 450), Color.Black);
                spriteBatch.DrawString(font, "Lives: " + lives.ToString(), new Vector2(170, 450), Color.Black);
            }
        }
        
        public void UpdateInAttract()
        {
            time+=0.15f;
            Vector2 SinPosition = new Vector2((float)(Math.Sin(time) * 0.05f*time), -this.Texture.Height * Scale);
            this.Position = new Vector2(120, 200) + SinPosition;
        }
        public void Reset()
        {
            this.Position = new Vector2(100, 100);
            lives = 3;
            score = 0;
            name = "";
            time = 0;
        }
        
    }
}
