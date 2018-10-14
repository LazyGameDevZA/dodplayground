using System;
using Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ObjectOriented;
using MathF = Common.MathF;
using static Common.Constants.Dot;

namespace OOGame
{
    internal class Dot : IGameObject
    {
        private readonly Random random;
        private readonly Bubble[] bubbles;

        private int boundX;
        private int boundY;
        
        private Vector2 position;

        private Vector2 velocity;

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
            
            this.position = new Vector2(this.random.Next(this.boundX), this.random.Next(this.boundY));

            var velocityMagnitude = (float)this.random.NextDouble() * (MaxVelocity - MinVelocity) + MinVelocity;
            this.velocity = this.random.NextVector2() * velocityMagnitude;

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
            this.position += this.velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            const int minX = 0;
            var maxX = this.boundX;
            const int minY = 0;
            var maxY = this.boundY;

            var x = MathF.Select(1, -1, this.position.X < minX || this.position.X > maxX);
            var y = MathF.Select(1, -1, this.position.Y < minY || this.position.Y > maxY);
            var multiply = new Vector2(x, y);

            this.velocity *= multiply;

            this.position = new Vector2(this.position.X.Clamp(minX, maxX), this.position.Y.Clamp(minY, maxY));

            for(int i = 0; i < this.bubbles.Length; i++)
            {
                var bubble = this.bubbles[i];
                var diff = this.position - bubble.Position;

                if(System.MathF.Pow(bubble.Size, 2) < diff.LengthSquared())
                {
                    continue;
                }
                
                this.velocity = this.velocity + this.velocity * (bubble.VelocityModifier * (float)gameTime.ElapsedGameTime.TotalSeconds);
                this.velocity = this.velocity.ClampMagnitude(MinVelocity, MaxVelocity);
            }
        }

        public void Draw(GameTime gameTime)
        {
            var color = new Color(this.red, this.green, this.blue, byte.MaxValue);
            var origin = new Vector2(this.HorizontalSize, this.VerticalSize);
            this.spriteBatch.Draw(this.texture2D, this.position, null, color, 0.0f, origin, Vector2.One, SpriteEffects.None, 0.0f);
        }
    }
}
