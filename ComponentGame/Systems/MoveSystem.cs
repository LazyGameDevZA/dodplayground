using Common;
using ComponentGame.Components;
using Microsoft.Xna.Framework;

namespace ComponentGame.Systems
{
    public class MoveSystem
    {
        private readonly int minX, maxX, minY, maxY;
        private readonly int length;
        private readonly PositionComponent[] positionComponents;
        private readonly VelocityComponent[] velocityComponents;

        public MoveSystem(GraphicsDeviceManager graphics, int length, PositionComponent[] positionComponents, VelocityComponent[] velocityComponents)
        {
            this.minX = 0;
            this.maxX = graphics.PreferredBackBufferWidth;
            this.minY = 0;
            this.maxY = graphics.PreferredBackBufferHeight;
            this.length = length;
            this.positionComponents = positionComponents;
            this.velocityComponents = velocityComponents;
        }

        public void Update(float deltaTime)
        {
            for(int i = 0; i < this.length; i++)
            {
                var position = this.positionComponents[i];

                position.X += this.velocityComponents[i].X * deltaTime;
                position.Y += this.velocityComponents[i].Y * deltaTime;

                var velocity = this.velocityComponents[i];

                if(position.X < this.minX || position.X > this.maxX)
                {
                    velocity.X *= -1;
                }

                if(position.Y < this.minY || position.Y > this.maxY)
                {
                    velocity.Y *= -1;
                }

                position.X = position.X.Clamp(this.minX, this.maxX);
                position.Y = position.Y.Clamp(this.minY, this.maxY);

                this.positionComponents[i] = position;
                this.velocityComponents[i] = velocity;
            }
        }
    }
}
