namespace ComponentGame.Components
{
    public readonly struct VelocityConstraintComponent
    {
        public readonly float Min, Max;

        public VelocityConstraintComponent(float min, float max)
        {
            this.Min = min;
            this.Max = max;
        }
    }
}
