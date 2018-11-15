using System;
using System.Runtime.CompilerServices;

namespace MultiThreadComponentGame.Utilities
{
    public static class ArrayExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ArraySegment<T> Slice<T>(this T[] array, int offset, int count)
        {
            return new ArraySegment<T>(array, offset, count);
        }
    }
}
