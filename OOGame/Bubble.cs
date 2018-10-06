using System;
using Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ObjectOriented;
using MathF = Common.MathF;

namespace OOGame
{
    internal class Bubble : IGameObject
    {
        public float PositionX { get; private set; }
        public float PositionY { get; private set; }

        public float VelocityModifier { get; private set; }

        private const float minModifier = -3.0f;
        private const float maxModifier = 5.0f;
        private const float maxVelocity = 10.0f;
        private const float minVelocity = 1.0f;
        
        private readonly Random random;

        private int boundX;
        private int boundY;

        private float velocityX;
        private float velocityY;

        private byte red;
        private byte green;
        private byte alpha;

        private SpriteBatch spriteBatch;
        private Texture2D texture2D;

        public float HorizontalSize => this.texture2D.Width / 2;
        public float VerticalSize => this.texture2D.Height / 2;

        public Bubble(Random random)
        {
            this.random = random;
        }

        public void Initialize(GraphicsDeviceManager graphics)
        {
            this.boundX = graphics.PreferredBackBufferWidth;
            this.boundY = graphics.PreferredBackBufferHeight;
            
            this.PositionX = this.random.Next(this.boundX);
            this.PositionY = this.random.Next(this.boundY);

            var velocityMagnitude = (float)this.random.NextDouble() * (maxVelocity - minVelocity) + minVelocity;
            var velocity = this.random.NextVector2() * velocityMagnitude;

            this.velocityX = velocity.X;
            this.velocityY = velocity.Y;

            this.VelocityModifier = ((float)this.random.NextDouble() * (maxModifier - minModifier) + minModifier);

            this.red = MathB.Select(0, byte.MaxValue, this.VelocityModifier < 0.0f);
            this.green = MathB.Select(0, byte.MaxValue, this.VelocityModifier >= 0.0f);
            var scaleMax = MathF.Select(minModifier, maxModifier, this.VelocityModifier >= 0.0f);
            this.alpha = (byte)(int)(128 * (this.VelocityModifier / scaleMax));
        }

        public void LoadContent(SpriteBatch spriteBatch, Texture2D texture2D)
        {
            this.spriteBatch = spriteBatch;
            this.texture2D = texture2D;
        }

        public void Update(GameTime gameTime)
        {
            this.PositionX += this.velocityX * (float)gameTime.ElapsedGameTime.TotalSeconds;
            this.PositionY += this.velocityY * (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            var minX = 0 + this.HorizontalSize;
            var maxX = this.boundX - this.HorizontalSize;
            var minY = 0 + this.VerticalSize;
            var maxY = this.boundY - this.VerticalSize;

            if(this.PositionX < minX || this.PositionX > maxX)
            {
                this.velocityX *= -1;
            }

            if(this.PositionY < minY || this.PositionY > maxY)
            {
                this.velocityY *= -1;
            }

            this.PositionX = this.PositionX.Clamp(minX, maxX);
            this.PositionY = this.PositionY.Clamp(minY, maxY);
        }

        public void Draw(GameTime gameTime)
        {
            var position = new Vector2(this.PositionX, this.PositionY);
            var color = new Color(this.red, this.green, byte.MinValue, this.alpha);
            var origin = new Vector2(this.HorizontalSize, this.VerticalSize);
            this.spriteBatch.Draw(this.texture2D, position, null, color, 0.0f, origin, Vector2.One, SpriteEffects.None, 0.0f);
        }
    }
}
