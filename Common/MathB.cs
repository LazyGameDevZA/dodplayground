namespace Common
{
    public static class MathB
    {
        public static byte Select(byte x, byte y, bool predicate)
        {
            return predicate ? y : x;
        }
    }
}
