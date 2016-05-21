using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;


namespace The_Magic_Scarf_And_The_Quest_for_Cheese
{
    public class Particle
    {
        private Texture2D texture;
        private Vector2 position;
        private Vector2 velocity;
        private float angle;
        private float angularvelocity;
        private Color color;
        private float size;
        private int lifetime;

        public Texture2D Texture
        {
            get { return texture; }
            set { texture = value; }
        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public Vector2 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }

        public float Angle
        {
            get { return angle; }
            set { angle = value; }
        }

        public float Angularvelocity
        {
            get { return angularvelocity; }
            set { angularvelocity = value; }
        }

        public Color Color
        {
            get { return color; }
            set { color = value; }
        }

        public float Size
        {
            get { return size; }
            set { size = value; }
        }

        public int Lifetime
        {
            get { return lifetime; }
            set { lifetime = value; }
        }

        public Particle(Texture2D texture, Vector2 position, Vector2 velocity, float angle, float angularvelocity, Color color, float size, int lifetime)
        {
            Texture = texture;
            Position = position;
            Velocity = velocity;
            Angle = angle;
            Angularvelocity = angularvelocity;
            Color = color;
            Size = size;
            Lifetime = lifetime;
        }
        /// <summary>
        /// decreses the lifetime and modifies the possition acording to the velocity
        /// </summary>
        public void Update()
        {
            Lifetime--;
            Position += Velocity;
            Angle += Angularvelocity;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle sourcerectangle = new Rectangle(0, 0, Texture.Width, Texture.Height);
            Vector2 origin = new Vector2(Texture.Width / 2, Texture.Height / 2);

            spriteBatch.Draw(Texture, Position, sourcerectangle, Color, Angle, origin, Size, SpriteEffects.None, 0f);
        }

    }
}
