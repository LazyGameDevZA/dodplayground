using System;
using static CompositionGame.Utilities.ComponentUtilities;

namespace CompositionGame.Components
{
    internal class PositionComponent : Component
    {
        private readonly Random random;
        
        public float X, Y;

        public PositionComponent(Random random)
        {
            this.random = random;
        }

        public override void Start()
        {
            var worldBoundsComponent = FindOfType<WorldBoundsComponent>();
            this.X = random.Next(worldBoundsComponent.MinX, worldBoundsComponent.MaxX);
            this.Y = random.Next(worldBoundsComponent.MinY, worldBoundsComponent.MaxY);
        }
    }
}
