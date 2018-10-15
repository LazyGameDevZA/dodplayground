using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using static System.MathF;

namespace Common
{
    public static class Vector2Extensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 ClampMagnitude(this Vector2 vector2, float min, float max)
        {
            var magSquared = vector2.SquareMagnitude();

            if(magSquared < Pow(min, 2))
            {
                return vector2 / vector2.Magnitude() * min;
            }

            if(magSquared > Pow(max, 2))
            {
                return vector2 / vector2.Magnitude() * max;
            }

            return vector2;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float SquareMagnitude(this Vector2 vector2)
        {
            return Pow(vector2.X, 2) + Pow(vector2.Y, 2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Magnitude(this Vector2 vector2)
        {
            return Sqrt(vector2.SquareMagnitude());
        }
    }
}
