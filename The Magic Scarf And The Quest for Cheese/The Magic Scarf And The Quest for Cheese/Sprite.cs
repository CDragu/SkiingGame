using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;


namespace The_Magic_Scarf_And_The_Quest_for_Cheese
{
    public class Sprite
    {
        public struct Info//custom structure used to save the info of the sprite
        {
            public Vector2 position;
            public float scale;
            public float rotation;
            public bool hasbeenHit;
            public Texture2D texture;
        }
        /// <summary>
        /// Normal Variables
        /// </summary>
        private Vector2 position;
        private float scale;
        private float rotation;
        private float transparency;
        private Texture2D texture;
        public List<Sprite> children;//every child can move in another way or with the main object
        private bool hasbeenHit;
        /// <summary>
        /// Sound Variables
        /// </summary>
        public SoundEffectInstance soundEffectInstance;
        public AudioEmitter emitter = new AudioEmitter();
        public AudioListener listener = new AudioListener();

        /// <summary>
        /// Animation
        /// </summary>        
        public int currentFrame;
        public int spriteWidth;
        public int spriteHeight;
        public float spriteSpeed;
        public double time;
        public float frameduration;

        private Rectangle sourceRect;

        /// <summary>
        /// Initialization
        /// </summary>

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public float Scale
        {
            get { return scale; }
            set { scale = value; }
        }

        public float Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }

        public float Transparency
        {
            get { return transparency; }
            set { transparency = value; }
        }

        public Texture2D Texture
        {
            get { return texture; }
            set { texture = value; }
        }

        public bool HasBeenHit
        {
            get { return hasbeenHit; }
            set { hasbeenHit = value; }
        }

        /// <summary>
        /// Construct
        /// </summary>
        public Sprite(Vector2 position, float scale, Texture2D texture, SoundEffectInstance soundEffect, float rotation, float transparency, PlayField field)
        {
            this.position = position;
            this.scale = scale;
            this.rotation = rotation;
            this.texture = texture;
            this.transparency = transparency;
            children = new List<Sprite>();
            field.Addtoplayfield(this);
            this.soundEffectInstance = soundEffect;
        }
        public Sprite(Vector2 position, float scale, Texture2D texture, float rotation, float transparency, PlayField field)
          : this(position, scale, texture, null, rotation, transparency, field)
        {

        }
        public Sprite(Vector2 position, float scale, Texture2D texture, float rotation, PlayField field)
            : this(position, scale, texture, null, rotation, 1f, field)
        {

        }
        public Sprite(Vector2 position, float scale, Texture2D texture, PlayField field)
            : this(position, scale, texture, 0f, 1f, field)
        {
        }
        public Sprite(Vector2 position, Texture2D texture, PlayField field)
            : this(position, 1f, texture, 0f, 1f, field)
        {
        }
        public Sprite(Texture2D texture, PlayField field)
            : this(Vector2.Zero, 1f, texture, 0f, 1f, field)
        {
        }

        /// <summary>
        /// Draw
        /// </summary>
        /// <param name="spriteBatch"></param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {

            spriteBatch.Draw(texture, position, null, Color.White * transparency, rotation, Vector2.Zero, scale, SpriteEffects.None, 0f);//draw the main object
            foreach (Sprite Children in this.children)//gose through the list and draws any of the children
            {
                Vector2 ChildPosition = new Vector2(this.position.X + Children.position.X, this.position.Y + Children.position.Y);
                spriteBatch.Draw(Children.texture, ChildPosition, null, Color.White * Children.transparency, Children.rotation, Vector2.Zero, Children.scale, SpriteEffects.None, 0f);
            }
        }

        /// <summary>
        /// Animation Stuff
        /// </summary>
        public void SetSourceRect()//calculates the rectangle that will be drawn on the screen
        {
            sourceRect = new Rectangle(currentFrame * spriteWidth, 0, spriteWidth, spriteHeight);
        }

        public void InitializeAnimation(int spriteWidth, int spriteHeight, float spriteSpeed, float frameduration)//sets the variables
        {
            this.spriteWidth = spriteWidth;
            this.spriteHeight = spriteHeight;
            this.spriteSpeed = spriteSpeed;
            this.frameduration = frameduration;
        }
        public void RunAnimation(int startingframe, int totalduration, float frameduration)// Runs an animation that takes totalduration frames and starts form starting frame. frameduratin is for time, not used
        {
            time = time + 0.16f;// time till last update on a 30FPS game
            if (currentFrame < startingframe)
            {
                currentFrame = startingframe;
            }
            if (time > frameduration)
            {
                currentFrame++;
                SetSourceRect();
                time = 0;
            }
            if (currentFrame > totalduration)
            {
                currentFrame = startingframe;
                SetSourceRect();
            }

        }
        public void UpdateAnimation(int currentframe)//to be overriten
        {
            this.currentFrame = currentframe;
            SetSourceRect();
        }

        public virtual void DrawWithAnimation(SpriteBatch spriteBatch)// drwas the sprite using the sourceRect informations
        {

            spriteBatch.Draw(texture, position, sourceRect, Color.White * transparency, rotation, Vector2.Zero, scale, SpriteEffects.None, 0f);
            foreach (Sprite Children in this.children)
            {
                Vector2 ChildPosition = new Vector2(this.position.X + Children.position.X, this.position.Y + Children.position.Y);
                spriteBatch.Draw(Children.texture, ChildPosition, Children.sourceRect, Color.White * Children.transparency, Children.rotation, Vector2.Zero, Children.scale, SpriteEffects.None, 0f);
            }
        }



        /// <summary>
        /// Normal Stuff
        /// </summary>
        public virtual void Update()//to be overriten
        {

        }

        /// <summary>
        /// Save Function Stuff
        /// </summary>
        /// <returns></returns>
        public Info Save()// returns the structure filed with information
        {
            Info info = new Info();
            info.position = this.position;
            info.scale = this.scale;
            info.rotation = this.rotation;
            info.hasbeenHit = this.hasbeenHit;
            return info;
        }
        public void Load(Info info)// replaces the current info with the ones form the file
        {
            this.position = info.position;
            this.scale = info.scale;
            this.rotation = info.rotation;
            this.hasbeenHit = info.hasbeenHit;
        }

        /// <summary>
        /// Audio Stuff
        /// </summary>
        public void PlayAudio()
        {
            soundEffectInstance.Apply3D(listener, emitter);
            //soundEffectInstance.Play();//only when the sound will be implemented
        }
    }
}
