using System.Runtime.CompilerServices;

namespace Common
{
    public static class MathB
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte Select(byte x, byte y, bool predicate)
        {
            return predicate ? y : x;
        }
    }
}
