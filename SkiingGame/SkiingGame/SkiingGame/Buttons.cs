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
        

        public Buttons(Vector2 position, float scale, Texture2D texture, float rotation, float transparency, PlayField field, bool isvisible) : base(position, scale, texture, rotation, transparency, field)
        {
            this.Position = position;
            this.Scale = scale;
            this.Rotation = rotation;
            this.Texture = texture;
            this.Transparency = transparency;
            this.isvisible = isvisible;
        }
        /// <summary>
        /// Call this method if you want to see if a butoon is pressesd
        /// </summary>
        /// <param name="ms"></param>
        /// <returns></returns>
        public bool IsPressed(Microsoft.Xna.Framework.Input.MouseState ms)
        {
            if (isvisible == true)
            {   
                if (ms.X < (this.Position.X + Texture.Width) && ms.X > (this.Position.X) && ms.Y > (this.Position.Y) && ms.Y < (this.Position.Y + Texture.Height) && ms.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)//checs to see if the cursor is in the boundary of the buttton and if it is pressed
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
