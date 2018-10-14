using System;
using Common;
using Microsoft.Xna.Framework;
using static CompositionGame.Utilities.ComponentUtilities;
using MathF = Common.MathF;

namespace CompositionGame.Components
{
    internal class VelocityComponent : Component
    {
        private readonly float minVelocity, maxVelocity;
        
        public Vector2 Value;
        
        private WorldBoundsComponent bounds;
        private PositionComponent position;

        public VelocityComponent(Random random, float minVelocity, float maxVelocity)
        {
            var direction = random.NextVector2();
            var speed = random.NextFloat(minVelocity, maxVelocity);
            var velocity = direction * speed;

            this.Value = velocity;
            this.minVelocity = minVelocity;
            this.maxVelocity = maxVelocity;
        }

        public override void Start()
        {
            this.bounds = FindOfType<WorldBoundsComponent>();
            this.position = this.GameObject.GetComponent<PositionComponent>();
        }

        public override void Update(float deltaTime)
        {
            this.position.Value += this.Value * deltaTime;

            var x = MathF.Select(1, -1, position.Value.X < this.bounds.MinX || position.Value.X > this.bounds.MaxX);
            var y = MathF.Select(1, -1, position.Value.Y < this.bounds.MinY || position.Value.Y > this.bounds.MaxY);
            var multiply = new Vector2(x, y);

            this.Value *= multiply;
            
            this.position.Value = new Vector2(position.Value.X.Clamp(this.bounds.MinX, this.bounds.MaxX), position.Value.Y.Clamp(this.bounds.MinY, this.bounds.MaxY));
        }

        public void SetVelocity(Vector2 velocity)
        {
            this.Value = velocity.ClampMagnitude(this.minVelocity, this.maxVelocity);
        }
    }
}
