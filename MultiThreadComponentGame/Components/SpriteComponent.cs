namespace MultiThreadComponentGame.Components
{
    public readonly struct SpriteComponent
    {
        public readonly byte ColorR, ColorG, ColorB, Alpha;
        public readonly int Index;

        public SpriteComponent(byte colorR, byte colorG, byte colorB, byte alpha, int index)
        {
            this.ColorR = colorR;
            this.ColorG = colorG;
            this.ColorB = colorB;
            this.Alpha = alpha;
            this.Index = index;
        }
    }
}
