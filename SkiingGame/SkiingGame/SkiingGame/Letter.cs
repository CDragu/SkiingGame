using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace SkiingGame
{
    class Letter:Sprite
    {
        int currentLettr;
        private bool isvisible;

        public bool Isvisible
        {
            get { return isvisible; }
            set { isvisible = value; }
        }

        public Letter(Vector2 position, float scale, Texture2D texture, SoundEffectInstance soundEffect, float rotation, float transparency, PlayField field) : base(position, scale, texture, rotation, transparency, field)
        {
            this.Position = position;
            this.Scale = scale;
            this.Rotation = rotation;
            this.Texture = texture;
            this.Transparency = transparency;
            children = new List<Sprite>();
            field.Addtoplayfield(this);
            this.soundEffectInstance = soundEffect;
            currentLettr = 0;
            isvisible = false;
            this.InitializeAnimation(40, 50, 0.2f, 0.16f);
        }

        public override void Update()
        {
            currentFrame = currentLettr;
            SetSourceRect();
        }

        public void Up()
        {
            currentLettr++;
            if (currentLettr > 28)
                currentLettr = 0;           
        }

        public void Down()
        {
            currentLettr--;
            if (currentLettr < 0)
                currentLettr = 28;            
        }

        public string CurrentLetter()
        {
            int value = 65 + currentLettr;
            return Char.ConvertFromUtf32(value).ToString();
        }

        public void Reset()
        {
            currentLettr = 0;
        }

        public override void DrawWithAnimation(SpriteBatch spriteBatch)
        {
            if(Isvisible == true)
                base.DrawWithAnimation(spriteBatch);
        }

    }
}
