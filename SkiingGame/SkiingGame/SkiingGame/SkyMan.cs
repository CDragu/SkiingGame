using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SkiingGame
{
    class SkyMan : PhysicsObject
    {
        private bool isvisible;

        public bool Isvisible
        {
            get { return isvisible; }
            set { isvisible = value; }
        }

        public SkyMan(Vector2 position, float scale, Texture2D texture, float rotation, float transparency, PlayField field) : base(position, scale, texture, rotation, transparency, field)
        {
            this.Position = position;
            this.Scale = scale;
            this.Rotation = rotation;
            this.Texture = texture;
            this.Transparency = transparency;
            children = new List<Sprite>();
            Phizicalchildren = new List<PhysicsObject>();
            field.Addtoplayfield(this);           
        }

        public override void Update()
        {
            
            KeyboardState keyboard = Keyboard.GetState();
             if (keyboard.IsKeyDown(Keys.Down) || keyboard.IsKeyDown(Keys.S))
            {
                this.Position += new Vector2(0, +2);
            }
             if (keyboard.IsKeyDown(Keys.Up) || keyboard.IsKeyDown(Keys.W))
            {
                this.Position += new Vector2(0, -2);
            }
             if(keyboard.IsKeyDown(Keys.Left) || keyboard.IsKeyDown(Keys.A))
            {
                this.Position += new Vector2(-2, 0);
            }
             if(keyboard.IsKeyDown(Keys.Right) || keyboard.IsKeyDown(Keys.D))
            {
                this.Position += new Vector2(+2, 0);
            }
            hitbox = new Rectangle((int)Position.X, (int)Position.Y, (int)Math.Ceiling(Texture.Width * Scale), (int)Math.Ceiling(Texture.Height * Scale));

        }

        public override void Collision(PhysicsObject Obj1, PhysicsObject Obj2)
        {
            Obj1.Position = new Vector2(100, 100);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if(Isvisible == true)
                base.Draw(spriteBatch);
        }

    }
}
