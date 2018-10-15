using System;
using Common;
using Microsoft.Xna.Framework;
using ObjectOriented;
using MathF = Common.MathF;
using static Common.Constants;
using static Common.Constants.Bubble;

namespace OOGame
{
    internal class Bubble : IGameObject
    {
        public Color SpriteColor { get; private set; }
        public int SpriteIndex => Sprites.Bubble;
        public Vector2 Position { get; private set; }

        public float VelocityModifier { get; private set; }

        private readonly Random random;

        private int boundX;
        private int boundY;

        private Vector2 velocity;

        public static float Size => 64;

        public Bubble(Random random)
        {
            this.random = random;
        }

        public void Initialize(GraphicsDeviceManager graphics)
        {
            this.boundX = graphics.PreferredBackBufferWidth;
            this.boundY = graphics.PreferredBackBufferHeight;
            
            this.Position = new Vector2(this.random.Next(this.boundX), this.random.Next(this.boundY));

            var velocityMagnitude = (float)this.random.NextDouble() * (MaxVelocity - MinVelocity) + MinVelocity;
            this.velocity = this.random.NextVector2() * velocityMagnitude;

            this.VelocityModifier = (float)this.random.NextDouble() * (MaxModifier - MinModifier) + MinModifier;

            var red = MathB.Select(0, byte.MaxValue, this.VelocityModifier < 0.0f);
            var green = MathB.Select(0, byte.MaxValue, this.VelocityModifier >= 0.0f);
            var scaleMax = MathF.Select(MinModifier, MaxModifier, this.VelocityModifier >= 0.0f);
            var alpha = (byte)(int)(128 * (this.VelocityModifier / scaleMax));
            this.SpriteColor = new Color(red, green, byte.MinValue, alpha);
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
        }
    }
}
