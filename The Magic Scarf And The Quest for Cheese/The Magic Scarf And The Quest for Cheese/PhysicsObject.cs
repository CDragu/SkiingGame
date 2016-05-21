using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace The_Magic_Scarf_And_The_Quest_for_Cheese
{
    public class PhysicsObject: Sprite
    {
        public Rectangle hitbox;
        public List<PhysicsObject> Phizicalchildren;

        public bool Active;
        public string type;



        public PhysicsObject(Vector2 position, float scale, Texture2D texture, float rotation, float transparency, PlayField field) : base(position, scale, texture, rotation, transparency, field)
        {
            this.Position = position;
            this.Scale = scale;
            this.Rotation = rotation;
            this.Texture = texture;
            this.Transparency = transparency;

            children = new List<Sprite>();
            Phizicalchildren = new List<PhysicsObject>();
            field.Addtoplayfield(this);
            hitbox = new Rectangle((int)position.X, (int)position.Y, (int)Math.Ceiling(texture.Width * scale), (int)Math.Ceiling(texture.Height * scale));
        }
        public override void Update()
        {
            hitbox = new Rectangle((int)Position.X, (int)Position.Y, (int)Math.Ceiling(Texture.Width * Scale), (int)Math.Ceiling(Texture.Height * Scale));
        }
        /// <summary>
        /// checks for the collision 
        /// </summary>
        /// <param name="obj1"></param>
        /// <param name="obj2"></param>
        public void PhysicsUpdate(PhysicsObject obj1, PhysicsObject obj2)
        {
            if (obj1.hitbox.Intersects(obj2.hitbox))
            {
                Collision(obj1, obj2);
            }
        }
        public virtual void Collision(PhysicsObject Obj1, PhysicsObject Obj2)//override this
        {

        }
    }
}
