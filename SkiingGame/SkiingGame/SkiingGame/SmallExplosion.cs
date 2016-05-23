using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SkiingGame
{
    public class SmallExplosion : ParticleEmiter
    {
        public SmallExplosion(Texture2D[] textures, Vector2 position, int maxparticles, int numberoftextures, int particleLife) : base(textures,position,maxparticles,numberoftextures,particleLife)
        {

        }

        public bool oneburst = false;
        public int burstammount = 14;
        public override void Update()
        {
            int currentburstammount = burstammount;
            for (int i = 0; i < maxparticles && oneburst == false; i++)//fills the list with particles
            {
                if (particleList[i] == null && currentburstammount <= burstammount && currentburstammount >= burstammount/2)
                {
                    particleList[i] = GenerateNewParticle();
                    currentburstammount--;

                }
                if (particleList[i] == null && currentburstammount <= burstammount/2 && currentburstammount >= 0)
                {
                    particleList[i] = GenerateNewYellowParticle();
                    currentburstammount--;

                }
                if (currentburstammount == 0)
                    break;
                if (i == maxparticles - 1)
                    oneburst = true;
            }

            for (int particle = 0; particle < maxparticles; particle++)//checks the lifetime of the particle
            {
                if (particleList[particle] != null)
                {
                    particleList[particle].Update();
                    if (particleList[particle].Lifetime <= 0)
                    {
                        particleList[particle] = null;
                    }
                }

            }
        }

        public override Particle GenerateNewParticle()
        {
            Texture2D texture = textureList[random.Next(numberoftextures)];
            Vector2 position = this.position;
            //Vector2 velocity = new Vector2(random.Next(-180, 180), -random.Next(1, 3));// generates a random direction in a 180 degree arc (float)random.NextDouble() *
            Vector2 velocity = new Vector2((float)random.NextDouble() * random.Next(-3, 3), (float)random.NextDouble() * random.Next(-3, 3));
            float angle = 0;
            float angularVelocity = 0f;
            Color color = new Color(
                        (float)random.NextDouble(),
                        0,
                        0);// generates a random red color to give variety to the exposion
            float size = 0.45f;
            int lifetime = particleLife;

            return new Particle(texture, position, velocity, angle, angularVelocity, color, size, lifetime);
        }

        public  Particle GenerateNewYellowParticle()
        {
            Texture2D texture = textureList[random.Next(numberoftextures)];
            Vector2 position = this.position;
            Vector2 velocity = new Vector2((float)random.NextDouble() * random.Next(-6, 6), (float)random.NextDouble() * random.Next(-6, 6));// generates a random direction in a 180 degree arc
            float angle = 0;
            float angularVelocity = 0f;
            Color color = new Color(
                        random.Next(128,255),
                        random.Next(128, 255),
                        0);// generates a random red color to give variety to the exposion
            float size = 0.3f;
            int lifetime = particleLife;

            return new Particle(texture, position, velocity, angle, angularVelocity, color, size, lifetime);
        }

        public void reset()
        {
            this.oneburst = false;
        }

    }
}
