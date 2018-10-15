using System.Runtime.CompilerServices;

namespace Common
{
    public static class Math
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Select(int x, int y, bool predicate)
        {
            return predicate ? y : x;
        }
    }
}
