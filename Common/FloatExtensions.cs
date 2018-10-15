using System.Runtime.CompilerServices;

namespace Common
{
    public static class FloatExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Clamp(this float x, float min, float max)
        {
            var minSelect = System.MathF.Max(x, min);
            return System.MathF.Min(minSelect, max);
        }
    }
}
