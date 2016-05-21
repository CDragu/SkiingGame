using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace The_Magic_Scarf_And_The_Quest_for_Cheese
{
    class SmartSprite : Sprite
    {
        private bool isvisible = false;

        public bool Isvisible
        {
            get { return isvisible; }
            set { isvisible = value; }
        }
        public SmartSprite(Vector2 position, float scale, Texture2D texture, float rotation, float transparency, PlayField field) : base(position, scale, texture, rotation, transparency, field)
        {
            this.Position = position;
            this.Scale = scale;
            this.Rotation = rotation;
            this.Texture = texture;
            this.Transparency = transparency;
            this.isvisible = false;
            children = new List<Sprite>();

            field.Addtoplayfield(this);

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Isvisible == true)
                base.Draw(spriteBatch);
        }
    }
}
