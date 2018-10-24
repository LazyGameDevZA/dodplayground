using System;
using Common;
using Microsoft.Xna.Framework;
using MathF = Common.MathF;
using static Common.Constants;
using static Common.Constants.Bubble;

namespace OOGame
{
    internal class Bubble : GameObject
    {
        private Color spriteColor;

        public override Color SpriteColor => this.spriteColor;

        public float VelocityModifier { get; private set; }

        private readonly Random random;

        public static float Size => 64;

        public Bubble(Random random)
            : base(Sprites.Bubble, random)
        {
            this.random = random;
        }

        protected override Vector2 Velocity { get; set; }

        public override void Initialize(GraphicsDeviceManager graphics)
        {
            base.Initialize(graphics);

            var velocityMagnitude = (float)this.random.NextDouble() * (MaxVelocity - MinVelocity) + MinVelocity;
            this.Velocity = this.random.NextVector2() * velocityMagnitude;

            this.VelocityModifier = (float)this.random.NextDouble() * (MaxModifier - MinModifier) + MinModifier;

            var red = MathB.Select(0, byte.MaxValue, this.VelocityModifier < 0.0f);
            var green = MathB.Select(0, byte.MaxValue, this.VelocityModifier >= 0.0f);
            var scaleMax = MathF.Select(MinModifier, MaxModifier, this.VelocityModifier >= 0.0f);
            var alpha = (byte)(int)(128 * (this.VelocityModifier / scaleMax));
            this.spriteColor = new Color(red, green, byte.MinValue, alpha);
        }
    }
}
