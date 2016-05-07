using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace The_Magic_Scarf_And_The_Quest_for_Cheese
{
    class Flags : PhysicsObject
    {

        private bool isvisible;
        int ScreenHeight;
        public int numberOfFlags;
        float distance;
        public bool atractmode;
        new float time = 0;
        float speed = 1;
        Texture2D SecondTexture;// the right flag

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
            hitbox = new Rectangle((int)Position.X, (int)Position.Y, (int)Math.Ceiling(Texture.Width * Scale), (int)Math.Ceiling(Texture.Height * Scale));//moves the rectangle used to detect collisions

            for (int i = 0; i < numberOfFlags; i += 2)//gose through evey pair of flags in the scene
            {
                Phizicalchildren[i].Position += new Vector2(0, speed);//moves them
                Phizicalchildren[i + 1].Position += new Vector2(0, speed);
                if (Phizicalchildren[i].Position.Y > 480)//if they are off screen it resets their y and then calculates an x using the sin funcion and the time elapses
                {
                    Vector2 SinPosition = new Vector2((float)(Math.Sin(time) * 0.05f * time), -this.Texture.Height * Scale);
                    Phizicalchildren[i].Position = SinPosition + new Vector2(1500 / (distance * 0.9f) + 20, 0);
                    Phizicalchildren[i + 1].Position = SinPosition + new Vector2(distance + 20, 0);
                    Phizicalchildren[i].HasBeenHit = false;//resets the colision flag
                    Phizicalchildren[i + 1].HasBeenHit = false;
                }
                Phizicalchildren[i].Update();//updates the children
                Phizicalchildren[i + 1].Update();
            }
            distance -= 0.02f;
            if(time < 10)
                time += 0.16f;// aproximate time for a frame
            if(speed < 5)
                speed += time * 0.000001f;//increasing the speed over time
        }

        /// <summary>
        /// checks to see if the player has passed every pair of flags
        /// </summary>
        /// <param name="skyman"></param>
        public void IncreaseScore(SkyMan skyman)
        {
            for (int i = 0; i < numberOfFlags; i += 2)
            {
                if (Phizicalchildren[i].Position.Y > skyman.Position.Y && Phizicalchildren[i].HasBeenHit == false && skyman.Position.X > Phizicalchildren[i].Position.X && skyman.Position.X < Phizicalchildren[i + 1].Position.X)
                {
                    skyman.score++;// if yes then increment the score
                    Phizicalchildren[i].HasBeenHit = true;
                    Phizicalchildren[i + 1].HasBeenHit = true;

                }
            }

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
        /// <summary>
        /// called at load content to create the childrens (flags)
        /// </summary>
        /// <param name="field"></param>
        public void SetupFlags(PlayField field)
        {
            numberOfFlags = (int)Math.Ceiling((ScreenHeight + (this.Texture.Height * Scale)) / ((this.Texture.Height * Scale) / 2));
            for (int i = 0; i < numberOfFlags; i += 2)
            {
                Vector2 SinPosition = new Vector2((float)(Math.Sin(time) * 0.05f * time), (-i * this.Texture.Height * Scale) / 2);
                this.Phizicalchildren.Add(new Flags(SinPosition += new Vector2(33, 0), 0.2f, this.Texture, 0, 1, field, numberOfFlags, distance, SecondTexture));
                this.Phizicalchildren.Add(new Flags(SinPosition += new Vector2(distance - 20, 0), 0.2f, SecondTexture, 0, 1, field, numberOfFlags, distance, SecondTexture));
            }
        }
        /// <summary>
        /// movind the flags in a more simple way for the demo mode.
        /// </summary>
        public void UpdateInAttract()
        {
            for (int i = 0; i < numberOfFlags; i += 2)
            {
                Phizicalchildren[i].Position += new Vector2(0, 1);
                Phizicalchildren[i + 1].Position += new Vector2(0, 1);
                if (Phizicalchildren[i].Position.Y > 480)
                {
                    Vector2 SinPosition = new Vector2((float)(Math.Sin(time) * 0.05f * time), -this.Texture.Height * Scale);
                    Phizicalchildren[i].Position = SinPosition += new Vector2(33, 0);
                    Phizicalchildren[i + 1].Position = SinPosition + new Vector2(distance-20, 0);
                    Phizicalchildren[i].HasBeenHit = false;
                    Phizicalchildren[i + 1].HasBeenHit = false;
                }
                Phizicalchildren[i].Update();
                Phizicalchildren[i + 1].Update();
            }
        }
        /// <summary>
        /// draws the flags and the high scores list
        /// </summary>
        /// <param name="Scores"></param>
        /// <param name="spriteBatch"></param>
        /// <param name="font"></param>
        public void DrawInAttract(List<Game1.Score> Scores, SpriteBatch spriteBatch, SpriteFont font)
        {
            if (isvisible == true && atractmode == true)
            {
                if (Scores != null)
                    for (int i = 0; i < Scores.Count; i++)
                    {
                        spriteBatch.DrawString(font, (i + 1) + "." + Scores[i].PlayerName.ToString() + "  " + Scores[i].PlayerScore.ToString(), new Vector2(90, 20 + (i * 10)), Color.White);
                    }
                spriteBatch.DrawString(font, "Press Space To Try Agian!!", new Vector2(20, 400), Color.White);
            }
        }
        /// <summary>
        /// resets the postion, time, and the ishit check
        /// </summary>
        /// <param name="field"></param>
        public void Reset(PlayField field)
        {
            this.Position = Vector2.Zero;
            distance = 200;
            time = 0;
            for (int i = 0; i < numberOfFlags; i += 2)
            {
                Vector2 SinPosition = new Vector2((float)(Math.Sin(time) * 0.05f * time), (-i * this.Texture.Height * Scale) / 2);
                this.Phizicalchildren[i].Position = (SinPosition += new Vector2(33, 0));
                this.Phizicalchildren[i + 1].Position = (SinPosition += new Vector2(distance - 20, 0));
                this.Phizicalchildren[i].HasBeenHit = false;
                this.Phizicalchildren[i + 1].HasBeenHit = false;
            }

        }
    }
}
