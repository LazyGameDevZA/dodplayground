using System;
using Common;
using Microsoft.Xna.Framework;
using static CompositionGame.Utilities.ComponentUtilities;

namespace CompositionGame.Components
{
    internal class VelocityComponent : Component
    {
        private readonly float minVelocity, maxVelocity;
        
        public float X, Y;
        private WorldBoundsComponent bounds;
        private PositionComponent position;

        public VelocityComponent(Random random, float minVelocity, float maxVelocity)
        {
            var direction = random.NextVector2();
            var speed = random.NextFloat(minVelocity, maxVelocity);
            var velocity = direction * speed;

            this.X = velocity.X;
            this.Y = velocity.Y;
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
            this.position.X += this.X * deltaTime;
            this.position.Y += this.Y * deltaTime;

            if(this.position.X < this.bounds.MinX || this.position.X > this.bounds.MaxX)
            {
                this.X *= -1;
            }

            if(this.position.Y < this.bounds.MinY || this.position.Y > this.bounds.MaxY)
            {
                this.Y *= -1;
            }
            
            this.position.X = this.position.X.Clamp(this.bounds.MinX, this.bounds.MaxX);
            this.position.Y = this.position.Y.Clamp(this.bounds.MinY, this.bounds.MaxY);
        }

        public void SetVelocity(float velX, float velY)
        {
            var velocity = new Vector2(velX, velY);
            velocity = velocity.ClampMagnitude(this.minVelocity, this.maxVelocity);

            this.X = velocity.X;
            this.Y = velocity.Y;
        }
    }
}
