using System;
using Common;
using Microsoft.Xna.Framework;
using MathF = Common.MathF;

namespace OOGame
{
    internal abstract class GameObject
    {
        private readonly Random random;
        
        public abstract Color SpriteColor { get; }
        public int SpriteIndex { get; }
        public Vector2 Position { get; private set; }
        
        protected abstract Vector2 Velocity { get; set; }
        private int boundX;
        private int boundY;

        protected GameObject(int spriteIndex, Random random)
        {
            this.SpriteIndex = spriteIndex;
            this.random = random;
        }

        public virtual void Initialize(GraphicsDeviceManager graphics)
        {
            this.boundX = graphics.PreferredBackBufferWidth;
            this.boundY = graphics.PreferredBackBufferHeight;
            
            this.Position = new Vector2(this.random.Next(this.boundX), this.random.Next(this.boundY));
        }

        public virtual void Update(GameTime gameTime)
        {
            this.Position += this.Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            const int minX = 0;
            var maxX = this.boundX;
            const int minY = 0;
            var maxY = this.boundY;

            var x = MathF.Select(1, -1, this.Position.X < minX || this.Position.X > maxX);
            var y = MathF.Select(1, -1, this.Position.Y < minY || this.Position.Y > maxY);
            var multiply = new Vector2(x, y);

            this.Velocity *= multiply;

            this.Position = new Vector2(this.Position.X.Clamp(minX, maxX), this.Position.Y.Clamp(minY, maxY));
        }
    }
}
