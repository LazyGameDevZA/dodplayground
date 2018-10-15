using System;
using Common;
using Microsoft.Xna.Framework;
using ObjectOriented;
using MathF = Common.MathF;
using static Common.Constants.Dot;
using static Common.Constants;

namespace OOGame
{
    internal class Dot : IGameObject
    {
        public Color SpriteColor { get; private set; }
        public int SpriteIndex => Sprites.Dot;
        
        private readonly Random random;
        private readonly Bubble[] bubbles;

        private int boundX;
        private int boundY;

        public Vector2 Position { get; private set; }

        private Vector2 velocity;

        public Dot(Random random, Bubble[] bubbles)
        {
            this.random = random;
            this.bubbles = bubbles;
        }

        public void Initialize(GraphicsDeviceManager graphics)
        {
            this.boundX = graphics.PreferredBackBufferWidth;
            this.boundY = graphics.PreferredBackBufferHeight;
            
            this.Position = new Vector2(this.random.Next(this.boundX), this.random.Next(this.boundY));

            var velocityMagnitude = (float)this.random.NextDouble() * (MaxVelocity - MinVelocity) + MinVelocity;
            this.velocity = this.random.NextVector2() * velocityMagnitude;

            var colors = new byte[3];
            this.random.NextBytes(colors);

            this.SpriteColor = new Color(colors[0], colors[1], colors[2], byte.MaxValue);
        }

        public void Update(GameTime gameTime)
        {
            this.Position += this.velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            const int minX = 0;
            var maxX = this.boundX;
            const int minY = 0;
            var maxY = this.boundY;

            var x = MathF.Select(1, -1, this.Position.X < minX || this.Position.X > maxX);
            var y = MathF.Select(1, -1, this.Position.Y < minY || this.Position.Y > maxY);
            var multiply = new Vector2(x, y);

            this.velocity *= multiply;

            this.Position = new Vector2(this.Position.X.Clamp(minX, maxX), this.Position.Y.Clamp(minY, maxY));

            for(int i = 0; i < this.bubbles.Length; i++)
            {
                var bubble = this.bubbles[i];
                var diff = this.Position - bubble.Position;

                if(System.MathF.Pow(Bubble.Size, 2) < diff.LengthSquared())
                {
                    continue;
                }
                
                this.velocity = this.velocity + this.velocity * (bubble.VelocityModifier * (float)gameTime.ElapsedGameTime.TotalSeconds);
                this.velocity = this.velocity.ClampMagnitude(MinVelocity, MaxVelocity);
            }
        }
    }
}
