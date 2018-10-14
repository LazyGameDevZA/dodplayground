using System;
using Microsoft.Xna.Framework;
using static CompositionGame.Utilities.ComponentUtilities;

namespace CompositionGame.Components
{
    internal class PositionComponent : Component
    {
        private readonly Random random;
        
        public Vector2 Value;

        public PositionComponent(Random random)
        {
            this.random = random;
        }

        public override void Start()
        {
            var worldBoundsComponent = FindOfType<WorldBoundsComponent>();
            var x = random.Next(worldBoundsComponent.MinX, worldBoundsComponent.MaxX);
            var y = random.Next(worldBoundsComponent.MinY, worldBoundsComponent.MaxY);

            this.Value = new Vector2(x, y);
        }
    }
}
