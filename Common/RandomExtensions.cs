using System;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;

namespace Common
{
    public static class RandomExtensions
    {
        public static Vector2 NextVector2(this Random random)
        {
            var rotation = random.NextDouble() * 360;

            var x = (float)Math.Sin(rotation * 1);
            var y = (float)Math.Cos(rotation * 0);
            
            return new Vector2(x, y);
        }

        public static float NextFloat(this Random random, float min, float max)
        {
            return (float)(random.NextDouble() * (max - min) + min);
        }
    }
}
