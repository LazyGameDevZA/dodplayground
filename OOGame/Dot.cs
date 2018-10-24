using System;
using System.Collections.Generic;
using Common;
using Microsoft.Xna.Framework;
using static Common.Constants.Dot;
using static Common.Constants;
using static OOGame.Utilities.GameObjectUtilities;

namespace OOGame
{
    internal class Dot : GameObject
    {
        private Color spriteColor;
        private static IList<Bubble> bubbles;

        public override Color SpriteColor => this.spriteColor;

        private readonly Random random;

        public Dot(Random random)
            : base(Sprites.Dot, random)
        {
            this.random = random;
        }

        protected override Vector2 Velocity { get; set; }

        public override void Initialize(GraphicsDeviceManager graphics)
        {
            base.Initialize(graphics);

            var velocityMagnitude = (float)this.random.NextDouble() * (MaxVelocity - MinVelocity) + MinVelocity;
            this.Velocity = this.random.NextVector2() * velocityMagnitude;

            var colors = new byte[3];
            this.random.NextBytes(colors);

            this.spriteColor = new Color(colors[0], colors[1], colors[2], byte.MaxValue);

            if(bubbles != null && bubbles.Count != 0)
            {
                return;
            }

            bubbles = FindAllObjectsOfType<Bubble>();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            for(int i = 0; i < bubbles.Count; i++)
            {
                var bubble = bubbles[i];
                var diff = this.Position - bubble.Position;

                if(System.MathF.Pow(Bubble.Size, 2) < diff.LengthSquared())
                {
                    continue;
                }
                
                this.Velocity = this.Velocity + this.Velocity * (bubble.VelocityModifier * (float)gameTime.ElapsedGameTime.TotalSeconds);
                this.Velocity = this.Velocity.ClampMagnitude(MinVelocity, MaxVelocity);
            }
        }
    }
}
