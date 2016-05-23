using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace SkiingGame
{
    public class Wings:ParticleEmiter
    {
        public Wings(Texture2D[] textures, Vector2 position, int maxparticles, int numberoftextures, int particleLife) : base(textures,position,maxparticles,numberoftextures,particleLife)
        {

        }
        public float time=0;
        public bool oneburst = false;
        public int burstammount = 200;
        public override void Update()
        {
            time += 0.3f;
           
            for (int i = 0; i < maxparticles && oneburst == false; i+=5)//fills the list with particles
            {
                if (particleList[i] == null && particleList[i + 1] == null && particleList[i +2] == null && particleList[i + 3] == null && particleList[i + 4] == null && particleList[i + 5] == null)
                {
                    particleList[i] = GenerateNewRightParticle(time, new Vector2(this.position.X + 10, this.position.Y-5));
                    particleList[i+1] = GenerateNewRightParticle(time, new Vector2(this.position.X + 10, this.position.Y));
                    particleList[i+2] = GenerateNewRightParticle(time , new Vector2(this.position.X + 10, this.position.Y+5));
                    particleList[i + 3] = GenerateNewLeftParticle(time, new Vector2(this.position.X - 10, this.position.Y - 5));
                    particleList[i + 4] = GenerateNewLeftParticle(time, new Vector2(this.position.X - 10, this.position.Y));
                    particleList[i + 5] = GenerateNewLeftParticle(time, new Vector2(this.position.X - 10, this.position.Y + 5));
                    burstammount-=5;
                    break;
                }
               
                if (burstammount < 120)
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

        public Particle GenerateNewRightParticle(float time, Vector2 Position)
        {
            Texture2D texture = textureList[random.Next(numberoftextures)];
            Vector2 position = Position + new Vector2(0, (float)Math.Sin(time) * 7);
            Vector2 velocity = new Vector2(5, 3);
            float angle = 0;
            float angularVelocity = 0f;
            Color color = new Color(
                        255,
                        255,
                        0);
            float size = 0.45f;
            int lifetime = particleLife;

            return new Particle(texture, position, velocity, angle, angularVelocity, color, size, lifetime);
        }

        public Particle GenerateNewLeftParticle(float time, Vector2 Position)
        {
            Texture2D texture = textureList[random.Next(numberoftextures)];
            Vector2 position = Position + new Vector2(0,(float)Math.Sin(time)*7);
            Vector2 velocity = new Vector2(-5, 3);
            float angle = 0;
            float angularVelocity = 0f;
            Color color = new Color(
                        255,
                        255,
                        0);
            float size = 0.45f;
            int lifetime = particleLife;

            return new Particle(texture, position, velocity, angle, angularVelocity, color, size, lifetime);
        }

        public void reset()
        {
            this.oneburst = false;
            burstammount = 200;
            time = 0;
        }
    }
}
