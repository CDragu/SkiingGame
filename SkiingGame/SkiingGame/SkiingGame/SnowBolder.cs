using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SkiingGame
{
    class SnowBolder : PhysicsObject
    {
        Random rnd = new Random();
        private bool isvisible;
        public int maxboulders;
        

        public bool Isvisible
        {
            get { return isvisible; }
            set { isvisible = value; }
        }

        public SnowBolder(Vector2 position, float scale, Texture2D texture, float rotation, float transparency, PlayField field, int maxboulders) : base(position, scale, texture, rotation, transparency, field)
        {
            this.Position = position;
            this.Scale = scale;
            this.Rotation = rotation;
            this.Texture = texture;
            this.Transparency = transparency;
            this.maxboulders = maxboulders;// the number of boulders in the scene, this way we have an object with multiple childs and we modifie only the position of the childs.
            this.Active = false;
            children = new List<Sprite>();
            Phizicalchildren = new List<PhysicsObject>();
            field.Addtoplayfield(this);
            this.type = "Boulder";
        }

        public override void Update()
        {
            hitbox = new Rectangle((int)Position.X, (int)Position.Y, (int)Math.Ceiling(Texture.Width * Scale), (int)Math.Ceiling(Texture.Height * Scale));//moves the rectangle with the object
            int Newrnd = rnd.Next(1, 150);//generates a random number
            if (Newrnd == 80)
            {
                Activate(480);
            }
            for (int i = 0; i < Phizicalchildren.Count; i++)// runs through all the childrens of the object
            {
                if (Phizicalchildren[i].Active == true)
                {
                    Phizicalchildren[i].Position += new Vector2(1, 0);//moves the cheese across the screen

                }
                if(Phizicalchildren[i].Position.X > 400)
                {
                    Deactivate(Phizicalchildren[i]);//stops the cheese from being updated
                    Phizicalchildren[i].HasBeenHit = false;
                }
                if(Phizicalchildren[i].Position.X < 260 && Phizicalchildren[i].Position.X > 10)
                {
                    Phizicalchildren[i].Active = true;
                }
                Phizicalchildren[i].Update();
            }
            Rotation++;

        }
        /// <summary>
        /// creates the chilren = boulders
        /// </summary>
        /// <param name="field"></param>
        public void InitializeBoulders(PlayField field)
        {
            for(int i = 0; i < maxboulders; i++)
            {
                this.Phizicalchildren.Add(new SnowBolder(new Vector2(-100,-100), 0.2f, this.Texture, 0, 1, field, maxboulders));
            }
        }

        /// <summary>
        /// checks to see with of the boulders is not in use and activates only one of the ones that are free
        /// </summary>
        /// <param name="Haithofscreen"></param>
        public void Activate(int Haithofscreen)
        {
            for (int i = 0; i < Phizicalchildren.Count; i++)
            {
                if(Phizicalchildren[i].Active == false)
                {
                    Vector2 cheeseposition = new Vector2(-50, rnd.Next(50, Haithofscreen - 50));
                    Phizicalchildren[i].Active = true;
                    Phizicalchildren[i].Position = cheeseposition;
                    break;

                }
            }
        }
        public void Deactivate(PhysicsObject p)
        {
            p.Active = false;
        }
        /// <summary>
        /// for when the level is restarted, reinitializes the children of the class
        /// </summary>
        public void Reset()
        {
            for (int i = 0; i < Phizicalchildren.Count; i++)
            {
                Phizicalchildren[i].Active = false;
                Phizicalchildren[i].Position = new Vector2(-100, -100);
            }
        }
        /// <summary>
        /// draws the children on the screen
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (isvisible == true)
                foreach (PhysicsObject Children in this.Phizicalchildren)
                {
                    Vector2 ChildPosition = new Vector2(this.Position.X + Children.Position.X, this.Position.Y + Children.Position.Y);
                    spriteBatch.Draw(Children.Texture, ChildPosition, null, Color.White * Children.Transparency, Children.Rotation, new Vector2(this.Texture.Height/2,this.Texture.Width/2), Children.Scale, SpriteEffects.None, 0f);
                }
        }
    }
}
