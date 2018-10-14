using System;
using Common;
using Microsoft.Xna.Framework;

namespace ComponentGame.Utilities
{
    public static class RandomExtensions
    {
        public static Vector2 NextVelocity(this Random random, float minVelocity, float maxVelocity)
        {
            var direction = random.NextVector2();
            var speed = random.NextFloat(minVelocity, maxVelocity);
            var velocity = direction * speed;

            return velocity;
        }
    }
}
