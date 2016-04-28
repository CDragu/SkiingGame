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
            this.maxboulders = maxboulders;
            this.Active = false;
            children = new List<Sprite>();
            Phizicalchildren = new List<PhysicsObject>();
            field.Addtoplayfield(this);
            hitbox = new Rectangle((int)position.X, (int)position.Y, (int)Math.Ceiling(texture.Width * scale), (int)Math.Ceiling(texture.Height * scale));
            this.type = "Boulder";
        }

        public override void Update()
        {
            hitbox = new Rectangle((int)Position.X, (int)Position.Y, (int)Math.Ceiling(Texture.Width * Scale), (int)Math.Ceiling(Texture.Height * Scale));
            int Newrnd = rnd.Next(1, 150);
            if (Newrnd == 80)
            {
                Activate(480);
            }
            for (int i = 0; i < Phizicalchildren.Count; i++)
            {
                if (Phizicalchildren[i].Active == true)
                {
                    Phizicalchildren[i].Position += new Vector2(1, 0);
                   
                }
                if(Phizicalchildren[i].Position.X > 270)
                {
                    Deactivate(Phizicalchildren[i]);
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
        
        public void InitializeBoulders(PlayField field)
        {
            for(int i = 0; i < maxboulders; i++)
            {
                this.Phizicalchildren.Add(new SnowBolder(new Vector2(-100,-100), 0.2f, this.Texture, 0, 1, field, maxboulders));
            }
        }

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
        public void Reset()
        {
            for (int i = 0; i < Phizicalchildren.Count; i++)
            {
                Phizicalchildren[i].Active = false;
                Phizicalchildren[i].Position = new Vector2(-100, -100);
            }
        }
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
