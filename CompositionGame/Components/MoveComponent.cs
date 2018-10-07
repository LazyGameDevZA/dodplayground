using System;
using Common;
using static CompositionGame.Utilities.ComponentUtilities;

namespace CompositionGame.Components
{
    internal class MoveComponent : Component
    {
        public float VelX, VelY;
        public WorldBoundsComponent bounds;

        public MoveComponent(Random random, float minSpeed, float maxSpeed)
        {
            var direction = random.NextVector2();
            var speed = random.NextFloat(minSpeed, maxSpeed);
            var velocity = direction * speed;

            this.VelX = velocity.X;
            this.VelY = velocity.Y;
        }

        public override void Start()
        {
            this.bounds = FindOfType<WorldBoundsComponent>();
        }

        public override void Update(float deltaTime)
        {
            var position = this.GameObject.GetComponent<PositionComponent>();

            position.X += this.VelX * deltaTime;
            position.Y += this.VelY * deltaTime;

            if(position.X < this.bounds.MinX || position.X > this.bounds.MaxX)
            {
                this.VelX *= -1;
            }

            if(position.Y < this.bounds.MinY || position.Y > this.bounds.MaxY)
            {
                this.VelY *= -1;
            }
            
            position.X = position.X.Clamp(this.bounds.MinX, this.bounds.MaxX);
            position.Y = position.Y.Clamp(this.bounds.MinY, this.bounds.MaxY);
        }
    }
}
