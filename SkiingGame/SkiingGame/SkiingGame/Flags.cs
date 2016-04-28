using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SkiingGame
{
    class Flags : PhysicsObject
    {
        private bool isvisible;
        int ScreenHeight;
        public int numberOfFlags;
        float distance;
        public bool atractmode;
        new float time = 0;
        float speed =1;
        Texture2D SecondTexture;

        public bool Isvisible
        {
            get { return isvisible; }
            set { isvisible = value; }
        }
        public Flags(Vector2 position, float scale, Texture2D texture, float rotation, float transparency, PlayField field, int ScreenHeight, float distance, Texture2D SecondTexture) : base(position, scale, texture, rotation, transparency, field)
        {
            this.Position = position;
            this.Scale = scale;
            this.Rotation = rotation;
            this.Texture = texture;
            this.Transparency = transparency;
            children = new List<Sprite>();
            Phizicalchildren = new List<PhysicsObject>();
            field.Addtoplayfield(this);
            this.ScreenHeight = ScreenHeight;
            this.distance = distance;
            this.type = "Flags";
            this.SecondTexture = SecondTexture;
        }

        public override void Update()
        {
            hitbox = new Rectangle((int)Position.X, (int)Position.Y, (int)Math.Ceiling(Texture.Width * Scale), (int)Math.Ceiling(Texture.Height * Scale));
           
            for (int i = 0; i < numberOfFlags; i+=2)
            {
                Phizicalchildren[i].Position += new Vector2(0, speed);
                Phizicalchildren[i + 1].Position += new Vector2(0, speed);
                if (Phizicalchildren[i].Position.Y > 480)
                {
                    Vector2 SinPosition = new Vector2((float)(Math.Sin(time) * 0.05f * time),-this.Texture.Height * Scale);
                    Phizicalchildren[i].Position = SinPosition + new Vector2(1500/(distance*0.9f)+20, 0);
                    Phizicalchildren[i+1].Position = SinPosition + new Vector2(distance+20, 0);
                    Phizicalchildren[i].HasBeenHit = false;
                    Phizicalchildren[i+1].HasBeenHit = false;
                }
                Phizicalchildren[i].Update();
                Phizicalchildren[i+1].Update();
            }
            distance -= 0.02f;
            time += 0.16f;
            speed += time*0.000001f;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (isvisible == true)
                foreach (PhysicsObject Children in this.Phizicalchildren)
                {
                    Vector2 ChildPosition = new Vector2(this.Position.X + Children.Position.X, this.Position.Y + Children.Position.Y);
                    spriteBatch.Draw(Children.Texture, ChildPosition, null, Color.White * Children.Transparency, Children.Rotation, Vector2.Zero, Children.Scale, SpriteEffects.None, 0f);
                }
        }
        public void SetupFlags(PlayField field)
        {
            numberOfFlags = (int)Math.Ceiling((ScreenHeight + (this.Texture.Height * Scale)) / ((this.Texture.Height*Scale)/2));
            for (int i = 0; i< numberOfFlags; i+=2)
            {
                Vector2 SinPosition = new Vector2((float)(Math.Sin(time) * 0.05f * time), (-i * this.Texture.Height * Scale)/2);
                this.Phizicalchildren.Add(new Flags(SinPosition += new Vector2(33, 0), 0.2f, this.Texture, 0, 1, field, numberOfFlags, distance, SecondTexture));
                this.Phizicalchildren.Add(new Flags(SinPosition += new Vector2(distance - 20, 0), 0.2f, SecondTexture, 0, 1, field, numberOfFlags, distance, SecondTexture));
            }
        }

        public void UpdateInAttract()
        {
            for (int i = 0; i < numberOfFlags; i += 2)
            {
                Phizicalchildren[i].Position += new Vector2(0, 1);
                Phizicalchildren[i + 1].Position += new Vector2(0, 1);
                if (Phizicalchildren[i].Position.Y > 480)
                {
                    Vector2 SinPosition = new Vector2((float)(Math.Sin(time) * 0.05f * time), -this.Texture.Height * Scale);
                    Phizicalchildren[i].Position = SinPosition;
                    Phizicalchildren[i + 1].Position = SinPosition + new Vector2(distance, 0);
                    Phizicalchildren[i].HasBeenHit = false;
                    Phizicalchildren[i + 1].HasBeenHit = false;
                }
                Phizicalchildren[i].Update();
                Phizicalchildren[i + 1].Update();
            }
        }
        
        public void DrawInAttract(List<RunSequence.Score> Scores, SpriteBatch spriteBatch, SpriteFont font)
        {
            if (isvisible == true && atractmode == true)
            {
              for(int i  = 0; i < Scores.Count; i++)
                {
                    spriteBatch.DrawString(font, Scores[i].PlayerName.ToString() + "  " + Scores[i].PlayerScore.ToString(), new Vector2(100, 20+(i*10)), Color.White);
                }
                spriteBatch.DrawString(font, "Press Space To Try Agian!!", new Vector2(20, 400), Color.White);
            }
        }
        public void Reset(PlayField field)
        {
            this.Position = Vector2.Zero;
            distance = 200;
            time = 0;
            for (int i = 0; i < numberOfFlags; i += 2)
            {
                Vector2 SinPosition = new Vector2((float)(Math.Sin(time) * 0.05f * time), (-i * this.Texture.Height * Scale) / 2);
                this.Phizicalchildren[i].Position =(SinPosition += new Vector2(33, 0));
                this.Phizicalchildren[i+1].Position =(SinPosition += new Vector2(distance - 20, 0));
            }

        }
    }
}
