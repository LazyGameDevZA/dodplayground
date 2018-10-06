namespace Common
{
    public static class FloatExtensions
    {
        public static float Clamp(this float x, float min, float max)
        {
            if(x < min)
            {
                return min;
            }

            if(x > max)
            {
                return max;
            }

            return x;
        }
    }
}
