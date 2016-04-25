using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SkiingGame
{
    class Buttons : Sprite
    {
        private bool isvisible;

        public bool Isvisible
        {
            get { return isvisible; }
            set { isvisible = value; }
        }
        Microsoft.Xna.Framework.Input.MouseState ms = Microsoft.Xna.Framework.Input.Mouse.GetState();

        public Buttons(Vector2 position, float scale, Texture2D texture, float rotation, float transparency, PlayField field, bool isvisible) : base(position, scale, texture, rotation, transparency, field)
        {
            this.Position = position;
            this.Scale = scale;
            this.Rotation = rotation;
            this.Texture = texture;
            this.Transparency = transparency;
            this.isvisible = isvisible;
        }

        public bool IsPressed()
        {
            if (isvisible == true)
            {
                if (ms.X > (this.Position.X - Texture.Width / 2) && ms.X < (this.Position.X + Texture.Width / 2) && ms.Y > (this.Position.Y - Texture.Height / 2) && ms.Y < (this.Position.Y + Texture.Height / 2) && ms.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                        return true;
                    else
                        return false;
            }
            else
                return false;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (isvisible == true)
                base.Draw(spriteBatch);
        }
    }
}
