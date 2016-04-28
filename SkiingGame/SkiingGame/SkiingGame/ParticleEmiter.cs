using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SkiingGame
{
    public class ParticleEmiter
    {
        public Random random;
        public Vector2 position;
        public Particle[] particleList;
        public int maxparticles;
        public Texture2D[] textureList;
        public int numberoftextures;
        public int particleLife;

        public ParticleEmiter(Texture2D[] textures, Vector2 position, int maxparticles, int numberoftextures, int particleLife)
        {
            particleList = new Particle[maxparticles];
            textureList = new Texture2D[numberoftextures];
            this.position = position;
            this.textureList = textures;
            this.maxparticles = maxparticles;
            this.numberoftextures = numberoftextures;
            this.particleLife = particleLife;         
            random = new Random();
        }

        public void Update()
        {
            

            for (int i = 0; i < maxparticles; i++)
            {
                if(particleList[i] == null)
                {
                    particleList[i] = GenerateNewParticle();
                    break;
                }
                    
            }

            for (int particle = 0; particle < maxparticles; particle++)
            {
                if(particleList[particle] != null)
                {
                    particleList[particle].Update();
                    if (particleList[particle].Lifetime <= 0)
                     {
                         particleList[particle] = null;
                     }
                }
                
            }
        }

        public virtual Particle GenerateNewParticle()
        {
            Texture2D texture = textureList[random.Next(numberoftextures)];
            Vector2 position = this.position;
            Vector2 velocity = new Vector2(1f,0f);
            float angle = 0;
            float angularVelocity =0f;
            Color color = new Color(
                        (float)random.NextDouble(),
                        (float)random.NextDouble(),
                        (float)random.NextDouble());
            float size = 0.4f;
            int lifetime = particleLife + random.Next(40);

            return new Particle(texture, position, velocity, angle, angularVelocity, color, size, lifetime);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            
            for (int i = 0; i < maxparticles; i++)
            {
                if(particleList[i] != null)
                particleList[i].Draw(spriteBatch);
            }
            
        }
    }
}
