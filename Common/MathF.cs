namespace Common
{
    public static class MathF
    {
        public static float Select(float x, float y, bool predicate)
        {
            return predicate ? y : x;
        }
    }
}
