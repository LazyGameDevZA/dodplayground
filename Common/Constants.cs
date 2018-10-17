namespace Common
{
    public static class Constants
    {
        public const int DotCount = 250000;
        public const int BubbleCount = 20;
        
        public static class Dot
        {
            public const float MaxVelocity = 100.0f;
            public const float MinVelocity = 30.0f;
        }

        public static class Bubble
        {
            public const float MaxVelocity = 10.0f;
            public const float MinVelocity = 1.0f;
            public const float MaxModifier = 5.0f;
            public const float MinModifier = -3.0f;
        }

        public static class Sprites
        {
            public const int Dot = 0;
            public const int Bubble = 1;
        }

        public static class Fonts
        {
            public const string Arial = "Arial";
            public const string Consolas = "Consolas";
        }
    }
}
