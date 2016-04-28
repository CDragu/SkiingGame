using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SkiingGame
{
    class Explosion : ParticleEmiter
    {
        public bool isvisible = false;

        public bool Isvisible
        {
            get { return isvisible; }
            set { isvisible = value; }
        }

        public Explosion(Texture2D[] textures, Vector2 position, int maxparticles, int numberoftextures, int particleLife) : base(textures,position,maxparticles,numberoftextures,particleLife)
        {
            
        }

        public override Particle GenerateNewParticle()
        {
            Texture2D texture = textureList[random.Next(numberoftextures)];
            Vector2 position = this.position;
            Vector2 velocity = new Vector2(0, 2f);
            float angle = 0;
            float angularVelocity = 0f;
            Color color = new Color(
                        (float)random.NextDouble(),
                        (float)random.NextDouble(),
                        (float)random.NextDouble());
            float size = 0.5f;
            int lifetime = particleLife;

            return new Particle(texture, position, velocity, angle, angularVelocity, color, size, lifetime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (isvisible == true)
            {
                base.Draw(spriteBatch);
            }
            
        }

    }
}
