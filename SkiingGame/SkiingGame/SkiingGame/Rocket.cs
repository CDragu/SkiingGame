using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SkiingGame
{
    class Rocket : PhysicsObject
    {
        Random rnd = new Random();
        private bool isvisible;
        public int maxrockets;


        public bool Isvisible
        {
            get { return isvisible; }
            set { isvisible = value; }
        }

        public Rocket(Vector2 position, float scale, Texture2D texture, float rotation, float transparency, PlayField field, int maxrockets) : base(position, scale, texture, rotation, transparency, field)
        {
          
            this.maxrockets = maxrockets;// the number of maxrockets in the scene, this way we have an object with multiple childs and we modifie only the position of the childs.
            this.Active = false;
            children = new List<Sprite>();
            Phizicalchildren = new List<PhysicsObject>();
            field.Addtoplayfield(this);
            hitbox = new Rectangle((int)position.X, (int)position.Y, (int)Math.Ceiling(texture.Width * scale), (int)Math.Ceiling(texture.Height * scale));
            this.type = "Rockets";
        }

        public override void Update()
        {
            hitbox = new Rectangle((int)Position.X, (int)Position.Y, (int)Math.Ceiling(Texture.Width * Scale), (int)Math.Ceiling(Texture.Height * Scale)); // moves the hitbox to the position of the object
            int Newrnd = rnd.Next(1, 200);
            if (Newrnd == 80)
            {
                Activate(272);
            }
            for (int i = 0; i < Phizicalchildren.Count; i++) // runs through all the childrens of the object
            {
                if (Phizicalchildren[i].Active == true)
                {
                    Phizicalchildren[i].Position += new Vector2(0, -3);//moves the rocketposition across the screen

                }
                if (Phizicalchildren[i].Position.Y < -50)
                {
                    Deactivate(Phizicalchildren[i]);//stops the rocketposition from being updated
                    Phizicalchildren[i].HasBeenHit = false;
                }
                if (Phizicalchildren[i].Position.Y < 480 && Phizicalchildren[i].Position.Y > 0)
                {
                    Phizicalchildren[i].Active = true;
                }
                Phizicalchildren[i].Update();
            }

        }
        /// <summary>
        /// creates the chilren = cheese
        /// </summary>
        /// <param name="field"></param> the list of sprites that is used to save the game
        public void Initialize(PlayField field)
        {
            for (int i = 0; i < maxrockets; i++)
            {
                this.Phizicalchildren.Add(new Rocket(new Vector2(-100, -100), 2f, this.Texture, 0, 1, field, maxrockets));
            }
        }
        /// <summary>
        /// checks to see with of the cheeses is not in use and activates only one of the ones that are free
        /// </summary>
        /// <param name="Haithofscreen"></param>
        public void Activate(int Haithofscreen)
        {
            for (int i = 0; i < Phizicalchildren.Count; i++)
            {
                if (Phizicalchildren[i].Active == false)
                {
                    Vector2 rocketposition = new Vector2(rnd.Next(50, Haithofscreen - 50), 520);
                    Phizicalchildren[i].Active = true;
                    Phizicalchildren[i].Position = rocketposition;
                    break;

                }
            }
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
        public void Deactivate(PhysicsObject p)
        {
            p.Active = false;
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
                    spriteBatch.Draw(Children.Texture, ChildPosition, null, Color.White * Children.Transparency, Children.Rotation, Vector2.Zero, Children.Scale, SpriteEffects.None, 0f);
                }
        }
    }
}
