using System;
using Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ObjectOriented;
using MathF = System.MathF;
using static Common.Constants.Dot;

namespace OOGame
{
    internal class Dot : IGameObject
    {
        private readonly Random random;
        private readonly Bubble[] bubbles;

        private int boundX;
        private int boundY;
        
        private float positionX;
        private float positionY;

        private float velocityX;
        private float velocityY;

        private byte red;
        private byte green;
        private byte blue;

        private SpriteBatch spriteBatch;
        private Texture2D texture2D;

        public Dot(Random random, Bubble[] bubbles)
        {
            this.random = random;
            this.bubbles = bubbles;
        }

        private float HorizontalSize => this.texture2D.Width / 2;
        private float VerticalSize => this.texture2D.Height / 2;
        
        public void Initialize(GraphicsDeviceManager graphics)
        {
            this.boundX = graphics.PreferredBackBufferWidth;
            this.boundY = graphics.PreferredBackBufferHeight;
            
            this.positionX = this.random.Next(this.boundX);
            this.positionY = this.random.Next(this.boundY);

            var velocityMagnitude = (float)this.random.NextDouble() * (MaxVelocity - MinVelocity) + MinVelocity;
            var velocity = this.random.NextVector2() * velocityMagnitude;

            this.velocityX = velocity.X;
            this.velocityY = velocity.Y;
                
            var colors = new byte[3];
            this.random.NextBytes(colors);

            this.red = colors[0];
            this.green = colors[1];
            this.blue = colors[2];
        }

        public void LoadContent(SpriteBatch spriteBatch, Texture2D texture2D)
        {
            this.spriteBatch = spriteBatch;
            this.texture2D = texture2D;
        }

        public void Update(GameTime gameTime)
        {
            this.positionX += this.velocityX * (float)gameTime.ElapsedGameTime.TotalSeconds;
            this.positionY += this.velocityY * (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            const int minX = 0;
            var maxX = this.boundX;
            const int minY = 0;
            var maxY = this.boundY;

            if(this.positionX < minX || this.positionX > maxX)
            {
                this.velocityX *= -1;
            }

            if(this.positionY < minY || this.positionY > maxY)
            {
                this.velocityY *= -1;
            }

            this.positionX = this.positionX.Clamp(minX, maxX);
            this.positionY = this.positionY.Clamp(minY, maxY);

            for(int i = 0; i < this.bubbles.Length; i++)
            {
                var bubble = this.bubbles[i];
                var diffX = MathF.Abs(this.positionX - bubble.PositionX);
                var diffY = MathF.Abs(this.positionY - bubble.PositionY);

                if(!(diffX < bubble.HorizontalSize) || !(diffY < bubble.VerticalSize))
                {
                    continue;
                }
                
                var velocity = new Vector2(this.velocityX, this.velocityY);
                velocity = velocity + velocity * (bubble.VelocityModifier * (float)gameTime.ElapsedGameTime.TotalSeconds);
                velocity = velocity.ClampMagnitude(MinVelocity, MaxVelocity);

                this.velocityX = velocity.X;
                this.velocityY = velocity.Y;
            }
        }

        public void Draw(GameTime gameTime)
        {
            var position = new Vector2(this.positionX, this.positionY);
            var color = new Color(this.red, this.green, this.blue, byte.MaxValue);
            var origin = new Vector2(this.HorizontalSize, this.VerticalSize);
            this.spriteBatch.Draw(this.texture2D, position, null, color, 0.0f, origin, Vector2.One, SpriteEffects.None, 0.0f);
        }
    }
}
