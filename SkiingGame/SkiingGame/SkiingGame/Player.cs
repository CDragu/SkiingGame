using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkiingGame
{
    class Player : Sprite
    {
        public Player(Vector2 position, float scale, Texture2D texture, float rotation, float transparency, PlayField field) : base(position,scale,texture,rotation,transparency,field)
        {
            this.Position = position;
            this.Scale = scale;
            this.Rotation = rotation;
            this.Texture = texture;
            this.Transparency = transparency;
            children = new List<Sprite>();
            field.Addtoplayfield(this);
            
        }
        public override void Reset(RunSequence game) { }

        public override void Update()
        {            
            KeyboardState keyboard = Keyboard.GetState();
            if (keyboard.IsKeyDown(Keys.A))
            {
                RunAnimation(5,7,0.5f);
                Position = Position + new Vector2(-2, 0) ;
            }
            else if (keyboard.IsKeyDown(Keys.D))
            {
                RunAnimation(9, 11, 0.5f);
                Position = Position + new Vector2(2, 0);
            }
            else
            {
                RunAnimation(0, 0, 0.5f);
            }
            

        }
    }
}
